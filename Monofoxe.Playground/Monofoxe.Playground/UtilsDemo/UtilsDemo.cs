using System;
using System.Collections.Generic;
using System.Text;
using Monofoxe.Engine.EC;
using Monofoxe.Engine.Drawing;
using Monofoxe.Engine.Utils;
using Monofoxe.Engine;
using Monofoxe.Engine.SceneSystem;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Resources.Sprites;
using System.Text.RegularExpressions;
using Monofoxe.Engine.Cameras;
using Monofoxe.Engine.Resources;

namespace Monofoxe.Playground.UtilsDemo
{
	public enum TestStates
	{
		Green,
		Blue,
		Red,
	}

	// Note that not all the utilities and their functions are shown here.

	public class UtilsDemo : Entity
	{
		
		Color _mainColor = Color.White;
		Color _secondaryColor = Color.Violet;

		double _animationSpeed = 1;
		
		Animation _fireAnimation;
		
		TimeKeeper _slowTimeKeeper;

		Alarm _autoAlarm;

		Alarm _slowAlarm;

		double _alarmPeriod = 1;

		bool _autoAlarmSwitch = false;
		bool _slowAlarmSwitch = false;
		bool _counterSwitch = false;

		double _counter = 0;

		Camera2D _camera;

		StateMachine<TestStates> _stateMachine;
		Color _color;
		bool _isRectangle = false;
		bool _isOutline = false;

		RandomExt _random;

		Effect _grayscale;

		Sprite _fireSprite;

		public UtilsDemo(Layer layer) : base(layer)
		{
			_grayscale = ResourceHub.GetResource<Effect>("Effects", "Grayscale");
			_fireSprite = ResourceHub.GetResource<Sprite>("DefaultSprites", "Fire");


			// Animation.

			// Animation class is more sophisticated way of implementing animations.
			// It can be used for anything from sprite animation to UI element position.
			_fireAnimation = new Animation();
			_fireAnimation.Speed = _animationSpeed;

			// You can set an easing to make animation non-linear.
			// If it's not set, animation will be linear.
			_fireAnimation.Easing = Easing.EaseInCirc;

			// You can trigger a method at the end of an animation.
			_fireAnimation.AnimationEndEvent += FireAnimationEnd;
			_fireAnimation.Start(true);
			// Animation.


			// Alarms.
			_slowTimeKeeper = new TimeKeeper();
			// Slowing down time for this time keeper.
			_slowTimeKeeper.TimeMultiplier = 0.5f;

			_autoAlarm = new Alarm(_alarmPeriod, OnTriggerAction.Loop);
			_autoAlarm.Start();
			_slowAlarm = new Alarm(_alarmPeriod);
			// This alarm will now use custom TimeKeeper, which is 2 times 
			// slower than global TimeKeeper. 
			_slowAlarm.TimeKeeper = _slowTimeKeeper;
			_slowAlarm.Start();
			_slowAlarm.TriggerEvent += AlarmTrigger;
			// Alarms.


			// Camera.
			_camera = new Camera2D(400, 600);
			_camera.PortPosition = new Vector2(400, 0);
			_camera.BackgroundColor = Color.Black;
			_camera.PostprocessorEffects.Add(_grayscale);
			_camera.PostprocessingMode = PostprocessingMode.Camera;
			// Camera.


			_random = new RandomExt();


			// State machine.
			// State machines are very useful for animation and complex logic.
			_stateMachine = new StateMachine<TestStates>(TestStates.Green, this);
			
			// Filling up the state machine with events.
			_stateMachine.AddState(TestStates.Green, Green, GreenEnter, GreenExit);
			_stateMachine.AddState(TestStates.Blue, Blue);
			_stateMachine.AddState(TestStates.Red, Red);
			// State machine.
		}

		/// <summary>
		/// Triggerrs when animation loops.
		/// </summary>
		public void FireAnimationEnd(Animation animation)
		{
			// Inverting animation direction and setting appropriate easing.
			_fireAnimation.Invert = !_fireAnimation.Invert;
			if (_fireAnimation.Invert)
			{
				_fireAnimation.Easing = Easing.EaseOutCirc;
			}
			else
			{
				_fireAnimation.Easing = Easing.EaseInCirc;
			}
		}

		/// <summary>
		/// Triggers when alarm runs out.
		/// </summary>
		public void AlarmTrigger(Alarm alarm)
		{
			// This is actually a bad practice. 
			// Alarm in OnTriggerAction.Stop mode doesn't take into account leftover time
			// and will reset counter to 0. This will introduce errors,
			// which will add up over time. If you need to set Alarm 
			// right after it triggers, use OnTriggerAction.Loop.
			//
			// This demo shows comparison of Stop and Loop modes.
			// You will notice that circles will start blinking out of sync
			// over time.
			alarm.Start(); 

			_slowAlarmSwitch = !_slowAlarmSwitch;
		}


		#region State machine.

		public void GreenEnter(StateMachine<TestStates> stateMachine, Entity caller)
		{
			// Here caller isn't really used, but it will be necessary if you're using EC.
			_isRectangle = !_isRectangle;
		}
		public void Green(StateMachine<TestStates> stateMachine, Entity caller)
		{
			_color = Color.Green;
			if (_random.NextDouble() > 0.9)
			{
				stateMachine.ChangeState(TestStates.Blue);
				return;
			}
			if (_random.NextDouble() > 0.8)
			{
				stateMachine.ChangeState(TestStates.Red);
				return;
			}
		}
		public void GreenExit(StateMachine<TestStates> stateMachine, Entity caller)
		{
			_isOutline = !_isOutline;
		}

		public void Blue(StateMachine<TestStates> stateMachine, Entity caller)
		{
			_color = Color.Blue;
			if (_random.NextDouble() > 0.9)
			{
				stateMachine.ChangeState(TestStates.Red);
				return;
			}
		}

		public void Red(StateMachine<TestStates> stateMachine, Entity caller)
		{
			_color = Color.Red;
			if (_random.NextDouble() > 0.97)
			{
				stateMachine.ChangeState(TestStates.Green);
				return;
			}
		}

		#endregion State machine.


		public override void Update()
		{
			base.Update();

			// All of those are not entities, so they have to be updated manually.
			
			// It needs to be updated automatically.
			_fireAnimation.Update();

			_slowAlarm.Update();

			// You also can not use TriggerAction and check if the alarm was triggered this way.
			if (_autoAlarm.Update())
			{
				_autoAlarmSwitch = !_autoAlarmSwitch;
			}


			// You can count time by hand. Not that you should, it just
			// shows how time keepers are used internally.
			// It is usually used for different tasks, which alarms can't really handle.
			_counter += _slowTimeKeeper.Time();
			if (_counter > _alarmPeriod)
			{
				// Subtracting instead of zeroing will correct for time errors.
				_counter -= _alarmPeriod;
				_counterSwitch = !_counterSwitch;
			}

			_stateMachine.Update();

		}


		public override void Draw()
		{
			base.Draw();

			var startingPosition = new Vector2(64, 64);
			var position = startingPosition;
			var spacing = 100;

			GraphicsMgr.CurrentColor = Color.White;
			
			_fireSprite.Draw(position, _fireAnimation.Progress, Vector2.Zero, Vector2.One, Angle.Right, Color.White);
			
			GraphicsMgr.CurrentColor = Color.SeaGreen;
			position += Vector2.UnitX * spacing;
			CircleShape.Draw(position, 8, _autoAlarmSwitch);


			GraphicsMgr.CurrentColor = Color.Sienna;
			position += Vector2.UnitX * 32;
			CircleShape.Draw(position, 8, _slowAlarmSwitch);


			GraphicsMgr.CurrentColor = Color.Thistle;
			position += Vector2.UnitX * 32;
			CircleShape.Draw(position, 8, _counterSwitch);

			position += Vector2.UnitX * 32;
			GraphicsMgr.CurrentColor = _color;
			if (_isRectangle)
			{
				RectangleShape.DrawBySize(position, Vector2.One * 16, _isOutline);
			}
			else
			{
				CircleShape.Draw(position, 8, _isOutline);
			}



		}

		public override void Destroy()
		{
			base.Destroy();
			_camera.Dispose();
		}


	}
}
