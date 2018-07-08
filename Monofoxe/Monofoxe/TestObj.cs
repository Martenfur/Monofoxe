using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Monofoxe.Engine;
using Monofoxe.Engine.Drawing;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Diagnostics;
using Monofoxe.Utils;

namespace Monofoxe
{
	class TestObj: GameObj 
	{
		float x, y;
		double period = 3; // Seconds.
		double ang = 0;
		
		RenderTarget2D surf;

		
		Camera cam = new Camera(600, 600);
		Camera cam1 = new Camera(600, 600);

		VertexPositionColor[] vertices = new VertexPositionColor[3];
		VertexBuffer vertexBuffer;

		float fireFrame = 0;

		RandomExt r = new RandomExt();

		AutoAlarm auto1 = new AutoAlarm(1);
		AutoAlarm auto2 = new AutoAlarm(1);

		Sprite s1 = Sprites.Default.Boss;
		Sprite s2 = Sprites.Default.Boss;

		Timer timer = new Timer();

		RenderTarget2D surfForDrawing;

		public TestObj()
		{
			GameCntrl.GameSpeedMultiplier = 1;
			auto1.AffectedBySpeedMultiplier = false;


			vertices[0] = new VertexPositionColor(new Vector3(0, 0, 0), Color.Transparent);
			vertices[1] = new VertexPositionColor(new Vector3(64, 64, 0), Color.Transparent);
			vertices[2] = new VertexPositionColor(new Vector3(0, 64, 0), Color.White);
			vertexBuffer = new VertexBuffer(DrawCntrl.Device, typeof(VertexPositionColor), 3, BufferUsage.WriteOnly);
			vertexBuffer.SetData(vertices);
			
			
			GameCntrl.MaxGameSpeed = 60;
			surf = new RenderTarget2D(
				DrawCntrl.Device, 
				512, 
				512, 
				false,
				DrawCntrl.Device.PresentationParameters.BackBufferFormat,
				DepthFormat.Depth24, 
				0, 
				RenderTargetUsage.PreserveContents
			);
			surfForDrawing = new RenderTarget2D(
				DrawCntrl.Device, 
				128, 
				128, 
				false,
				DrawCntrl.Device.PresentationParameters.BackBufferFormat,
				DepthFormat.Depth24, 
				0, 
				RenderTargetUsage.PreserveContents
			);
			

			
			DrawCntrl.SetSurfaceTarget(surf);
			Game1.effect.Parameters["test"].SetValue(new Vector4(0.5f, 0.3f, 0.1f, 1));
			DrawCntrl.Effect = Game1.effect;
			DrawCntrl.Device.Clear(Color.Black);
			DrawCntrl.DrawCircle(256, 256, 256, false);
			DrawCntrl.DrawSprite(Sprites.Default.Chiggin, 0, 0);

			DrawCntrl.Effect = null;
			DrawCntrl.ResetSurfaceTarget();




			cam.BackgroundColor = Color.DarkSeaGreen;

			cam.Offset = cam.Size / 2;

			x = cam.Size.X / 2;
			y = cam.Size.Y / 2;

			cam1.PortPos.X = 600;
			cam1.BackgroundColor = Color.DarkSeaGreen;
			cam1.Enabled = true;

			RasterizerState rasterizerState = new RasterizerState(); // Do something with it, I guees.
			rasterizerState.CullMode = CullMode.None;
			rasterizerState.ScissorTestEnable = false;//(_scissorRectangle != Rectangle.Empty);
			rasterizerState.FillMode = FillMode.Solid;
			DrawCntrl.Rasterizer = rasterizerState;

			DrawCntrl.Sampler = SamplerState.PointClamp;

			//DrawCntrl.ScissorRectangle = new Rectangle(0, 0, 100, 100);
			GameCntrl.WindowManager.CanvasSize = new Vector2(1200, 600);
			GameCntrl.WindowManager.Window.AllowUserResizing = false;
			GameCntrl.WindowManager.ApplyChanges();
			GameCntrl.WindowManager.CenterWindow();
			
			GameCntrl.WindowManager.CanvasMode = CanvasMode.Fill; 
		}
		
		public override void Update()
		{
			GameCntrl.WindowManager.WindowTitle = "Draw fps: " + GameCntrl.Fps;
			
			fireFrame += 0.1f;

			if (fireFrame >= Sprites.Default.DemonFire.Frames.Count())
			{
				fireFrame = 0;
			}

			#region Camera. 

			ang += GameCntrl.Time((Math.PI * 2) / period);

			if (ang >= Math.PI * 2)
			{
				ang -= Math.PI * 2;
			}
			
			if (Input.KeyboardCheck(Keys.Left))
			{x += (5 / cam.Zoom);}
			
			if (Input.KeyboardCheck(Keys.Right))
			{x -= (5 / cam.Zoom);}
			
			if (Input.KeyboardCheck(Keys.Up))
			{y += (5 / cam.Zoom);}
			
			if (Input.KeyboardCheck(Keys.Down))
			{y -= (5 / cam.Zoom);}
			
			if (Input.KeyboardCheck(Keys.Z))
			{
				cam.Zoom += 0.1f;
			}
			
			if (Input.KeyboardCheck(Keys.X))
			{
				cam.Zoom -= 0.1f;
				if (cam.Zoom <= 0)
				{
					cam.Zoom = 0.1f;
				}
			}
			
			if (Input.KeyboardCheck(Keys.C))
			{cam.Rotation += 5;}

			if (Input.KeyboardCheck(Keys.V))
			{cam.Rotation -= 5;}

			if (Input.KeyboardCheck(Keys.Escape))
			{
				GameCntrl.ExitGame();
			}
			
			if (Input.KeyboardCheckPress(Keys.F))
			{
				GameCntrl.WindowManager.SetFullScreen(!GameCntrl.WindowManager.IsFullScreen);	
			}
			cam.Pos.X = x;
			cam.Pos.Y = y;

			#endregion Camera. 
			
			DrawCntrl.CurrentColor = new Color((float)r.NextDouble(), (float)r.NextDouble(), (float)r.NextDouble(), 1.0f);
			DrawCntrl.SetSurfaceTarget(surfForDrawing);
			if (Input.MouseCheck(MouseButtons.Left))
			{
				DrawCntrl.DrawCircle(Input.ScreenMousePos, 16, false);
			}
			DrawCntrl.ResetSurfaceTarget();
			
		}

		
		public override void Draw()
		{	
			if (DrawCntrl.CurrentCamera == cam)
			{
				//DrawCntrl.BlendState = BlendState.Additive;
				Game1.effect.Parameters["test"].SetValue(new Vector4(0.0f, 0.7f, 0.0f, 1.0f));
				DrawCntrl.Effect = Game1.effect;
			}
			else
			{
				//DrawCntrl.BlendState = BlendState.AlphaBlend;
			}


			DrawCntrl.CurrentColor = Color.Violet;
			//DrawCntrl.DrawRectangle(-32, -32, 500, 500, false);
			DrawCntrl.DrawSprite(Sprites.Default.BstGam, 0, Vector2.Zero);
			
			DrawCntrl.DrawSprite(Sprites.Default.DemonFire, (int)fireFrame, new Vector2(0, 0), new Vector2(1, 1), 0, Color.White);

			
			Frame f = Sprites.Default.DemonFire.Frames[(int)fireFrame];
			DrawCntrl.CurrentColor = Color.White;
			DrawCntrl.DrawRectangle(0, 0, Sprites.Default.DemonFire.W, Sprites.Default.DemonFire.H, true);
			
			DrawCntrl.CurrentColor = Color.BlueViolet;
			DrawCntrl.DrawRectangle(f.Origin.X, f.Origin.Y, f.TexturePosition.Width + f.Origin.X, f.TexturePosition.Height + f.Origin.Y, true);

			DrawCntrl.DrawCircle(Input.MousePos, 4, true);
			
			Vector2 p1 = new Vector2(300, 400);
			Vector2 p2 = new Vector2(300 + 100, 400 + 32);
			Vector2 p3 = new Vector2(300 + 200, 400 - 50);
			Vector2 s = new Vector2(30, 40);
			

			if (GameMath.LinesCross(Input.MousePos, p2, p1, p3, ref s) == 1)//GameMath.RectangleInRectangle(Input.MousePos, Input.MousePos + s, p1, p2))
			{
				DrawCntrl.CurrentColor = Color.Red;
			}
			else
			{
				DrawCntrl.CurrentColor = Color.Black;
			}

			//DrawCntrl.DrawRectangle(p1, p2, true);
			//DrawCntrl.DrawRectangle(Input.MousePos, Input.MousePos + s, true);
			DrawCntrl.DrawLine(p1, p3);
			DrawCntrl.DrawLine(Input.MousePos, p2);
		
			DrawCntrl.CurrentColor = Color.White;
			
			DrawCntrl.DrawCircle(s, 8, true);
			
			DrawCntrl.DrawSprite(s1, new Vector2(200, 200));
			DrawCntrl.DrawSprite(s2, new Vector2(400, 200));
			
			DrawCntrl.DrawSurface(surf, 128, 128);

			DrawCntrl.Effect = null;
			
			
		}

		public override void DrawGUI()
		{
			DrawCntrl.CurrentColor = Color.Red;

			
			DrawCntrl.CurrentColor = Color.White;
			DrawCntrl.DrawSurface(surfForDrawing, 0, 0);
			DrawCntrl.DrawCircle(Input.ScreenMousePos, 8, false);
		}

		private void TestDrawPrimitives()
		{
			DrawCntrl.PrimitiveAddVertex(new Vector2(64, 64), Color.DarkOrange);
			DrawCntrl.PrimitiveAddVertex(new Vector2(70, 70), Color.Aquamarine);
			DrawCntrl.PrimitiveAddVertex(new Vector2(100, 80), Color.DarkBlue);
			DrawCntrl.PrimitiveAddVertex(new Vector2(64, 80), new Color(76, 135, 255, 128));
			DrawCntrl.PrimitiveSetLineStripIndices(true);
			DrawCntrl.PrimitiveEnd();
			
			//DrawCntrl.DrawTriangle(0,0,32,32,64,100,true);

			DrawCntrl.PrimitiveAddVertex(new Vector2(120, 54), Color.DarkOrange);
			DrawCntrl.PrimitiveAddVertex(new Vector2(150, 60), Color.Aquamarine);
			DrawCntrl.PrimitiveAddVertex(new Vector2(180, 60), Color.DarkBlue);
			DrawCntrl.PrimitiveAddVertex(new Vector2(130, 80), Color.Chartreuse);
			DrawCntrl.PrimitiveSetTriangleFanIndices();
			DrawCntrl.PrimitiveEnd();
			
			
			DrawCntrl.PrimitiveAddVertex(0, 0, new Vector2(0, 0));
			DrawCntrl.PrimitiveAddVertex(32, 32, new Color(56, 135, 255, 0), new Vector2(0, 1));
			DrawCntrl.PrimitiveAddVertex(64, 0,new Color(56, 135, 255, 0) , new Vector2(1, 0));
			DrawCntrl.PrimitiveAddVertex(96, 32, new Color(56, 135, 255, 0), new Vector2(1, 1));
			DrawCntrl.PrimitiveSetTriangleStripIndices();
			DrawCntrl.PrimitiveSetTexture(Sprites.Default.BirdieBody, 0);
			DrawCntrl.PrimitiveEnd();
			
			

			int _x = 0;
			int _y = 100;

			int w = 7;
			int h = 7;
			
			for(var k = 0; k < h; k += 1)
			{
				for(var i = 0; i < w; i += 1)
				{			
					DrawCntrl.PrimitiveAddVertex(_x + 8 * i + i * i * k, _y + 8 * k + k * k * i, Color.White, new Vector2(i / (float)(w - 1), k / (float)(h - 1)));	
				}
			}
			DrawCntrl.PrimitiveSetTexture(Sprites.Default.Boulder3, 0);
			DrawCntrl.PrimitiveSetMeshIndices(w, h);
			DrawCntrl.PrimitiveEnd();
		}

	}
}
