using System;

namespace Monofoxe.Engine
{
	public delegate void LoggerDelegate(string log, LogLevel level);

	public static class Logger
	{
		public static LogLevel LogFilter = LogLevel.All;

		public static event LoggerDelegate OnLog;

		/// <summary>
		/// If true, skips a newline at the end of next log.
		/// </summary>
		private static bool _sameLine = false;

		public static void Log(object log, LogLevel level = LogLevel.Info)
		{
			if (OnLog == null || (level & LogFilter) == 0)
			{
				return;
			}

			if (_sameLine)
			{
				_sameLine = false;
				OnLog.Invoke(log.ToString(), level);
			}
			else
			{
				OnLog.Invoke(log.ToString() + Environment.NewLine, level);
			}
		}

		/// <summary>
		/// Skips a newline at the end of next log.
		/// </summary>
		public static void SameLine() =>
			_sameLine = true;
	}
}
