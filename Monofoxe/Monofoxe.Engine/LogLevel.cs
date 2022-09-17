using System;

namespace Monofoxe.Engine
{
	[Flags]
	public enum LogLevel : uint
	{
		/// <summary>
		/// A log level describing events showing step by step execution of your code 
		/// that can be ignored during the standard operation, but may be useful 
		/// during extended debugging sessions.
		/// </summary>
		Trace = 1,

		/// <summary>
		/// A log level used for events considered to be useful during software debugging 
		/// when more granular information is needed.
		/// </summary>
		Debug = 2,

		/// <summary>
		/// An event happened, the event is purely informative and can be 
		/// ignored during normal operations.
		/// </summary>
		Info = 4,

		/// <summary>
		/// Unexpected behavior happened inside the application, but it is continuing its 
		/// work and the key features are operating as expected.
		/// </summary>
		Warn = 8,

		/// <summary>
		/// One or more functionalities are not working, preventing some functionalities 
		/// from working correctly.
		/// </summary>
		Error = 16,

		/// <summary>
		/// One or more key functionalities are not working and the whole system 
		/// doesn’t fulfill the functionalities.
		/// </summary>
		Fatal = 32,

		All = uint.MaxValue
	}
}
