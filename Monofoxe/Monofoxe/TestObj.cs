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
		int x, y;
		double period = 3; // Seconds.
		double ang = 0;
		
		RenderTarget2D surf;

		
		Camera cam = new Camera(800, 480);

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
			surf = new RenderTarget2D(DrawCntrl.Device, 512, 512);

			SpriteBatch batch = new SpriteBatch(DrawCntrl.Device);

			DrawCntrl.Device.SetRenderTarget(surf);

			batch.Begin();

			batch.Draw(Game1.tex, new Vector2(32, 32), Color.White);

			batch.End();

			DrawCntrl.Device.SetRenderTarget(null);

		}

		public override void Update()
		{

			ang += GameCntrl.Time((Math.PI * 2) / period);

			if (ang >= Math.PI * 2)
			{
				ang -= Math.PI * 2;
			//	Debug.WriteLine("fps: " + GameCntrl.Fps + " " + GameCntrl.ElapsedTimeTotal);
			}
			
			if (Input.KeyboardCheck(Keys.Left))
			{x += 5;}
			
			if (Input.KeyboardCheck(Keys.Right))
			{x -= 5;}
			
			if (Input.KeyboardCheck(Keys.Up))
			{y += 5;}
			
			if (Input.KeyboardCheck(Keys.Down))
			{y -= 5;}
			
			if (Input.KeyboardCheck(Keys.Z))
			{
				cam.ScaleX += 0.1f;
				cam.ScaleY += 0.1f;
			}
			
			if (Input.KeyboardCheck(Keys.X))
			{
				cam.ScaleX -= 0.1f;
				cam.ScaleY -= 0.1f;
			}
			
			if (Input.KeyboardCheck(Keys.C))
			{cam.Rotation += 5;}

			if (Input.KeyboardCheck(Keys.V))
			{cam.Rotation -= 5;}


			//x=32;
			//y=32;
			cam.X = x;
			cam.Y = y;
			//cam.ScaleX = 2;
			//cam.ScaleY = 2;
			//cam.OffsetX = cam.W/2;
			//cam.OffsetY = cam.H/2;



		}

		public override void Draw()
		{	
			
			DrawCntrl.CurrentColor = Color.Red;
			/*
			Engine.Drawing.Rectangle.Draw(100, 100, 150, 200, false);
			

			Engine.Drawing.Rectangle.Draw(100, 100, 150, 200, false, Color.SkyBlue, Color.Sienna, Color.SpringGreen, Color.Tomato);
			int xx, yy;
			
			xx = 32;
			yy = 32;
			DrawCntrl.CurrentColor = Color.SkyBlue;
			Engine.Drawing.Rectangle.Draw(xx, yy, xx + 32, yy + 32, false);
			
			xx = 64;
			yy = 32;
			DrawCntrl.CurrentColor = Color.Sienna;
			Engine.Drawing.Rectangle.Draw(xx, yy, xx + 32, yy + 32, false);
			
			
			xx = 32;
			yy = 64;
			DrawCntrl.CurrentColor = Color.SpringGreen;
			Engine.Drawing.Rectangle.Draw(xx, yy, xx + 32, yy + 32, false);

			
			xx = 64;
			yy = 64;
			DrawCntrl.CurrentColor = Color.Tomato;
			Engine.Drawing.Rectangle.Draw(xx, yy, xx + 32, yy + 32, false);
			
			//DrawCntrl.DrawPrimitive(vertexBuffer);
			
			//DrawCntrl.DrawSprite(Game1.tex, -cam.X + x + 64, -cam.Y + y, new Color(255, 0, 0, 128));
			
			//DrawCntrl.DrawSprite(Game1.part, x + 64, y, new Color(0, 0, 255, 128));
			Circle.Draw(200, 200, 8, true);
			DrawCntrl.CurrentColor = new Color(56, 0, 10, 128);
			Circle.Draw(200, 200+16, 8, false);
			*/
			Debug.WriteLine(GameCntrl.Fps);
			DrawCntrl.CurrentColor = new Color(255, 0, 0, 100);
			var xx = 0;
			for(var i = 0; i < 2500*4 ; i += 1)
			{
				Triangle.Draw(32,32+xx,44,64,450,64,false);
				xx += 1;
				if (xx > 200)
				{xx = 0;}
				//Triangle.Draw(32+32,32+32,32+32,64+32,64+32,64+32,true);
				
				//Engine.Drawing.Rectangle.Draw(32, 32, 100, 100, false);
				//Engine.Drawing.Rectangle.Draw(32, 32, 100, 100, true);
			}
			

			Debug.WriteLine(GameCntrl.Fps);
			//Triangle.Draw(32,32+32,32,64+32,64,64+32,false);
			DrawCntrl.CurrentColor = Color.Blue;
			//Engine.Drawing.Rectangle.Draw(32, 32, 100, 100, true);
			//Engine.Drawing.Rectangle.Draw(32, 32, 100, 100, false);

			

		}

		public override void DrawGUI()
		{
			
		}

	}
}
