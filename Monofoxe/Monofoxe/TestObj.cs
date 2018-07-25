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
using Resources.Sprites;
using Resources;
using Monofoxe.Engine.Audio;

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

		float fireFrame = 0;

		RandomExt r = new RandomExt();

		AutoAlarm auto1 = new AutoAlarm(1);
		AutoAlarm auto2 = new AutoAlarm(1);

		Timer timer = new Timer();

		RenderTarget2D surfForDrawing;

		float lowpass = 1f;

		Sound snd1;
		Sound snd2;
		Sound snd3;

		FMOD.ChannelGroup group;//new FMOD.ChannelGroup((IntPtr)0);

		public TestObj()
		{
			snd1 = AudioMgr.LoadStreamedSound("Music/m_mission", FMOD.MODE._3D);
			snd2 = AudioMgr.LoadStreamedSound("Music/m_peace");
			snd3 = AudioMgr.LoadSound("Sounds/punch", FMOD.MODE._3D);
			
			AudioMgr.ListenerCount = 1;
			AudioMgr.SetListenerPosition(new Vector2(256, 256), 0);

			group = AudioMgr.CreateChannelGroup("group");
			
			GameCntrl.GameSpeedMultiplier = 1;
			auto1.AffectedBySpeedMultiplier = false;

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
			Effects.Effect.Parameters["test"].SetValue(new Vector4(0.5f, 0.3f, 0.1f, 1));
			DrawCntrl.Effect = Effects.Effect;
			DrawCntrl.Device.Clear(Color.Black);
			DrawCntrl.DrawCircle(256, 256, 256, false);
			DrawCntrl.DrawSprite(SpritesDefault.Chiggin, 0, 0);

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
			snd1.Set3DAttributes(Input.ScreenMousePos, Vector2.Zero);
			snd1.Set3DMinMaxDistance(100, 300);

			Debug.WriteLine(snd1.Volume);
			GameCntrl.WindowManager.WindowTitle = "Draw fps: " + GameCntrl.Fps;
			
			if (Input.KeyboardCheckPress(Keys.A))
			{
				snd1.Play(group);
			}
			if (Input.KeyboardCheckPress(Keys.S))
			{
				snd2.Play(group);
			}
			if (Input.KeyboardCheck(Keys.D))
			{
				if (!snd3.IsPlaying)
				{
					snd3.Play(group);
					snd3.Channel?.setReverbProperties(0, lowpass);
				}
			}

			if (Input.KeyboardCheckPress(Keys.O))
			{
				group.stop();
			}

			if (Input.KeyboardCheck(Keys.Q))
			{
				lowpass += 0.1f;
				if (lowpass > 1000)
				{
					lowpass = 1;
				}	
				group.setLowPassGain(lowpass);

				//snd1.LowPass = lowpass;
			}
			if (Input.KeyboardCheck(Keys.W))
			{
				lowpass -= 0.1f;
				if (lowpass < 0.1f)
				{
					lowpass = 0.1f;
				}
				//snd1.LowPass = lowpass;
				group.setLowPassGain(lowpass);
			}

			fireFrame += 0.1f;

			if (fireFrame >= SpritesDefault.DemonFire.Frames.Count())
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
				Effects.Effect.Parameters["test"].SetValue(new Vector4(0.0f, 0.7f, 0.0f, 1.0f));
				DrawCntrl.Effect = Effects.Effect;
			}
			else
			{
				//DrawCntrl.BlendState = BlendState.AlphaBlend;
			}
			

			DrawCntrl.CurrentColor = Color.Violet;
			//DrawCntrl.DrawRectangle(-32, -32, 500, 500, false);
			DrawCntrl.DrawSprite(SpritesDefault.BstGam, 0, Vector2.Zero);
			
			DrawCntrl.DrawSprite(SpritesDefault.DemonFire, (int)fireFrame, new Vector2(0, 0), new Vector2(1, 1), 0, Color.White);

			
			Frame f = SpritesDefault.DemonFire.Frames[(int)fireFrame];
			DrawCntrl.CurrentColor = Color.White;
			DrawCntrl.DrawRectangle(0, 0, SpritesDefault.DemonFire.W, SpritesDefault.DemonFire.H, true);
			
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
			
			DrawCntrl.DrawSprite(SpritesDefault.Boss, new Vector2(200, 200));
			DrawCntrl.DrawSprite(SpritesDefault.Boulder3, new Vector2(400, 200));
			
			DrawCntrl.DrawSurface(surf, 128, 128);

			DrawCntrl.Effect = null;
			
			
		}

		public override void DrawGUI()
		{
			DrawCntrl.CurrentColor = Color.Red;


			DrawCntrl.CurrentColor = new Color(Color.White, 0.5f);
			DrawCntrl.DrawSurface(surfForDrawing, 0, 0);
			DrawCntrl.DrawCircle(Input.ScreenMousePos, 8, false);

			var spr = SpritesDefault.Scene3TreeLeft;

			
			//DrawCntrl.CurrentColor = new Color(Color.White, 0.5f);
			//DrawCntrl.DrawSprite(spr, 0, 0);
			
			DrawCntrl.CurrentColor = Color.White;
			DrawCntrl.DrawSurface(surfForDrawing, new Rectangle(32, 32, 32, 48), new Rectangle(32, 32, 32, 48));

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
			DrawCntrl.PrimitiveSetTexture(SpritesDefault.BirdieBody, 0);
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
			DrawCntrl.PrimitiveSetTexture(SpritesDefault.Boulder3, 0);
			DrawCntrl.PrimitiveSetMeshIndices(w, h);
			DrawCntrl.PrimitiveEnd();
		}

	}
}
