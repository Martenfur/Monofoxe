using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Monofoxe.Engine;
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

		public TestObj()
		{
			GameCntrl.MaxGameSpeed = 30;
			surf = new RenderTarget2D(DrawCntrl.Device, 512, 512);

			SpriteBatch batch = new SpriteBatch(DrawCntrl.Device);

			DrawCntrl.Device.SetRenderTarget(surf);

			batch.Begin();

			batch.Draw(Game1.tex, new Vector2(32, 32), Color.White);

			batch.End();

			DrawCntrl.Device.SetRenderTarget(null);


			var cam = new Camera(100, 100, 640, 480);
			
			var cam1 = new Camera(0, 0, 640, 480);
			cam.Rotation = 10;

		}

		public override void Update()
		{

			var btn = Input.GP.RT;
			
			if (Input.GamepadCheckPress(0, btn))
			{Debug.WriteLine("Pressed!");}
			
			if (Input.GamepadCheck(0, btn))
			{Debug.WriteLine("Holding!");}
			
			if (Input.GamepadCheckRelease(0, btn))
			{Debug.WriteLine("Released!");}
			//GameCntrl.MaxGameSpeed = 5;

			ang += GameCntrl.Time((Math.PI * 2) / period);

			if (ang >= Math.PI * 2)
			{
				ang -= Math.PI * 2;
			//	Debug.WriteLine("fps: " + GameCntrl.Fps + " " + GameCntrl.ElapsedTimeTotal);
			}
			Input.GamepadSetVibration(0,1,1);
			//Debug.Write(ObjCntrl.ObjExists(o));
			x = (int)(100 + 100*Math.Cos(ang));
			y = (int)(100 + 100*Math.Sin(ang));
			//y+=Input.MouseWheelVal * 10;
		}

		public override void Draw()
		{
			Vector2 gp = Input.GamepadGetLeftStick(0);
			//Debug.WriteLine(gp.ToString());
			
			
			DrawCntrl.DrawSprite(Game1.tex, x, y, Color.White);
			
			//DrawCntrl.DrawSurface(surf, 0, 0, Color.White);
			
		}
	}
}
