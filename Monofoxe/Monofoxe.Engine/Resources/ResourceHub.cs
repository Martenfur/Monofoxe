using System;
using System.Collections.Generic;

namespace Monofoxe.Engine.Resources
{
	/// <summary>
	/// Central place to access all game resources.
	/// Hub holds resource boxes and gives access to them.
	/// </summary>
	public static class ResourceHub
	{
		private static Dictionary<string, IResourceBox> _boxes =
			new Dictionary<string, IResourceBox>(StringComparer.OrdinalIgnoreCase);
		
		public static void UnloadAll()
		{
			foreach (var boxPair in _boxes)
			{
				boxPair.Value.Unload();
			}
		}

		public static void AddResourceBox(string key, IResourceBox box) =>
			_boxes.Add(key, box);
		
		public static void RemoveResourceBox(string key) =>
			_boxes.Remove(key);
		
		public static IResourceBox GetResourceBox(string key)
		{
			if (_boxes.TryGetValue(key,	out IResourceBox box))
			{
				if (!box.Loaded)
				{
					box.Load();
				}

				return box;
			}

			return null;
		}

		public static bool ContainsResourceBox(string key) =>
			_boxes.ContainsKey(key);



		/// <summary>
		/// Returns resource from the hub with the given name.
		/// </summary>
		public static TValue GetResource<TValue>(string boxKey, string resourceKey)
		{
			if (_boxes.TryGetValue(boxKey, out IResourceBox box))
			{
				if (!box.Loaded)
				{
					box.Load();
				}

				return ((ResourceBox<TValue>)box).GetResource(resourceKey);
			}

			return default(TValue);
		}

		/// <summary>
		/// Returns resource from the hub with the given name.
		/// </summary>
		public static bool ContainsResource<TValue>(string boxKey, string resourceKey)
		{
			if (_boxes.TryGetValue(boxKey, out IResourceBox box))
			{
				return ((ResourceBox<TValue>)box).ContainsResource(resourceKey);
			}

			return false;
		}

	}
}
