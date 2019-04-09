using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Monofoxe.Engine;
using Monofoxe.Engine.Drawing;
using Monofoxe.Engine.ECS;
using Monofoxe.Engine.SceneSystem;
using Monofoxe.Engine.Utils;
using Monofoxe.Engine.Utils.Cameras;

namespace Monofoxe.Test
{
	public class MainTester : Entity
	{
		public static Camera MainCamera = new Camera(600, 600);
		public static Camera SecondCamera = new Camera(600, 600);	

		public RandomExt Random = new RandomExt();
		
		public MainTester() : base(SceneMgr.GetScene("default")["default"])
		{
			Random.GetListWithoutRepeats(5, -90, -10);
			Random.GetListWithoutRepeats(5, 0, 10);
			Random.GetListWithoutRepeats(5, -10, 10);

			
			var scene = SceneMgr.GetScene("default");
			var ballsLayer = scene.CreateLayer("balls", 1);
			ballsLayer.IsGUI = false;

			InitCameras();
			
			InitWindow();

			InitRasterizer();


			
			GameMgr.MaxGameSpeed = 60;
			GameMgr.FixedUpdateRate = 1.0 / 30.0;
			
			
			//LayerMgrs.Get("default").IsGUI = true;

			new DrawingTester();
			new ECSTester(ballsLayer);
			//new AlarmTester(ballsLayer);
			new TileTester(ballsLayer);
			//new StateMachineTester(ballsLayer);

			/*
			var aStart = 3000000.0;
			var divider = 0.5;
			var dt1 = 1.0/60.0;
			var dt2 = 1.0/25.0;
			
			
			var a = aStart;
			
			for(var i = 0; i < 60; i += 1)
			{
				a = a * Math.Pow(divider, dt1); 
				Console.WriteLine(a);
			}
			Console.WriteLine("60 fps: " + a);

			a = aStart;

			for(var i = 0; i < 25; i += 1)
			{
				a = a * Math.Pow(divider, dt2);
				Console.WriteLine(a);
			}
			Console.WriteLine("30 fps: " + a);

			var n = 1;
			n.ToBool();



			var v = new Vector2(0, 0);
			v = v.GetSafeNormalize();
			
			Console.WriteLine("L: " + v.Length());
			*/
			Console.WriteLine("res: " + Math.Pow(2, 1000));
		}


		
		public override void Update()
		{
			if (Input.CheckButton(Buttons.Escape))
			{
				GameMgr.ExitGame();
			}
			
			if (Input.CheckButtonPress(Buttons.P))
			{
				var scene = SceneMgr.GetScene("default");
				
				//scene["balls"].Enabled = !scene["balls"].Enabled;
				foreach(var entity in scene.GetEntityList("ball"))
				{
					entity.Enabled = !entity.Enabled;
				}
			}

			#region Camera. 

			
			if (Input.CheckButton(Buttons.Left))
			{
				MainCamera.Position.X += (5 / MainCamera.Zoom);
			}
			
			if (Input.CheckButton(Buttons.Right))
			{
				MainCamera.Position.X -= (5 / MainCamera.Zoom);
			}
			
			if (Input.CheckButton(Buttons.Up))
			{
				MainCamera.Position.Y += (5 / MainCamera.Zoom);
			}
			
			if (Input.CheckButton(Buttons.Down))
			{
				MainCamera.Position.Y -= (5 / MainCamera.Zoom);
			}
			
			if (Input.CheckButton(Buttons.Z))
			{
				MainCamera.Zoom += 0.1f;
			}
			
			if (Input.CheckButton(Buttons.X))
			{
				MainCamera.Zoom -= 0.1f;
				if (MainCamera.Zoom <= 0)
				{
					MainCamera.Zoom = 0.1f;
				}
			}
			
			if (Input.CheckButton(Buttons.C))
			{
				MainCamera.Rotation += 5;
			}

			if (Input.CheckButton(Buttons.V))
			{
				MainCamera.Rotation -= 5;
			}

			if (Input.CheckButtonPress(Buttons.F))
			{
				GameMgr.WindowManager.SetFullScreen(!GameMgr.WindowManager.IsFullScreen);	
			}
			
			#endregion Camera. 
			
		}

		public override void Draw()
		{
			
		}

		void InitCameras()
		{
			MainCamera.BackgroundColor = Color.DarkSeaGreen;
			MainCamera.ClearBackground = true;
			MainCamera.Offset = MainCamera.Size / 2;
			MainCamera.Position = MainCamera.Size / 2;
			//MainCamera.PortOffset = Vector2.One * 32;

			var layer = SceneMgr.GetScene("default")["balls"];
			var layer1 = SceneMgr.GetScene("default")["default"];
			//layer.IsGUI = false;
			MainCamera.PostprocessingMode = PostprocessingMode.CameraAndLayers;
			//MainCamera.PositiontprocessorEffects.Add(Resources.Effects.Effect);
			//layer.PostprocessorEffects.Add(Resources.Effects.BW);
			
			//layer1.PostprocessorEffects.Add(Resources.Effects.BW);
			

			SecondCamera.PortPosition.X = 600;
			SecondCamera.BackgroundColor = Color.DarkSeaGreen;
			SecondCamera.Enabled = true;
			//SecondCamera.AddFilterEntry("default", "balls");
			SecondCamera.FilterMode = FilterMode.Exclusive;
		}


		void InitWindow()
		{
			GameMgr.WindowManager.CanvasSize = new Vector2(1200, 600);
			GameMgr.WindowManager.Window.AllowUserResizing = false;
			GameMgr.WindowManager.ApplyChanges();
			GameMgr.WindowManager.CenterWindow();
			
			GameMgr.WindowManager.CanvasMode = CanvasMode.Fill; 
		}


		void InitRasterizer()
		{
			RasterizerState rasterizerState = new RasterizerState();
			rasterizerState.CullMode = CullMode.CullCounterClockwiseFace;
			rasterizerState.ScissorTestEnable = false;
			rasterizerState.FillMode = FillMode.Solid;
			
			GraphicsMgr.Rasterizer = rasterizerState;

			GraphicsMgr.Sampler = SamplerState.PointClamp;
		}

	}
}
