using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Monofoxe.Engine;
using Monofoxe.Engine.Drawing;
using Monofoxe.Engine.ECS;
using Monofoxe.Engine.SceneSystem;
using Monofoxe.Engine.Cameras;
using Resources.Sprites;
using Monofoxe.Playground.Interface;
using Monofoxe.Engine.Resources;

namespace Monofoxe.Playground
{
	public class GameController : Entity
	{
		public Camera MainCamera = new Camera(800, 600);

		Layer _guiLayer;

		public static RasterizerState DefaultRasterizer;
		public static RasterizerState WireframeRasterizer;

		public GameController() : base(SceneMgr.GetScene("default")["default"])
		{
			GameMgr.MaxGameSpeed = 60;
			GameMgr.MinGameSpeed = 60; // Fixing framerate on 60.

			MainCamera.BackgroundColor = new Color(38, 38, 38);

			GameMgr.WindowManager.CanvasSize = new Vector2(800, 600);
			GameMgr.WindowManager.Window.AllowUserResizing = false;
			GameMgr.WindowManager.ApplyChanges();
			GameMgr.WindowManager.CenterWindow();
			GameMgr.WindowManager.CanvasMode = CanvasMode.KeepAspectRatio;

			//GraphicsMgr.Sampler = SamplerState.PointClamp;
			GraphicsMgr.Sampler = SamplerState.PointWrap; // WIll make textures repeat without interpolation.

			
			DefaultRasterizer = new RasterizerState();
			DefaultRasterizer.CullMode = CullMode.CullCounterClockwiseFace;
			DefaultRasterizer.FillMode = FillMode.Solid;
			DefaultRasterizer.MultiSampleAntiAlias = false;

			WireframeRasterizer = new RasterizerState();
			WireframeRasterizer.CullMode = CullMode.CullCounterClockwiseFace;
			WireframeRasterizer.FillMode = FillMode.WireFrame;
			WireframeRasterizer.MultiSampleAntiAlias = false;


			GraphicsMgr.Rasterizer = DefaultRasterizer;
			
			_guiLayer = Scene.CreateLayer("gui");
			_guiLayer.IsGUI = true;
			

			var cameraController = new CameraController(_guiLayer, MainCamera);

			var switcher = new SceneSwitcher(_guiLayer, cameraController);
			switcher.CurrentFactory.CreateScene();

			// Enabling applying postprocessing effects to separate layers.
			// Note that this will create an additional surface.
			MainCamera.PostprocessingMode = PostprocessingMode.CameraAndLayers;

		}

		public override void Update()
		{

		}


		public override void Draw()
		{
		}

	}
}