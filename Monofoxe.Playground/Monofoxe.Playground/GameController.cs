using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Monofoxe.Engine;
using Monofoxe.Engine.Cameras;
using Monofoxe.Engine.Drawing;
using Monofoxe.Engine.EC;
using Monofoxe.Engine.SceneSystem;
using Monofoxe.Playground.Interface;
using System.Diagnostics;

namespace Monofoxe.Playground
{
	public class GameController : Entity
	{
		public Camera2D MainCamera = new Camera2D(800, 600);

		Layer _guiLayer;

		public static RasterizerState DefaultRasterizer;
		public static RasterizerState WireframeRasterizer;

		private Stopwatch _stopwatch = new Stopwatch();
		
		public GameController() : base(SceneMgr.GetScene("default")["default"])
		{
			GameMgr.MaxGameSpeed = 60;
			GameMgr.MinGameSpeed = 60; // Fixing framerate on 60.

			MainCamera.BackgroundColor = new Color(38, 38, 38);

			GameMgr.WindowManager.CanvasSize = MainCamera.Size;
			GameMgr.WindowManager.Window.AllowUserResizing = false;
			GameMgr.WindowManager.ApplyChanges();
			GameMgr.WindowManager.CenterWindow();
			GameMgr.WindowManager.CanvasMode = CanvasMode.KeepAspectRatio;


			GraphicsMgr.VertexBatch.SamplerState = SamplerState.PointWrap; // Will make textures repeat without interpolation.

#if LINUX
			// -0.5 pixel offset because OpenGL is a snowflake.
			GraphicsMgr.VertexBatch.UsesHalfPixelOffset = true;
#endif

			DefaultRasterizer = new RasterizerState();
			DefaultRasterizer.CullMode = CullMode.CullCounterClockwiseFace;
			DefaultRasterizer.FillMode = FillMode.Solid;
			DefaultRasterizer.MultiSampleAntiAlias = false;

			WireframeRasterizer = new RasterizerState();
			WireframeRasterizer.CullMode = CullMode.CullCounterClockwiseFace;
			WireframeRasterizer.FillMode = FillMode.WireFrame;
			WireframeRasterizer.MultiSampleAntiAlias = false;

			GraphicsMgr.VertexBatch.RasterizerState = DefaultRasterizer;

			_guiLayer = Scene.CreateLayer("gui");
			_guiLayer.IsGUI = true;


			var cameraController = new CameraController(_guiLayer, MainCamera);

			var switcher = new SceneSwitcher(_guiLayer, cameraController);
			switcher.CurrentFactory.CreateScene();

			// Enabling applying postprocessing effects to separate layers.
			// Note that this will create an additional surface.
			MainCamera.PostprocessingMode = PostprocessingMode.CameraAndLayers;

			SceneMgr.OnPreDraw += OnPreDraw; // You can do the same for individual layers or scenes.
			SceneMgr.OnPostDraw += OnPostDraw;
		}


		private void OnPreDraw() =>
			_stopwatch.Start();

		private void OnPostDraw()
		{
			_stopwatch.Stop();
			GameMgr.WindowManager.WindowTitle = "Rendering time: " + _stopwatch.Elapsed;
			_stopwatch.Reset();
		}


		public override void Destroy()
		{
			base.Destroy();

			SceneMgr.OnPreDraw -= OnPreDraw;
			SceneMgr.OnPostDraw -= OnPostDraw;
		}
	}
}
