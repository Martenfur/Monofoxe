using System;
using Monofoxe.Engine.WindowsGL;

namespace Monofoxe.Template.GL
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
