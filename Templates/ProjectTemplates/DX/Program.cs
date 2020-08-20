using System;
// using YourSharedProjectNamespace;
// Don't forget to reference it first!

namespace $safeprojectname$
{
	public static class Program
	{
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
