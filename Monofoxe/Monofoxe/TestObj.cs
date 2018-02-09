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
			cam.BackgroundColor = Color.BlanchedAlmond;
		}

		public override void Update()
		{

			ang += GameCntrl.Time((Math.PI * 2) / period);

			if (ang >= Math.PI * 2)
			{
				ang -= Math.PI * 2;
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


			cam.X = x;
			cam.Y = y;
			


		}
		int c = 0;
		public override void Draw()
		{	
			
			Debug.WriteLine(GameCntrl.Fps);
			DrawCntrl.CurrentColor = new Color(255, 0, 0, 100);

			DrawCntrl.CurrentColor = Color.Blue;
			DrawCntrl.DrawRectangle(110, 110, 32, 32, false);
			DrawCntrl.CurrentColor = Color.Brown;
			DrawCntrl.DrawRectangle(0, 0, 32, 32, true);
			
			DrawCntrl.DrawCircle(400, 64, 32, false);
			DrawCntrl.DrawRectangle(0, 0, 32, 32, false);
			DrawCntrl.DrawCircle(64, 64, 32, true);
			DrawCntrl.DrawRectangle(0, 0, 32, 32, false);
			
			DrawCntrl.DrawCircle(200, 64, 32, false);
			int _x = 0;
			int _y = 300;

			int w = 8;
			int h = 8;

			c += 1;
			if (c>320)
			{c=0;}

			DrawCntrl.PrimitiveBegin(DrawCntrl.PrType.Triangles);
			for(var k = 0; k < h; k += 1)
			{
				for(var i = 0; i < w; i += 1)
				{			
					DrawCntrl.PrimitiveAddVertex(new Vector2(_x + 32 * i + (int)Math.Sqrt(i)*k*c, _y + 32 * k + (int)Math.Sqrt(k)*i*c), Color.BlueViolet);	
				}
			}

			DrawCntrl.PrimitiveSetMeshIndices(w, h);
			DrawCntrl.PrimitiveEnd();

			Debug.WriteLine(GameCntrl.Fps);
		}

		public override void DrawGUI()
		{
			
		}

	}
}
