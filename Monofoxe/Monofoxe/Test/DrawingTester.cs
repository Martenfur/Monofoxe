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
using System.Collections;
using System.Collections.Generic;


namespace Monofoxe.Test
{
	public class DrawingTester : Entity
	{
		Sprite testSpr = Default.BstGam;
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
			
			var frame = Math.Max(0, Math.Min(Default.DemonFire.Frames.Length - 1, (int)(fireFrame * Default.DemonFire.Frames.Length)));

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
			
			//fireFrame
			GraphicsMgr.CurrentColor = Color.White;
	
			Default.DemonFire.Draw(
				0.5f, 
				new Vector2(32, 32), 
				Default.DemonFire.Origin,
				new Vector2((float)Math.Cos(GameMgr.ElapsedTimeTotal), 
				(float)Math.Sin(GameMgr.ElapsedTimeTotal)), 
				90, 
				Color.White
			);
			CircleShape.Draw(new Vector2(32, 32), 2, true);
			
			//DrawMgr.Effect = null;
			
			//Frame f = SpritesDefault.DemonFire.Frames[(int)fireFrame];
			//DrawMgr.CurrentColor = Color.White;
			//DrawMgr.DrawRectangle(0, 0, SpritesDefault.DemonFire.W, SpritesDefault.DemonFire.H, true);
		
			GraphicsMgr.CurrentColor = Color.Aqua;
			
			
			GraphicsMgr.CurrentEffect = Effects.BW;
			var p = new Vector2(50, 200);
			for(var i = 0; i < 8; i += 1)
			{
				testSpr.Draw(
					0.75f, 
					p + 
						Vector2.UnitX * i * 16 + 
						Vector2.UnitY * (float)Math.Sin(Math.PI * 2 * (chigginWave + 1f / 8f * i)) * 8, 
					testSpr.Origin,
					Vector2.One,
					i * 5, 
					Color.White * 0.5f
				);
			}
			
			GraphicsMgr.CurrentColor = Color.Violet * 0.5f;
			//DrawMgr.DrawRectangle(0, 0, 300, 300, false);

			GraphicsMgr.CurrentEffect = null;
			Text.CurrentFont = Fonts.Arial;
			
			GraphicsMgr.CurrentColor = Color.White * 0.5f;//new Color(0.5f, 0.5f, 0.5f, 0.5f);
			CircleShape.Draw(Input.MousePosition, 2, false);
			GraphicsMgr.CurrentColor = Color.Green;//Color.Red * 0.5f;
			

			Default.Flare.Draw(new Vector2(400, 100), Default.Flare.Origin);
			Text.Draw("test", 100, 100);			
			Default.Flare.Draw(new Vector2(400, 200), Default.Flare.Origin);

			
			GraphicsMgr.CurrentColor = new Color(255, 0, 255, 255);
			Default.Flare.Draw(new Vector2(400, 300), Default.Flare.Origin);

			CircleShape.Draw(500, 100, 32, false);
			RectangleShape.Draw(500, 200, 532, 232, false);
			LineShape.Draw(500, 300, 532, 300);
			LineShape.Draw(532, 400, 500, 400);
			
			

			TestDrawPrimitives();
		}



		private void TestDrawPrimitives()
		{
			var fan = new TriangleFanPrimitive();
			fan.SetTextureFromFrame(Default.BstGam.Frames[0]);
			fan.Position = new Vector2(120, 64);
			fan.Vertices = new List<Vertex>
			{
				new Vertex(new Vector2(0, 0), Color.White * 0.5f, new Vector2(0, 0)),
				new Vertex(new Vector2(32, 0), Color.Green * 1.0f, new Vector2(1, 0)),
				new Vertex(new Vector2(32, 32), Color.Blue, new Vector2(1, 1)),
				new Vertex(new Vector2(0, 32), Color.Red, new Vector2(0, 1)),
			};
			fan.Draw();
			
			
			var strip = new TriangleStripPrimitive();
			strip.Vertices = new List<Vertex>
			{

				new Vertex(new Vector2(0, 0)),
				new Vertex(new Vector2(32, 32), new Color(56, 135, 255) * 0.1f, new Vector2(0, 1)),
				new Vertex(new Vector2(64, 0), new Color(56, 135, 255) * 0.1f, new Vector2(1, 0)),
				new Vertex(new Vector2(96, 32), new Color(56, 135, 255) * 0.1f, new Vector2(1, 1)),
				new Vertex(new Vector2(64+32, 0), new Color(56, 135, 255) * 0.1f, new Vector2(1, 0)),
				new Vertex(new Vector2(96+32, 32), new Color(56, 135, 255), new Vector2(1, 1)),
				new Vertex(new Vector2(64+64, 0), new Color(56, 135, 255) , new Vector2(1, 0)),
				new Vertex(new Vector2(96+64, 32), new Color(56, 135, 255), new Vector2(1, 1)),
			};
			strip.Draw();

			var mesh = new MeshPrimitive(7, 7);
			mesh.Position = new Vector2(0, 100);
			mesh.SetTextureFromFrame(Default.Barrel.Frames[0]);
			mesh.Vertices = new List<Vertex>();
			
			int w = 7;
			int h = 7;
			
			for(var k = 0; k < h; k += 1)
			{
				for(var i = 0; i < w; i += 1)
				{			
					mesh.Vertices.Add(new Vertex(new Vector2(8 * i * i, 8 * k), Color.White, new Vector2(i / (float)(w - 1), k / (float)(h - 1))));	
				}
			}

			mesh.Draw();
			
		}
		

	}
}
