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

namespace Monofoxe
{
	class TestObj: GameObj 
	{
		float x, y;
		double period = 3; // Seconds.
		double ang = 0;
		
		RenderTarget2D surf;

		
		Camera cam = new Camera(400, 480);
		Camera cam1 = new Camera(400, 480);

		VertexPositionColor[] vertices = new VertexPositionColor[3];
		VertexBuffer vertexBuffer;

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
			

			cam.BackgroundColor = Color.BurlyWood;

			
			cam.OffsetX = cam.W / 2;
			cam.OffsetY = cam.H / 2;

			x = cam.W / 2;
			y = cam.H / 2;

			cam1.PortX = 400;
			cam1.BackgroundColor = Color.Sienna;
			//cam1.Enabled = false;
		}

		public override void Update()
		{

			ang += GameCntrl.Time((Math.PI * 2) / period);

			if (ang >= Math.PI * 2)
			{
				ang -= Math.PI * 2;
			}
			
			if (Input.KeyboardCheck(Keys.Left))
			{x += (5 / cam.ScaleX);}
			
			if (Input.KeyboardCheck(Keys.Right))
			{x -= (5 / cam.ScaleX);;}
			
			if (Input.KeyboardCheck(Keys.Up))
			{y += (5 / cam.ScaleX);}
			
			if (Input.KeyboardCheck(Keys.Down))
			{y -= (5 / cam.ScaleX);}
			
			if (Input.KeyboardCheck(Keys.Z))
			{
				cam.ScaleX += 0.1f;
				cam.ScaleY += 0.1f;
			}
			
			if (Input.KeyboardCheck(Keys.X))
			{
				cam.ScaleX -= 0.1f;
				cam.ScaleY -= 0.1f;
				if (cam.ScaleX <= 0)
				{
					cam.ScaleX = 0.1f;
					cam.ScaleY = 0.1f;
				}
			}
			
			if (Input.KeyboardCheck(Keys.C))
			{cam.Rotation += 5;}

			if (Input.KeyboardCheck(Keys.V))
			{cam.Rotation -= 5;}


			cam.X = x;
			cam.Y = y;

			DrawCntrl.SetSurfaceTarget(surf, Matrix.CreateTranslation(0, 0, 0));
			//DrawCntrl.Device.Clear(Color.Azure);
			DrawCntrl.DrawSprite(Game1.part, cam.X, cam.Y, Color.White);
			DrawCntrl.ResetSurfaceTarget();
		}

		public override void Draw()
		{	
			
			

	
			DrawCntrl.PrimitiveAddVertex(new Vector2(64, 64), Color.DarkOrange);
			DrawCntrl.PrimitiveAddVertex(new Vector2(70, 70), Color.Aquamarine);
			DrawCntrl.PrimitiveAddVertex(new Vector2(100, 80), Color.DarkBlue);
			DrawCntrl.PrimitiveAddVertex(new Vector2(64, 80), new Color(76, 135, 255, 128));
			DrawCntrl.PrimitiveSetLineStripIndices(true);
			DrawCntrl.PrimitiveEnd();
			
			//DrawCntrl.DrawTriangle(0,0,32,32,64,100,true);

			DrawCntrl.DrawSurface(surf, 32, 32, Color.White);
			
			DrawCntrl.DrawSprite(Game1.part, 0, 0, Color.Red);

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
			DrawCntrl.PrimitiveSetTexture(Game1.tex);
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
			DrawCntrl.PrimitiveSetTexture(Game1.tex);
			DrawCntrl.PrimitiveSetMeshIndices(w, h);
			DrawCntrl.PrimitiveEnd();
			
			//DrawCntrl.DrawRectangle(0, 0, 100, 100, false);
			Debug.WriteLine(GameCntrl.Fps);
			
			//DrawCntrl.DrawLine(Input.MousePos, Vector2.Zero, 16, Color.AliceBlue, Color.Black);

			//for(var i = 0; i < 5000; i +=1)
			//DrawCntrl.DrawCircle(Input.MousePos, 8, false);
			
			//DrawCntrl.CurrentColor = Color.Brown;
			//DrawCntrl.DrawRectangle(64,64,96,96,false);
			//DrawCntrl.CurrentColor = Color.Cornsilk;
			//DrawCntrl.DrawTriangle(0,0,0,16,3,320,false);
			//DrawCntrl.DrawLine(Input.MousePos, Vector2.Zero, Color.AliceBlue, Color.Black);
			//DrawCntrl.DrawLine(100,100,120,120, Color.AliceBlue, Color.Black);

			
		}

		public override void DrawGUI()
		{
		
			DrawCntrl.DrawSprite(Game1.tex, 0, 0, Color.White);
			
			DrawCntrl.CurrentColor = Color.Black;
			DrawCntrl.DrawRectangle(32,16,640,64,false);

			Color c = new Color(56, 135, 255, 0);
			DrawCntrl.PrimitiveAddVertex(0, 0, new Vector2(0, 0));
			DrawCntrl.PrimitiveAddVertex(32, 32, c, new Vector2(0, 1));
			DrawCntrl.PrimitiveAddVertex(64, 0, c, new Vector2(1, 0));
			DrawCntrl.PrimitiveAddVertex(96, 32, c, new Vector2(1, 1));
			DrawCntrl.PrimitiveSetTriangleStripIndices();
			//DrawCntrl.PrimitiveSetTexture(Game1.tex);
			DrawCntrl.PrimitiveEnd();
			


		}

	}
}
