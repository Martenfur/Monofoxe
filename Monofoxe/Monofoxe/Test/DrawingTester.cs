using System;
using Monofoxe.Engine;
using Monofoxe.Engine.Drawing;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Diagnostics;
using Resources.Sprites;
using Resources;
using Monofoxe.Engine.ECS;
using Monofoxe.ECSTest.Systems;
using Monofoxe.ECSTest.Components;

namespace Monofoxe.Test
{
	public class DrawingTester : Entity
	{
		Sprite testSpr = SpritesDefault.Chiggin;
		float fireFrame = 0;
	

		public DrawingTester() : base(LayerMgr.Get("default"))
		{
			
		}

		public override void Update()
		{
			fireFrame += 0.1f;

			if (fireFrame >= SpritesDefault.DemonFire.Frames.Length)
			{
				fireFrame = 0;
			}

			GameMgr.WindowManager.WindowTitle = "Draw fps: " + GameMgr.Fps;
		}

		public override void Draw()
		{	
			/*
			if (DrawMgr.CurrentCamera == MainTester.MainCamera)
			{
				Effects.Effect.Parameters["test"].SetValue(new Vector4(0.0f, 0.7f, 0.0f, 1.0f));
				DrawMgr.Effect = Effects.Effect;
			}
			*/

			DrawMgr.CurrentColor = Color.Violet;
			
			DrawMgr.DrawSprite(SpritesDefault.DemonFire, (int)fireFrame, new Vector2(0, 0), new Vector2(1, 1), 0, Color.White);

			
			Frame f = SpritesDefault.DemonFire.Frames[(int)fireFrame];
			DrawMgr.CurrentColor = Color.White;
			DrawMgr.DrawRectangle(0, 0, SpritesDefault.DemonFire.W, SpritesDefault.DemonFire.H, true);
		
			DrawMgr.CurrentColor = Color.White;
			
			

			var p = new Vector2(50, 200);
			for(var i = 0; i < 8; i += 1)
			{
				DrawMgr.DrawSprite(testSpr, 0, p + Vector2.UnitX * i * 16, Vector2.One, i * 5, Color.White * 0.5f);
			}
			

			DrawMgr.Effect = null;
			DrawMgr.CurrentFont = Fonts.Arial;
			
			DrawMgr.CurrentColor = Color.White * 0.5f;//new Color(0.5f, 0.5f, 0.5f, 0.5f);
			DrawMgr.DrawCircle(200, 200, 32, false);
			DrawMgr.DrawCircle(200, 216, 32, false);
			DrawMgr.CurrentColor = Color.White;

			DrawMgr.DrawText("test", 100, 100);
		}



		private void TestDrawPrimitives()
		{
			DrawMgr.PrimitiveAddVertex(new Vector2(64, 64), Color.DarkOrange);
			DrawMgr.PrimitiveAddVertex(new Vector2(70, 70), Color.Aquamarine);
			DrawMgr.PrimitiveAddVertex(new Vector2(100, 80), Color.DarkBlue);
			DrawMgr.PrimitiveAddVertex(new Vector2(64, 80), new Color(76, 135, 255, 128));
			DrawMgr.PrimitiveSetLineStripIndices(true);
			DrawMgr.PrimitiveEnd();
			
			//DrawCntrl.DrawTriangle(0,0,32,32,64,100,true);

			DrawMgr.PrimitiveAddVertex(new Vector2(120, 54), Color.DarkOrange);
			DrawMgr.PrimitiveAddVertex(new Vector2(150, 60), Color.Aquamarine);
			DrawMgr.PrimitiveAddVertex(new Vector2(180, 60), Color.DarkBlue);
			DrawMgr.PrimitiveAddVertex(new Vector2(130, 80), Color.Chartreuse);
			DrawMgr.PrimitiveSetTriangleFanIndices();
			DrawMgr.PrimitiveEnd();
			
			
			DrawMgr.PrimitiveAddVertex(0, 0, new Vector2(0, 0));
			DrawMgr.PrimitiveAddVertex(32, 32, new Color(56, 135, 255, 0), new Vector2(0, 1));
			DrawMgr.PrimitiveAddVertex(64, 0,new Color(56, 135, 255, 0) , new Vector2(1, 0));
			DrawMgr.PrimitiveAddVertex(96, 32, new Color(56, 135, 255, 0), new Vector2(1, 1));
			DrawMgr.PrimitiveSetTriangleStripIndices();
			DrawMgr.PrimitiveSetTexture(SpritesDefault.BirdieBody, 0);
			DrawMgr.PrimitiveEnd();
			
			

			int _x = 0;
			int _y = 100;

			int w = 7;
			int h = 7;
			
			for(var k = 0; k < h; k += 1)
			{
				for(var i = 0; i < w; i += 1)
				{			
					DrawMgr.PrimitiveAddVertex(_x + 8 * i + i * i * k, _y + 8 * k + k * k * i, Color.White, new Vector2(i / (float)(w - 1), k / (float)(h - 1)));	
				}
			}
			DrawMgr.PrimitiveSetTexture(SpritesDefault.Boulder3, 0);
			DrawMgr.PrimitiveSetMeshIndices(w, h);
			DrawMgr.PrimitiveEnd();
		}


	}
}
