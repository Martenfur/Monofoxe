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

		float fireFrame = 0;

		Random r = new Random();
		Sprite fontSheet;
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

			
			cam.OffsetX = cam.W / 2;
			cam.OffsetY = cam.H / 2;

			x = cam.W / 2;
			y = cam.H / 2;

			cam1.PortX = 400;
			cam1.BackgroundColor = Color.AliceBlue;//Color.Sienna;
			//cam1.Enabled = false;

			RasterizerState rasterizerState = new RasterizerState(); // Do something with it, I guees.
			rasterizerState.CullMode = CullMode.None;
			rasterizerState.ScissorTestEnable = false;//(_scissorRectangle != Rectangle.Empty);
			rasterizerState.FillMode = FillMode.Solid;
			DrawCntrl.Rasterizer = rasterizerState;

			//DrawCntrl.ScissorRectangle = new Rectangle(0, 0, 100, 100);


			fontSheet = new Sprite(new Frame(Game1.Def.Texture, new Rectangle(0, 0, Game1.Def.Texture.Width, Game1.Def.Texture.Height), Vector2.Zero, Game1.Def.Texture.Width, Game1.Def.Texture.Height), 0, 0);


		}

		public override void Update()
		{
			fireFrame += 0.1f;

			if (fireFrame >= Sprites.DemonFire.Frames.Count())
			{
				fireFrame = 0;
			}

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

			

			if (Input.KeyboardCheck(Keys.T))
			{
				fuckup += 4;
			}
			if (Input.KeyboardCheck(Keys.Y))
			{
				fuckup = 0;
			}

			cam.X = x;
			cam.Y = y;

			Debug.WriteLine("Draw fps: " + GameCntrl.Fps + " Step fps: " + GameCntrl.Tps);
			
		}

		float fuckupSpd = 0;
		float fuckup = 0;
		float mtxAng = 0;

		public override void Draw()
		{	
			mtxAng += 1;
			if (mtxAng > 359)
			{mtxAng -= 360;}

			//DrawCntrl.DrawSprite(Sprites.DemonFire, (int)fireFrame, new Vector2(0, 0), new Vector2(1, 1), 0, Color.White);


			Frame f = Sprites.DemonFire.Frames[(int)fireFrame];
			DrawCntrl.CurrentColor = Color.Red;
			//DrawCntrl.DrawRectangle(0, 0, Sprites.DemonFire.W, Sprites.DemonFire.H, true);
			
			DrawCntrl.CurrentColor = Color.BlueViolet;
			//DrawCntrl.DrawRectangle(f.Origin.X, f.Origin.Y, f.TexturePosition.Width + f.Origin.X, f.TexturePosition.Height + f.Origin.Y, true);

			
			DrawCntrl.PrimitiveBegin();
			DrawCntrl.PrimitiveSetTexture(Sprites.BstGam, 1);

			int _x = 100;
			int _y = 100;

			int w = 32;
			int h = 32;
			
			

			for(var k = 0; k < h; k += 1)
			{
				for(var i = 0; i < w; i += 1)
				{			
					DrawCntrl.PrimitiveAddVertex(
					_x + 8 * i + (float)(r.NextDouble() * 2.0 - 1.0) * fuckup,
					_y + 8 * k + (float)(r.NextDouble() * 2.0 - 1.0) * fuckup,
					Color.White, 
					new Vector2(i / (float)(w - 1)*2, k / (float)(h - 1)*2));	
				}
			}

			DrawCntrl.PrimitiveSetMeshIndices(w, h);
			DrawCntrl.PrimitiveEnd();
			
			Vector2 size = Game1.Def.MeasureString("AVFoxes" + Environment.NewLine + "лисята");
			Vector2 pos = new Vector2(0, 0);
			Vector2 pos1 = new Vector2(32, 100);


			Matrix 
			TransformMatrix = Matrix.CreateTranslation(new Vector3(-8, -8, 0)) *          // Coordinates.
		                    Matrix.CreateRotationZ(MathHelper.ToRadians(-mtxAng)) *   // Rotation.
		                    Matrix.CreateScale(new Vector3(1, 1, 1)) *	      // Scale.
		                    Matrix.CreateTranslation(new Vector3(32, 100, 0)); // Offset.		
			
			DrawCntrl.DrawRectangle(pos1, pos1 + size, true);
			
			//DrawCntrl.AddTransformMatrix(TransformMatrix);
			DrawCntrl.DrawRectangle(pos, pos + size, true);
			DrawCntrl.CurrentColor = Color.White;
			DrawCntrl.DrawText(Fonts.TexFont, pos.X, pos.Y, "AVFoxIes" + Environment.NewLine + "moar foxi'es", TextAlign.Center, TextAlign.Center);
			//DrawCntrl.ResetTransformMatrix();
			Debug.WriteLine("STR: " + Game1.Def.MeasureString("A") + " " + Game1.Def.MeasureString("_"));
			
		}

		public override void DrawGUI()
		{
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
			DrawCntrl.PrimitiveSetTexture(Sprites.BirdieBody, 0);
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
			DrawCntrl.PrimitiveSetTexture(Sprites.Boulder3, 0);
			DrawCntrl.PrimitiveSetMeshIndices(w, h);
			DrawCntrl.PrimitiveEnd();
		}

	}
}
