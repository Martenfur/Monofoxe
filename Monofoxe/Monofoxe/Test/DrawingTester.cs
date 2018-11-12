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
using Monofoxe.Engine.Utils;


namespace Monofoxe.Test
{
	public class DrawingTester : Entity
	{
		Sprite testSpr = SpritesDefault.Chiggin;
		double fireFrame = 0;
	
		double chigginWave = 0;

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

			GameMgr.WindowManager.WindowTitle = "Draw fps: " + GameMgr.Fps;

			chigginWave += TimeKeeper.GlobalTime();
			if (chigginWave > 1)
			{
				chigginWave -= 1;
			}
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
			//Effects.Effect.Parameters["test"].SetValue(new Vector4(0.0f, 0.7f, 0.0f, 1.0f));
			//DrawMgr.Effect = Effects.Effect;

			DrawMgr.DrawSprite(SpritesDefault.DemonFire, fireFrame, new Vector2(0, 0), new Vector2(1, 1), 0, Color.White);
			//DrawMgr.Effect = null;
			
			//Frame f = SpritesDefault.DemonFire.Frames[(int)fireFrame];
			//DrawMgr.CurrentColor = Color.White;
			//DrawMgr.DrawRectangle(0, 0, SpritesDefault.DemonFire.W, SpritesDefault.DemonFire.H, true);
		
			DrawMgr.CurrentColor = Color.Aqua;
			
			
			DrawMgr.CurrentEffect = Effects.Effect;
			var p = new Vector2(50, 200);
			for(var i = 0; i < 8; i += 1)
			{
				DrawMgr.DrawSprite(
					testSpr, 
					0, 
					p + 
						Vector2.UnitX * i * 16 + 
						Vector2.UnitY * (float)Math.Sin(Math.PI * 2 * (chigginWave + 1f / 8f * i)) * 8, 
						Vector2.One, 
						i * 5, 
						Color.White * 0.5f
				);
			}
			
			DrawMgr.CurrentColor = Color.Violet * 0.5f;
			//DrawMgr.DrawRectangle(0, 0, 300, 300, false);

			DrawMgr.CurrentEffect = null;
			DrawMgr.CurrentFont = Fonts.Arial;
			
			DrawMgr.CurrentColor = Color.White * 0.5f;//new Color(0.5f, 0.5f, 0.5f, 0.5f);
			DrawMgr.DrawCircle(Input.MousePos, 2, false);
			DrawMgr.CurrentColor = Color.Green;//Color.Red * 0.5f;
			

			DrawMgr.DrawSprite(SpritesDefault.Flare, 400, 100);
			DrawMgr.DrawText("test", 100, 100);
			DrawMgr.DrawSprite(SpritesDefault.Flare, 400, 200);

			
			DrawMgr.CurrentColor = new Color(255, 0, 255, 255);
			DrawMgr.DrawSprite(SpritesDefault.Flare, 400, 300);

			DrawMgr.DrawCircle(500, 100, 32, false);
			DrawMgr.DrawRectangle(500, 200, 532, 232, false);
			DrawMgr.DrawLine(500, 300, 532, 300);
			DrawMgr.DrawLine(532, 400, 500, 400);

			

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
			*/

			DrawMgr.PrimitiveBegin();
			DrawMgr.PrimitiveSetTexture(SpritesDefault.Flare, 0);
			DrawMgr.PrimitiveAddVertex(new Vector2(120, 64), Color.White * 0.5f, new Vector2(0, 0));
			DrawMgr.PrimitiveAddVertex(new Vector2(120+32, 64), Color.Green * 1.0f, new Vector2(1, 0));
			DrawMgr.PrimitiveAddVertex(new Vector2(120+32, 64+32), Color.Blue, new Vector2(1, 1));
			DrawMgr.PrimitiveAddVertex(new Vector2(120, 64+32), Color.Red, new Vector2(0, 1));
			
			//DrawMgr.PrimitiveSetLineStripIndices(true);
			
			DrawMgr.PrimitiveSetTriangleFanIndices();
			DrawMgr.PrimitiveEnd();
			

			DrawMgr.PrimitiveBegin();
			//DrawMgr.PrimitiveSetTexture(SpritesDefault.BirdieBody, 0);
			DrawMgr.PrimitiveAddVertex(0, 0, new Vector2(0, 0));
			DrawMgr.PrimitiveAddVertex(32, 32, new Color(56, 135, 255) * 0.1f, new Vector2(0, 1));
			DrawMgr.PrimitiveAddVertex(64, 0,new Color(56, 135, 255) * 0.1f, new Vector2(1, 0));
			DrawMgr.PrimitiveAddVertex(96, 32, new Color(56, 135, 255) * 0.1f, new Vector2(1, 1));
			DrawMgr.PrimitiveAddVertex(64+32, 0,new Color(56, 135, 255) * 0.1f, new Vector2(1, 0));
			DrawMgr.PrimitiveAddVertex(96+32, 32, new Color(56, 135, 255), new Vector2(1, 1));
			DrawMgr.PrimitiveAddVertex(64+64, 0,new Color(56, 135, 255) , new Vector2(1, 0));
			DrawMgr.PrimitiveAddVertex(96+64, 32, new Color(56, 135, 255), new Vector2(1, 1));

			DrawMgr.PrimitiveSetTriangleStripIndices();
			DrawMgr.PrimitiveEnd();
			
			
			

			DrawMgr.PrimitiveBegin();
			DrawMgr.PrimitiveSetTexture(SpritesDefault.Barrel, 0);
			
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
