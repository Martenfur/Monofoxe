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
		public static Camera MainCamera = new Camera(600, 600);
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


		public MainTester() : base(Layer.Get("default"))
		{
			InitCameras();

			InitWindow();

			InitRasterizer();

			var layer = Layer.Create("balls", 0);
			layer.IsGUI = true;

			GameMgr.MaxGameSpeed = 60;
			GameMgr.FixedUpdateRate = 1.0 / 30.0;
			


			new DrawingTester();
			new ECSTester(Layer.Get("balls"));
		}


		public override void Update()
		{
			if (Input.CheckButton(Buttons.Escape))
			{
				GameMgr.ExitGame();
			}
			
			
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
			SecondCamera.Enabled = true;
		}


		void InitWindow()
		{
			GameMgr.WindowManager.CanvasSize = new Vector2(1200, 600);
			GameMgr.WindowManager.Window.AllowUserResizing = false;
			GameMgr.WindowManager.ApplyChanges();
			GameMgr.WindowManager.CenterWindow();
			
			GameMgr.WindowManager.CanvasMode = CanvasMode.None; 
		}


		void InitRasterizer()
		{
			DrawMgr.BlendState = BlendState.NonPremultiplied; // Makes alpha magically work.
			
			RasterizerState rasterizerState = new RasterizerState(); // Do something with it, I guees.
			rasterizerState.CullMode = CullMode.None;
			rasterizerState.ScissorTestEnable = false;//(_scissorRectangle != Rectangle.Empty);
			rasterizerState.FillMode = FillMode.Solid;
			
			DrawMgr.Rasterizer = rasterizerState;

			DrawMgr.Sampler = SamplerState.PointClamp;

		}

	}
}
