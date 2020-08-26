using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Monofoxe.Engine;
using Monofoxe.Engine.Drawing;
using Monofoxe.Engine.EC;
using Monofoxe.Engine.SceneSystem;
using Monofoxe.Engine.Cameras;
using Monofoxe.Engine.Resources;

namespace $safeprojectname$
{
	public class GameController : Entity
	{
		public Camera2D Camera = new Camera2D(800, 600);
		private Sprite _monofoxe;

		public GameController() : base(SceneMgr.GetScene("default")["default"])
		{
			GameMgr.MaxGameSpeed = 60;
			GameMgr.MinGameSpeed = 60; // Fixing framerate on 60.

			Camera.BackgroundColor = Color.White;

			GameMgr.WindowManager.CanvasSize = Camera.Size;
			GameMgr.WindowManager.Window.AllowUserResizing = false;
			GameMgr.WindowManager.ApplyChanges();
			GameMgr.WindowManager.CenterWindow();
			GameMgr.WindowManager.CanvasMode = CanvasMode.Fill;
			
			GraphicsMgr.VertexBatch.SamplerState = SamplerState.PointClamp;

			_monofoxe = ResourceHub.GetResource<Sprite>("DefaultSprites", "Monofoxe");
			
			Text.CurrentFont = ResourceHub.GetResource<IFont>("Fonts", "Arial");
		}
		
		public override void Update()
		{
			base.Update();	
		}

		
		public override void Draw()
		{
			base.Draw();
			
			_monofoxe.Draw(Camera.Size / 2f);
		}

	}
}