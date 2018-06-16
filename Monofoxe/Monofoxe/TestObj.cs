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

		
		Camera cam = new Camera(800, 600);
		Camera cam1 = new Camera(600, 480);

		VertexPositionColor[] vertices = new VertexPositionColor[3];
		VertexBuffer vertexBuffer;

		float fireFrame = 0;

		Random r = new Random();

		public TestObj()
		{
			
			vertices[0] = new VertexPositionColor(new Vector3(0, 0, 0), Color.Transparent);
			vertices[1] = new VertexPositionColor(new Vector3(64, 64, 0), Color.Transparent);
			vertices[2] = new VertexPositionColor(new Vector3(0, 64, 0), Color.White);
			vertexBuffer = new VertexBuffer(DrawCntrl.Device, typeof(VertexPositionColor), 3, BufferUsage.WriteOnly);
			vertexBuffer.SetData(vertices);
			
			
			GameCntrl.MaxGameSpeed = 60;
			surf = new RenderTarget2D(DrawCntrl.Device, 512, 512, false,
                                           DrawCntrl.Device.PresentationParameters.BackBufferFormat,
                                           DepthFormat.Depth24, 0, RenderTargetUsage.PreserveContents);
			

			cam.BackgroundColor = Color.AliceBlue;

			
			cam.Offset = cam.Size / 2;

			x = cam.Size.X / 2;
			y = cam.Size.Y / 2;

			cam1.PortPos.X = 400;
			cam1.BackgroundColor = Color.Sienna;
			cam1.Enabled = false;

			RasterizerState rasterizerState = new RasterizerState(); // Do something with it, I guees.
			rasterizerState.CullMode = CullMode.None;
			rasterizerState.ScissorTestEnable = false;//(_scissorRectangle != Rectangle.Empty);
			rasterizerState.FillMode = FillMode.Solid;
			DrawCntrl.Rasterizer = rasterizerState;

			DrawCntrl.Sampler = SamplerState.PointClamp;

			//DrawCntrl.ScissorRectangle = new Rectangle(0, 0, 100, 100);
			GameCntrl.WindowManager.CanvasSize = new Vector2(800, 600);
			GameCntrl.WindowManager.Window.AllowUserResizing = false;
			GameCntrl.WindowManager.ApplyChanges();
			GameCntrl.WindowManager.CenterWindow();

			GameCntrl.WindowManager.CanvasMode = CanvasMode.Fill; 

		}
		public override void UpdateBegin()
		{
			if (Input.KeyboardCheck(Keys.Left))
			{}
			Input.MouseCheck(MouseButtons.Left);
		}

		public override void Update()
		{
			GameCntrl.WindowManager.WindowTitle = "Draw fps: " + GameCntrl.Fps + " Step fps: " + GameCntrl.Tps + " ";
			
			fireFrame += 0.1f;

			if (fireFrame >= Sprites.Default.DemonFire.Frames.Count())
			{
				fireFrame = 0;
			}

			ang += GameCntrl.Time((Math.PI * 2) / period);

			if (ang >= Math.PI * 2)
			{
				ang -= Math.PI * 2;
			}
			
			if (Input.KeyboardCheck(Keys.Left))
			{x += (5 / cam.Scale.X);}
			
			if (Input.KeyboardCheck(Keys.Right))
			{x -= (5 / cam.Scale.X);}
			
			if (Input.KeyboardCheck(Keys.Up))
			{y += (5 / cam.Scale.X);}
			
			if (Input.KeyboardCheck(Keys.Down))
			{y -= (5 / cam.Scale.X);}
			
			if (Input.KeyboardCheck(Keys.Z))
			{
				cam.Scale.X += 0.1f;
				cam.Scale.Y += 0.1f;
			}
			
			if (Input.KeyboardCheck(Keys.X))
			{
				cam.Scale.X -= 0.1f;
				cam.Scale.Y -= 0.1f;
				if (cam.Scale.X <= 0)
				{
					cam.Scale.X = 0.1f;
					cam.Scale.Y = 0.1f;
				}
			}
			
			if (Input.KeyboardCheck(Keys.C))
			{cam.Rotation += 5;}

			if (Input.KeyboardCheck(Keys.V))
			{cam.Rotation -= 5;}


			if (Input.KeyboardCheck(Keys.G))
			{
				fuckup += fuckupSpd;
				fuckupSpd += 0.01f;
			}

			if (Input.KeyboardCheck(Keys.H))
			{
				fuckup -= 0.1f;
				if (fuckup < 0)
				{
					fuckup = 0;
				}
			}

			if (Input.KeyboardCheck(Keys.Escape))
			{
				GameCntrl.ExitGame();
			}

			if (Input.KeyboardCheck(Keys.T))
			{
				fuckup += 4;
			}
			if (Input.KeyboardCheckPress(Keys.F))
			{
				GameCntrl.WindowManager.SetFullScreen(!GameCntrl.WindowManager.IsFullScreen);	
			}
			cam.Pos.X = x;
			cam.Pos.Y = y;

			
		}

		float fuckupSpd = 0;
		float fuckup = 0;
		float mtxAng = 0;
		
		public override void Draw()
		{	

			mtxAng += 1;
			if (mtxAng > 359)
			{mtxAng -= 360;}
			DrawCntrl.CurrentColor = Color.Violet;
			//DrawCntrl.DrawRectangle(-32, -32, 500, 500, false);
			DrawCntrl.DrawSprite(Sprites.Default.DemonFire, (int)fireFrame, new Vector2(0, 0), new Vector2(1, 1), 0, Color.White);

			DrawCntrl.DrawSprite(Sprites.Default.BstGam, 0, Vector2.Zero);
			Debug.WriteLine(Sprites.Default.BstGam.W);

			Frame f = Sprites.Default.DemonFire.Frames[(int)fireFrame];
			DrawCntrl.CurrentColor = Color.Red;
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

			DrawCntrl.DrawCircle(s, 8, true);
			
		}

		public override void DrawGUI()
		{
			//DrawCntrl.DrawSprite(Sprites.Bench, 32, 32);
			//DrawCntrl.DrawSurface(DrawCntrl.Cameras[0].ViewSurface, 0, 0, Color.White);
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
