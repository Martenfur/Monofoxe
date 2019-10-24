using System;

namespace Monofoxe.Engine.Utils
{
	public delegate void AnimationDelegate(Animation caller);

	/// <summary>
	/// Basic animation class. Can be used for sprite animation, 
	/// UI animation, movement, and many more.
	/// </summary>
	public class Animation
	{
		/// <summary>
		/// Linear animation progress. Always goes up from 0 to 1.
		/// Not affected by easing and inversion.
		/// </summary>
		public double LinearProgress;
		
		/// <summary>
		/// LinearProgress affected by easing and inversion.
		/// </summary>
		public double Progress
		{
			get
			{
				var progress = LinearProgress;
				if (Invert)
				{
					progress = 1 - LinearProgress;
				}

				if (Easing == null)
				{
					return progress;
				}
				else
				{
					return Easing.GetEasing(progress);
				}
			}
		}

		/// <summary>
		/// Animation speed.
		/// </summary>
		public double Speed = 0.0;

		/// <summary>
		/// If true, animation will be looped and properly time-corrected.
		/// </summary>
		public bool Looping = false;

		/// <summary>
		/// Tells if the animation is running.
		/// </summary>
		public bool Running {get; private set;} = false;

		/// <summary>
		/// Current easing function, which is applied to animation.
		/// If null, animation will be linear.
		/// </summary>
		public Easing Easing;

		/// <summary>
		/// Current TimeKeeper. Can affect animation speed.
		/// </summary>
		public TimeKeeper TimeKeeper = TimeKeeper.Global;
		
		/// <summary>
		/// If true, Progress will go from 1 to 0 insteasd of 0 to 1.
		/// </summary>
		public bool Invert = false;

		/// <summary>
		/// Gets called, when animation ends or loops.
		/// Use this for more fine control over the animation.
		/// </summary>
		public event AnimationDelegate AnimationEndEvent;


		/// <summary>
		/// Updates the animation. Not the part of entity loop, 
		/// you need to call it on your own.
		/// 
		/// NOTE: If using in Draw method, keep in mind that each camera calls the event separately.
		/// You may want to restrict it only to the first camera.
		/// </summary>
		public void Update()
		{
			if (!Running)
			{
				return;
			}

			LinearProgress += TimeKeeper.Time(Math.Abs(Speed));
			
			if (LinearProgress > 1) 
			{
				if (!Looping)
				{
					Running = false;
					LinearProgress = 1;
				}
				else
				{
					LinearProgress -= (int)LinearProgress;
				}

				AnimationEndEvent?.Invoke(this);
			}

		}

		/// <summary>
		/// Starts the animation and resets LinearProgress to 0.
		/// </summary>
		public void Start()
		{
			Running = true;
			LinearProgress = 0;
		}

		/// <summary>
		/// Starts the animation, overrides Looping and resets LinearProgress to 0.
		/// </summary>
		public void Start(bool looping)
		{
			Running = true;
			Looping = looping;
			LinearProgress = 0;
		}

		/// <summary>
		/// Stops the animation and resets LinearProgress to 0.
		/// </summary>
		public void Stop()
		{
			Running = false;
			LinearProgress = 0;
		}

		/// <summary>
		/// Resumes the animation from its current progress.
		/// </summary>
		public void Resume() =>
			Running = true;

		/// <summary>
		/// Resumes the animation from its current progress and overrides Looping.
		/// </summary>
		public void Resume(bool looping)
		{
			Running = true;
			Looping = looping;
		}

		/// <summary>
		/// Stops the animation without resetting current progress.
		/// </summary>
		public void Pause() =>
			Running = false;

	}
}
