using System;
using System.Collections.Generic;
//using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Monofoxe.Engine;
using Monofoxe.Engine.Drawing;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Diagnostics;
using Monofoxe.Utils;
using Resources.Sprites;
using Resources;
using Monofoxe.Engine.Audio;
using Monofoxe.Engine.ECS;
using Monofoxe.ECSTest.Systems;
using Monofoxe.ECSTest.Components;
using MonoGame.Extended.Tiled;
using Newtonsoft.Json;
using System.IO;
using Newtonsoft.Json.Linq;


namespace Monofoxe
{
	class TestObj : Entity 
	{
		
		TiledMap map;	

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

		Entity entity;

		Sprite testSpr;

		public TestObj()
		{
				
			//map.ObjectLayers[0].Objects[0];
			
			var raw = File.ReadAllText("Content/Entities/TestTemplate.json");
			JToken testData = JObject.Parse(raw);
			Console.WriteLine(testData["components"][1]);
			
			var mov = JsonConvert.DeserializeObject<CMovement>(testData["components"][1].ToString());

			testSpr = mov.Spr;

			ComponentSystemMgr.Systems.Add(new SCollision());
			ComponentSystemMgr.Systems.Add(new TestSystem());
			

			snd1 = AudioMgr.LoadStreamedSound("Music/m_mission", FMOD.MODE._3D);
			snd2 = AudioMgr.LoadStreamedSound("Music/m_peace");
			snd3 = AudioMgr.LoadSound("Sounds/punch", FMOD.MODE._3D);
			
			AudioMgr.ListenerCount = 1;
			AudioMgr.SetListenerPosition(new Vector2(256, 256), 0);

			group = AudioMgr.CreateChannelGroup("group");
			
			GameMgr.GameSpeedMultiplier = 1;
			auto1.AffectedBySpeedMultiplier = false;

			GameMgr.MaxGameSpeed = 60;
			GameMgr.FixedUpdateRate = 1.0 / 30.0;
			surf = new RenderTarget2D(
				DrawMgr.Device, 
				512, 
				512, 
				false,
				DrawMgr.Device.PresentationParameters.BackBufferFormat,
				DepthFormat.Depth24, 
				0, 
				RenderTargetUsage.PreserveContents
			);
			surfForDrawing = new RenderTarget2D(
				DrawMgr.Device, 
				128, 
				128, 
				false, 
				DrawMgr.Device.PresentationParameters.BackBufferFormat,
				DepthFormat.Depth24, 
				0, 
				RenderTargetUsage.PreserveContents
			);
			

			
			DrawMgr.SetSurfaceTarget(surf);
			Effects.Effect.Parameters["test"].SetValue(new Vector4(0.5f, 0.3f, 0.1f, 1));
			DrawMgr.Effect = Effects.Effect;
			DrawMgr.Device.Clear(Color.Black);
			DrawMgr.DrawCircle(256, 256, 256, false);
			DrawMgr.DrawSprite(SpritesDefault.Chiggin, 0, 0);

			DrawMgr.Effect = null;
			DrawMgr.ResetSurfaceTarget();

			DrawMgr.BlendState = BlendState.NonPremultiplied; // Makes alpha magically work.


			cam.BackgroundColor = Color.DarkSeaGreen;

			cam.Offset = cam.Size / 2;

			x = cam.Size.X / 2;
			y = cam.Size.Y / 2;

			cam1.PortPos.X = 600;
			cam1.BackgroundColor = Color.DarkSeaGreen;
			cam1.Enabled = false;

			RasterizerState rasterizerState = new RasterizerState(); // Do something with it, I guees.
			rasterizerState.CullMode = CullMode.None;
			rasterizerState.ScissorTestEnable = false;//(_scissorRectangle != Rectangle.Empty);
			rasterizerState.FillMode = FillMode.Solid;
			
			DrawMgr.Rasterizer = rasterizerState;

			DrawMgr.Sampler = SamplerState.PointClamp;


			//DrawCntrl.ScissorRectangle = new Rectangle(0, 0, 100, 100);
			GameMgr.WindowManager.CanvasSize = new Vector2(1200, 600);
			GameMgr.WindowManager.Window.AllowUserResizing = false;
			GameMgr.WindowManager.ApplyChanges();
			GameMgr.WindowManager.CenterWindow();
			
			GameMgr.WindowManager.CanvasMode = CanvasMode.Fill; 
		}
		
		public override void Update()
		{
			
			snd1.Set3DAttributes(Input.ScreenMousePos, Vector2.Zero);
			snd1.Set3DMinMaxDistance(100, 300);

			GameMgr.WindowManager.WindowTitle = "Draw fps: " + GameMgr.Fps;
			
			if (Input.CheckButtonPress(Buttons.A))
			{
				//snd1.Play(group);
				entity = Entities.CreateSomething();
				((TestComponent)entity["test"]).Position = new Vector2(32, 32);
			}
			if (Input.CheckButtonPress(Buttons.S))
			{
				snd2.Play(group);
			}
			if (Input.CheckButton(Buttons.D))
			{
				if (!snd3.IsPlaying)
				{
					snd3.Play(group);
					snd3.Channel?.setReverbProperties(0, lowpass);
				}
			}

			if (Input.CheckButtonPress(Buttons.O))
			{
				group.stop();
			}

			if (Input.CheckButton(Buttons.Q))
			{
				lowpass += 0.1f;
				if (lowpass > 1000)
				{
					lowpass = 1;
				}	
				group.setLowPassGain(lowpass);

				//snd1.LowPass = lowpass;
			}
			if (Input.CheckButton(Buttons.W))
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

			if (fireFrame >= SpritesDefault.DemonFire.Frames.Length)
			{
				fireFrame = 0;
			}

			if (Input.CheckButton(Buttons.MouseRight))
			{
				var ball = Entities.CreateBall();
				ball.GetComponent<CMovement>().Position = Input.MousePos;
				ball.GetComponent<CCollision>().MaskR = r.Next(32, 64);
				Console.WriteLine(EntityMgr.Count("ball"));
			}
			

			#region Camera. 

			ang += GameMgr.Time((Math.PI * 2) / period);

			if (ang >= Math.PI * 2)
			{
				ang -= Math.PI * 2;
			}
			
			if (Input.CheckButton(Buttons.Left))
			{x += (5 / cam.Zoom);}
			
			if (Input.CheckButton(Buttons.Right))
			{x -= (5 / cam.Zoom);}
			
			if (Input.CheckButton(Buttons.Up))
			{y += (5 / cam.Zoom);}
			
			if (Input.CheckButton(Buttons.Down))
			{y -= (5 / cam.Zoom);}
			
			if (Input.CheckButton(Buttons.Z))
			{
				cam.Zoom += 0.1f;
			}
			
			if (Input.CheckButton(Buttons.X))
			{
				cam.Zoom -= 0.1f;
				if (cam.Zoom <= 0)
				{
					cam.Zoom = 0.1f;
				}
			}
			
			if (Input.CheckButton(Buttons.C))
			{cam.Rotation += 5;}

			if (Input.CheckButton(Buttons.V))
			{cam.Rotation -= 5;}

			if (Input.CheckButton(Buttons.Escape))
			{
				GameMgr.ExitGame();
			}
			
			if (Input.CheckButtonPress(Buttons.F))
			{
				GameMgr.WindowManager.SetFullScreen(!GameMgr.WindowManager.IsFullScreen);	
			}
			cam.Pos.X = x;
			cam.Pos.Y = y;

			#endregion Camera. 
			
			DrawMgr.CurrentColor = new Color((float)r.NextDouble(), (float)r.NextDouble(), (float)r.NextDouble(), 1.0f);
			DrawMgr.SetSurfaceTarget(surfForDrawing);
			if (Input.CheckButton(Buttons.MouseLeft))
			{
				DrawMgr.DrawCircle(Input.ScreenMousePos, 16, false);
			}
			DrawMgr.ResetSurfaceTarget();
			
		}

		string str;
		
		public override void Draw()
		{	
			if (DrawMgr.CurrentCamera == cam)
			{
				//DrawCntrl.BlendState = BlendState.Additive;
			//	Effects.Effect.Parameters["test"].SetValue(new Vector4(0.0f, 0.7f, 0.0f, 1.0f));
			//	DrawMgr.Effect = Effects.Effect;
			}
			else
			{
				//DrawCntrl.BlendState = BlendState.AlphaBlend;
			}
			

			DrawMgr.CurrentColor = Color.Violet;
			//DrawCntrl.DrawRectangle(-32, -32, 500, 500, false);
			
			DrawMgr.DrawSprite(SpritesDefault.DemonFire, (int)fireFrame, new Vector2(0, 0), new Vector2(1, 1), 0, Color.White);

			
			Frame f = SpritesDefault.DemonFire.Frames[(int)fireFrame];
			DrawMgr.CurrentColor = Color.White;
			DrawMgr.DrawRectangle(0, 0, SpritesDefault.DemonFire.W, SpritesDefault.DemonFire.H, true);
		
			DrawMgr.CurrentColor = Color.White;
			
			DrawMgr.Effect = null;
			

			var p = new Vector2(50, 200);
			for(var i = 0; i < 8; i += 1)
				DrawMgr.DrawSprite(testSpr, 0, p + Vector2.UnitX * i * 16, Vector2.One, i * 5, new Color(Color.White, 0.5f));
			
			DrawMgr.CurrentColor = Color.Black;
			DrawMgr.CurrentFont = Fonts.Def;
			str += Input.KeyboardString;
			DrawMgr.DrawText(str, 400, 300);
		}

		public override void DrawGUI()
		{
			DrawMgr.CurrentColor = Color.Red;


			DrawMgr.CurrentColor = new Color(Color.White, 0.5f);
			DrawMgr.DrawSurface(surfForDrawing, 0, 0);
			DrawMgr.DrawCircle(Input.ScreenMousePos, 8, false);

			var spr = SpritesDefault.Scene3TreeLeft;

			
			//DrawCntrl.CurrentColor = new Color(Color.White, 0.5f);
			//DrawCntrl.DrawSprite(spr, 0, 0);
			

			DrawMgr.CurrentColor = Color.White;
			DrawMgr.DrawSurface(surfForDrawing, new Rectangle(32, 32, 32, 48), new Rectangle(32, 32, 32, 48));

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
