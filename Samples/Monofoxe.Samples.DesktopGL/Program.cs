using Monofoxe.Engine.DesktopGL;
using System;

namespace Monofoxe.Samples.DesktopGL
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
