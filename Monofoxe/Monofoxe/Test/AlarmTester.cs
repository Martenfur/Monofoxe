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
		Sprite spr = Default.BstGam;
		float scale = 1;
		float scale1 = 1;
		float period = 1;
		
		TimeKeeper defKeeper = new TimeKeeper();
		TimeKeeper slowKeeper = new TimeKeeper();


		AutoAlarm autoAlarm;
		Alarm slowAlarm;

		Timer timer;
		Timer slowTimer;

		public AlarmTester(Layer layer) : base(layer)
		{
			autoAlarm = new AutoAlarm(period, defKeeper, AutoAlarmAction);
			slowAlarm = new Alarm(slowKeeper, SlowAlarmAction);
			slowAlarm.Set(period);

			slowKeeper.TimeMultiplier = 0.5;

			timer = new Timer(defKeeper);
			slowTimer = new Timer(slowKeeper);
		}

		public override void Update()
		{
			autoAlarm.Update();
			slowAlarm.Update();
			timer.Update();
			slowTimer.Update();
		}

		public void AutoAlarmAction(Alarm owner)
		{
			scale *= -1;
		}

		public void SlowAlarmAction(Alarm owner)
		{
			scale1 *= -1;
			owner.Set(period + owner.Counter);
		}

		public override void Draw()
		{
			//spr.Draw(0, 200, 100, scale * 0.5f, 0.5f, 0, Color.White);
			//GraphicsMgr.DrawSprite(spr, 0, 200, 300, scale1 * 0.5f, 0.5f, 0, Color.White);

			GraphicsMgr.CurrentColor = Color.White;
			Text.CurrentFont = Resources.Fonts.Arial;

			Text.Draw("Normal: " + timer.Counter + "\nSlow: " + slowTimer.Counter, 32, 32);
		}

	}
}
