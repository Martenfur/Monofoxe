using Monofoxe.Engine.DesktopGl;
using System;


namespace Monofoxe.Playground.DesktopGL
{
	public static class Program
	{
		[STAThread]
		static void Main()
		{
			MonofoxePlatform.Init();
			using (var game = new Game1())
			{
				game.Run();
			}
		}
	}
}
