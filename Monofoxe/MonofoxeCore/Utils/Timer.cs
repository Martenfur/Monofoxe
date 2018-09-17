using Monofoxe.Engine;

namespace Monofoxe.Utils
{
	/// <summary>
	/// Counts seconds. Needs to be updated manually.
	/// </summary>
	public class Timer
	{
		/// <summary>
		/// Tells how much time has passed in seconds.
		/// </summary>
		public virtual double Counter {get; protected set;} = 0;
		
		/// <summary>
		/// Timer won't update if it's inactive.
		/// </summary>
		public bool Active = true;

		/// <summary>
		/// Tells if timer is affected by GameCntrl.GameSpeedMultiplier.
		/// </summary>
		public bool AffectedBySpeedMultiplier = true;


		public TimeKeeper TimeKeeper;

		public Timer() {}
		public Timer(TimeKeeper timeKeeper) => 
			TimeKeeper = timeKeeper;


		/// <summary>
		/// Resets timer.
		/// </summary>
		public virtual void Reset()
		{
			Active = false;
			Counter = 0;
		}



		/// <summary>
		/// Updates timer.
		/// </summary>
		public virtual void Update()
		{
			if (Active)
			{
				if (AffectedBySpeedMultiplier)
				{
					if (TimeKeeper == null)
					{
						Counter += TimeKeeper.GlobalTime();
					}
					else
					{
						Counter += TimeKeeper.Time();
					}
				}
				else
				{
					Counter += GameMgr.ElapsedTime;
				}
			}
		}

	}
}
