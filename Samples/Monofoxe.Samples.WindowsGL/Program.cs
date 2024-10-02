using Monofoxe.Engine.WindowsGL;
using System;

namespace Monofoxe.Samples.WindowsGL
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
