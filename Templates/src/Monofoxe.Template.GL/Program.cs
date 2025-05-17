using System;
using Monofoxe.Engine.DesktopGL;

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
