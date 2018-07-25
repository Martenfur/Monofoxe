using Monofoxe.Engine;
using Microsoft.Xna.Framework;
using Resources.Sprites;

namespace $safeprojectname$
{
	class Test : GameObj 
	{
		Camera cam = new Camera(800, 600);

		public Test()
		{
			GameCntrl.MaxGameSpeed = 60;
			
			cam.BackgroundColor = new Color(64, 32, 32);

			GameCntrl.WindowManager.CanvasSize = new Vector2(800, 600);
			GameCntrl.WindowManager.Window.AllowUserResizing = false;
			GameCntrl.WindowManager.ApplyChanges();
			GameCntrl.WindowManager.CenterWindow();
			GameCntrl.WindowManager.CanvasMode = CanvasMode.Fill; 
		}
		
		public override void Update()
		{
			
		}

		
		public override void Draw()
		{
			DrawCntrl.DrawSprite(SpritesDefault.Monofoxe, 400, 300);
		}

	}
}