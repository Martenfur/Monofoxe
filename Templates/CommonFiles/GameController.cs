using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Monofoxe.Engine;
using Monofoxe.Engine.Drawing;
using Monofoxe.Engine.ECS;
using Monofoxe.Engine.SceneSystem;
using Monofoxe.Engine.Cameras;
using Monofoxe.Engine.Resources;

namespace $safeprojectname$
{
	public class GameController : Entity
	{
		public Camera Camera = new Camera(800, 600);
		Sprite _monofoxe;

		public GameController() : base(SceneMgr.GetScene("default")["default"])
		{
			GameMgr.MaxGameSpeed = 60;
			GameMgr.MinGameSpeed = 60; // Fixing framerate on 60.

			Camera.BackgroundColor = new Color(30, 24, 24);

			GameMgr.WindowManager.CanvasSize = Camera.Size;
			GameMgr.WindowManager.Window.AllowUserResizing = false;
			GameMgr.WindowManager.ApplyChanges();
			GameMgr.WindowManager.CenterWindow();
			GameMgr.WindowManager.CanvasMode = CanvasMode.Fill;
			
			GraphicsMgr.Sampler = SamplerState.PointClamp;

			_monofoxe = ResourceHub.GetResource<Sprite>("DefaultSprites", "Monofoxe");
		}
		
		public override void Update()
		{
			
		}

		
		public override void Draw()
		{
			_monofoxe.Draw(Camera.Size / 2f, _monofoxe.Origin);
		}

	}
}