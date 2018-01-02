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

		
		public TestObj()
		{
			GameCntrl.MaxGameSpeed = 30;
		}

		public override void Update()
		{

			var btn = InputCntrl.GP.RT;
			
			if (InputCntrl.GamepadCheckPress(0, btn))
			{Debug.WriteLine("Pressed!");}
			
			if (InputCntrl.GamepadCheck(0, btn))
			{Debug.WriteLine("Holding!");}
			
			if (InputCntrl.GamepadCheckRelease(0, btn))
			{Debug.WriteLine("Released!");}
			//GameCntrl.MaxGameSpeed = 5;

			ang += GameCntrl.Time((Math.PI * 2) / period);

			if (ang >= Math.PI * 2)
			{
				ang -= Math.PI * 2;
			//	Debug.WriteLine("fps: " + GameCntrl.Fps + " " + GameCntrl.ElapsedTimeTotal);
			}

			if (InputCntrl.KeyboardString.Length > 0)
			{Debug.WriteLine(InputCntrl.KeyboardString);}

			//x = (int)(100 + 100*Math.Cos(ang));
			//y = (int)(100 + 100*Math.Sin(ang));
			y+=InputCntrl.MouseWheelVal * 10;
			
		}

		public override void Draw()
		{
			Rectangle rect = new Rectangle(new Point(x, y), new Point(Game1.tex.Width/2, Game1.tex.Height/2));
			Rectangle rect1 = new Rectangle(new Point(0, 0), new Point(Game1.tex.Width, Game1.tex.Height));
			
			Vector2 gp = new Vector2 (InputCntrl.GamepadGetRightTrigger(0), InputCntrl.GamepadGetLeftTrigger(0));
			//Debug.WriteLine(gp.X);
			Rectangle rect2 = new Rectangle(new Point((int)(200+gp.X*100), (int)(200-gp.Y*100)), new Point(Game1.tex.Width/10, Game1.tex.Height/10));
			Rectangle rect3 = new Rectangle(new Point(0, 0), new Point(Game1.tex.Width, Game1.tex.Height));

			
			Game1.spriteBatch.Begin();
			Game1.spriteBatch.Draw(Game1.tex, rect, rect1, Color.White, -1, new Vector2(0, 0), SpriteEffects.None, 0);
			
			//for(var i = 0; i < 10000; i += 1)
			//{
				Game1.spriteBatch.Draw(Game1.tex, rect2, rect3, Color.White, -1, new Vector2(0, 0), SpriteEffects.None, 0);
				//spriteBatch.Draw(tex, new Vector2(0, 0), Color.White);
			//}
		
			Game1.spriteBatch.End();

		}
	}
}
