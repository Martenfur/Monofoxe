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
using Monofoxe.Engine.SceneSystem;
using Monofoxe.Utils;


namespace Monofoxe.Test
{
	public class DrawingTester : Entity
	{
		Sprite testSpr = SpritesDefault.Chiggin;
		double fireFrame = 0;
	

		public DrawingTester() : base(SceneMgr.GetScene("default")["default"])
		{
			
		}

		public override void Update()
		{
			fireFrame += TimeKeeper.GlobalTime(0.5);

			if (fireFrame > 1)
			{
				fireFrame -= 1;
			}

			var frame = Math.Max(0, Math.Min(SpritesDefault.DemonFire.Frames.Length - 1, (int)(fireFrame * SpritesDefault.DemonFire.Frames.Length)));

			Console.WriteLine(
				fireFrame + " " + 
				frame + " " + 
				SpritesDefault.DemonFire.Frames.Length
			);

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
			DrawMgr.CurrentCamera.PortScale = 2;
			DrawMgr.CurrentCamera.PortRotation = 0;


			DrawMgr.CurrentColor = Color.Violet;
			
			DrawMgr.DrawSprite(SpritesDefault.DemonFire, fireFrame, new Vector2(0, 0), new Vector2(1, 1), 0, Color.White);

			
			//Frame f = SpritesDefault.DemonFire.Frames[(int)fireFrame];
			//DrawMgr.CurrentColor = Color.White;
			//DrawMgr.DrawRectangle(0, 0, SpritesDefault.DemonFire.W, SpritesDefault.DemonFire.H, true);
		
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
			
			TestDrawPrimitives();
		}



		private void TestDrawPrimitives()
		{/*
			DrawMgr.PrimitiveBegin();
			DrawMgr.PrimitiveAddVertex(new Vector2(64, 64), Color.DarkOrange);
			DrawMgr.PrimitiveAddVertex(new Vector2(70, 70), Color.Aquamarine);
			DrawMgr.PrimitiveAddVertex(new Vector2(100, 80), Color.DarkBlue);
			DrawMgr.PrimitiveAddVertex(new Vector2(64, 80), new Color(76, 135, 255, 128));
			DrawMgr.PrimitiveSetLineStripIndices(true);
			DrawMgr.PrimitiveEnd();
			
			//DrawCntrl.DrawTriangle(0,0,32,32,64,100,true);
			DrawMgr.PrimitiveBegin();
			DrawMgr.PrimitiveAddVertex(new Vector2(120, 54), Color.DarkOrange);
			DrawMgr.PrimitiveAddVertex(new Vector2(150, 60), Color.Aquamarine);
			DrawMgr.PrimitiveAddVertex(new Vector2(180, 60), Color.DarkBlue);
			DrawMgr.PrimitiveAddVertex(new Vector2(130, 80), Color.Chartreuse);
			DrawMgr.PrimitiveSetTriangleFanIndices();
			DrawMgr.PrimitiveEnd();
			
			DrawMgr.PrimitiveBegin();
			DrawMgr.PrimitiveSetTexture(SpritesDefault.BirdieBody, 0);
			DrawMgr.PrimitiveAddVertex(0, 0, new Vector2(0, 0));
			DrawMgr.PrimitiveAddVertex(32, 32, new Color(56, 135, 255, 0), new Vector2(0, 1));
			DrawMgr.PrimitiveAddVertex(64, 0,new Color(56, 135, 255, 0) , new Vector2(1, 0));
			DrawMgr.PrimitiveAddVertex(96, 32, new Color(56, 135, 255, 0), new Vector2(1, 1));
			DrawMgr.PrimitiveSetTriangleStripIndices();
			DrawMgr.PrimitiveEnd();
			
			*/
			

			DrawMgr.PrimitiveBegin();
			DrawMgr.PrimitiveSetTexture(SpritesDefault.Boulder3, 0);
			
			int _x = 0;
			int _y = 100;

			int w = 7;
			int h = 7;
			
			for(var k = 0; k < h; k += 1)
			{
				for(var i = 0; i < w; i += 1)
				{			
					DrawMgr.PrimitiveAddVertex(new Vector2(_x + 8 * i, _y + 8 * k), Color.White, new Vector2(i / (float)(w - 1), k / (float)(h - 1)));	
				}
			}
			DrawMgr.PrimitiveSetMeshIndices(w, h);
			DrawMgr.PrimitiveEnd();
		}


	}
}
