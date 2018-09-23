using System;
using Monofoxe.Engine;
using Monofoxe.Engine.Drawing;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Diagnostics;
using Monofoxe.Utils;
using Monofoxe.Engine.ECS;


namespace Monofoxe.Test
{
	public class MainTester : Entity
	{
		public static Camera MainCamera = new Camera(1440, 900);
		public static Camera SecondCamera = new Camera(600, 600);	

		public RandomExt Random = new RandomExt();

		/*
		RenderTarget2D surf = new RenderTarget2D(
				DrawMgr.Device, 
				512, 
				512, 
				false,
				DrawMgr.Device.PresentationParameters.BackBufferFormat,
				DepthFormat.Depth24, 
				0, 
				RenderTargetUsage.PreserveContents
			);
		 */


		public MainTester() : base(LayerMgr.Get("default"))
		{
			InitCameras();

			InitWindow();

			InitRasterizer();

			var layer = LayerMgr.Create("balls", -1);
			layer.IsGUI = false;

			GameMgr.MaxGameSpeed = 60;
			GameMgr.FixedUpdateRate = 1.0 / 30.0;
			
			//Layer.Get("default").IsGUI = true;

			new DrawingTester();
			new ECSTester(LayerMgr.Get("balls"));
			//new AlarmTester(Layer.Get("balls"));

		}


		public override void Update()
		{
			if (Input.CheckButton(Buttons.Escape))
			{
				GameMgr.ExitGame();
			}
			
			//Console.WriteLine(Layer.__GetLayerInfo());
			
			#region Camera. 

			
			if (Input.CheckButton(Buttons.Left))
			{
				MainCamera.Pos.X += (5 / MainCamera.Zoom);
			}
			
			if (Input.CheckButton(Buttons.Right))
			{
				MainCamera.Pos.X -= (5 / MainCamera.Zoom);
			}
			
			if (Input.CheckButton(Buttons.Up))
			{
				MainCamera.Pos.Y += (5 / MainCamera.Zoom);
			}
			
			if (Input.CheckButton(Buttons.Down))
			{
				MainCamera.Pos.Y -= (5 / MainCamera.Zoom);
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



		void InitCameras()
		{
			MainCamera.BackgroundColor = Color.DarkSeaGreen;

			MainCamera.Offset = MainCamera.Size / 2;

			MainCamera.Pos = MainCamera.Size / 2;

			SecondCamera.PortPos.X = 600;
			SecondCamera.BackgroundColor = Color.DarkSeaGreen;
			SecondCamera.Enabled = false;
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
			rasterizerState.CullMode = CullMode.None;
			rasterizerState.ScissorTestEnable = false;
			rasterizerState.FillMode = FillMode.Solid;
			
			
			DrawMgr.Rasterizer = rasterizerState;

			DrawMgr.Sampler = SamplerState.PointClamp;
		}

	}
}
