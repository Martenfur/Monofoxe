using Monofoxe.Engine.Abstractions;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Monofoxe.Engine
{
	/// <summary>
	/// Simple key-value based storage for configuration and other things that need to be preserved between launches.
	/// </summary>
	public static class ConfigStash
	{
		public static string ConfigPath => Path.Combine(_storage.CurrentDirectory, "config.cfg");
		private static string _configFileName;

		private const char _keyValueDivider = '=';

		private static Dictionary<string, string> _entries;

		private static ILocalStorage _storage => StuffResolver.GetStuff<ILocalStorage>();

		public static string GetEntry(string key) =>
			_entries[key];


		public static void SetEntry(string key, string value)
		{
			if (key.Contains(_keyValueDivider.ToString()))
			{
				throw new ArgumentException("Keys cannot contain a '" + _keyValueDivider + "'!");
			}
			if (_entries.ContainsKey(key))
			{
				_entries[key] = value.Replace(Environment.NewLine, "\n");
				return;
			}
			_entries.Add(key, value);
		}


		/// <summary>
		/// Writes the updated config to the disk.
		/// </summary>
		public static void Sync()
		{
			var contents = new StringBuilder();
			foreach (var entry in _entries)
			{
				contents.Append(entry.Key + _keyValueDivider + entry.Value + Environment.NewLine);
			}
			_storage.WriteAllText(ConfigPath, contents.ToString());
		}


		public static bool TryGetEntry(string key, out string value) =>
			_entries.TryGetValue(key, out value);


		public static bool HasEntry(string key) =>
			_entries.ContainsKey(key);


		private static bool _initialized = false;
		public static void Init(string configFileName = "config.cfg")
		{
			if (_initialized)
			{
				return;
			}
			_initialized = true;

			_configFileName = configFileName;
			if (_entries == null)
			{
				_entries = new Dictionary<string, string>();

				if (_storage.FileExists(ConfigPath))
				{
					var rawConfig = _storage.ReadAllLines(ConfigPath);
					foreach (var entry in rawConfig)
					{
						var split = entry.Split(new char[] { _keyValueDivider }, 2);
						var key = split[0];
						var value = "";
						if (split.Length > 1)
						{
							value = split[1].Replace("\n", Environment.NewLine);
						}
						try
						{
							_entries.Add(key, value);
						}
						catch (Exception e)
						{
							Logger.Log("Error reading config entry: " + e.Message);
						}
					}
				}
			}
		}
	}
}
