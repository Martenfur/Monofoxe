using System;

// NOTE: This project is dependent on Monofoxe solution.
// You need to build all Monofoxe libraries in Debug mode first.

namespace Monofoxe.Playground.DesktopGL
{
	/// <summary>
	/// The main class.
	/// </summary>
	public static class Program
	{
		/// <summary>
		/// The main entry point for the application.
		/// </summary>
		[STAThread]
		static void Main()
		{
			using (var game = new Game1())
			{
				game.Run();
			}
		}
	}
}
