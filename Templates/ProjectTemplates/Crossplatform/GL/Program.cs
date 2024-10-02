using Monofoxe.Engine.WindowsGL;
using System;

namespace $safeprojectname$
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
