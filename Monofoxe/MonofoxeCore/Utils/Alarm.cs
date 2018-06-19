using Monofoxe.Engine;

namespace Monofoxe.Utils
{
	/// <summary>
	/// Counts down seconds. Needs to be updated manually.
	/// </summary>
	public class Alarm
	{
		/// <summary>
		/// Tells how much time is left in seconds.
		/// </summary>
		protected double _counter;
		
		/// <summary>
		/// Alarm won't update if it's inactive.
		/// </summary>
		public bool Active = false;

		/// <summary>
		/// Tells if alarm was triggered.
		/// </summary>
		public bool Triggered {get; protected set;} = false;

		/// <summary>
		/// Tells if alarm is affected by GameCntrl.GameSpeedMultiplier.
		/// </summary>
		public bool AffectedBySpeedMultiplier = true;



		/// <summary>
		/// Sets alarm to given time.
		/// </summary>
		/// <param name="time">Time in seconds.</param>
		public void Set(double time)
		{
			Active = true;
			Triggered = false;
			_counter = time;
		}



		/// <summary>
		/// Resets alarm.
		/// </summary>
		public void Reset()
		{
			Active = false;
			Triggered = false;
			_counter = 0;
		}



		/// <summary>
		/// Updates alarm. Also can be used to check for triggering.
		/// </summary>
		public virtual bool Update()
		{
			Triggered = false;
			if (Active)
			{
				if (AffectedBySpeedMultiplier)
				{
					_counter -= GameCntrl.Time();
				}
				else
				{
					_counter -= GameCntrl.ElapsedTime;
				}
				
				if (_counter <= 0)
				{
					Triggered = true;
					Active = false;
				}
			}

			return Triggered;
		}
	}
}
