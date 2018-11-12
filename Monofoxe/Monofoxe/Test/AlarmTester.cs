using System;
using System.Collections.Generic;
using Monofoxe.Engine;
using Monofoxe.Engine.Utils;
using Microsoft.Xna.Framework.Graphics;
using Monofoxe.Engine.ECS;
using Monofoxe.Engine.Drawing;
using Microsoft.Xna.Framework;
using Resources.Sprites;
using Monofoxe.Engine.SceneSystem;


namespace Monofoxe.Test
{
	public class AlarmTester : Entity
	{
		Sprite spr = SpritesDefault.BstGam;
		float scale = 1;
		float scale1 = 1;
		float period = 1;
		
		TimeKeeper defKeeper = new TimeKeeper();
		TimeKeeper slowKeeper = new TimeKeeper();


		AutoAlarm autoAlarm;
		AutoAlarm slowAutoAlarm;

		Timer timer;
		Timer slowTimer;

		public AlarmTester(Layer layer) : base(layer)
		{
			autoAlarm = new AutoAlarm(period, defKeeper);
			slowAutoAlarm = new AutoAlarm(period, slowKeeper);
			slowKeeper.TimeMultiplier = 0.5;

			timer = new Timer(defKeeper);
			slowTimer = new Timer(slowKeeper);
		}

		public override void Update()
		{
			if (autoAlarm.Update())
			{
				scale *= -1;
			}

			if (slowAutoAlarm.Update())
			{
				scale1 *= -1;
			}
			timer.Update();
			slowTimer.Update();
		}

		public override void Draw()
		{
			DrawMgr.DrawSprite(spr, 0, 600, 100, scale * 0.5f, 0.5f, 0, Color.White);
			DrawMgr.DrawSprite(spr, 0, 600, 300, scale1 * 0.5f, 0.5f, 0, Color.White);

			DrawMgr.CurrentColor = Color.White;
			DrawMgr.CurrentFont = Resources.Fonts.Arial;

			DrawMgr.DrawText("Normal: " + timer.Counter + "\nSlow: " + slowTimer.Counter, 32, 32);
		}

	}
}
