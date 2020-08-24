using System;
using System.IO;

namespace Monofoxe.Pipeline
{
	/// <summary>
	/// Simple logger class. Writes info into a log file.
	/// Used for debugging.
	/// </summary>
	public static class Logger
	{
		#if DEBUG
			public static bool EnableLogging = true;
		#else
				public static bool EnableLogging = false;
		#endif

		private static string _filePath;

		/// <summary>
		/// Initializes log file.
		/// </summary>
		public static void Init(string fileName)
		{
			if (EnableLogging)
			{
				_filePath = Environment.CurrentDirectory + "/" + fileName;
				File.WriteAllText(_filePath, "");
			}
		}

		/// <summary>
		/// Writes a message and a new line into a log file.
		/// </summary>
		public static void LogLine(string message)
		{
			if (EnableLogging)
			{
				File.AppendAllText(_filePath, message + Environment.NewLine);
			}
		}

		
		/// <summary>
		/// Writes a message into a log file.
		/// </summary>
		public static void Log(string message)
		{
			if (EnableLogging)
			{
				File.AppendAllText(_filePath, message);
			}
		}

	}
}
