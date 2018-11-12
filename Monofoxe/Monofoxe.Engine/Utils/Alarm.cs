using Monofoxe.Engine;

namespace Monofoxe.Engine.Utils
{
	/// <summary>
	/// Counts down seconds. Needs to be updated manually.
	/// </summary>
	public class Alarm : Timer
	{
		/// <summary>
		/// Tells how much time is left in seconds.
		/// </summary>
		public new double Counter;
		
		/// <summary>
		/// Tells if alarm was triggered.
		/// </summary>
		public bool Triggered {get; protected set;} = false;

		

		public Alarm() {}
		public Alarm(TimeKeeper timeKeeper) : base(timeKeeper) {}


		/// <summary>
		/// Sets alarm to given time.
		/// </summary>
		/// <param name="time">Time in seconds.</param>
		public void Set(double time)
		{
			Active = true;
			Triggered = false;
			Counter = time;
		}



		/// <summary>
		/// Resets alarm.
		/// </summary>
		public override void Reset()
		{
			Active = false;
			Triggered = false;
			Counter = 0;
		}



		/// <summary>
		/// Updates alarm. Also can be used to check for triggering.
		/// </summary>
		public new virtual bool Update()
		{
			Triggered = false;
			if (Active)
			{
				if (AffectedBySpeedMultiplier)
				{
					if (TimeKeeper == null)
					{
						Counter -= TimeKeeper.GlobalTime();
					}
					else
					{
						Counter -= TimeKeeper.Time();
					}
				}
				else
				{
					Counter -= GameMgr.ElapsedTime;
				}
				
				if (Counter <= 0)
				{
					Triggered = true;
					Active = false;
				}
			}

			return Triggered;
		}
	}
}
