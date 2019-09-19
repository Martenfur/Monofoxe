using System;

namespace Monofoxe.Engine.Utils
{
	public delegate void AlarmDelegate(Alarm caller);

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
		/// Gets called in an update, if alarm is triggered. 
		/// </summary>
		public event AlarmDelegate TriggerEvent;

		/// <summary>
		/// Tells, if alarm is running right now.
		/// </summary>
		public bool Running => Counter > 0;
		
		public Alarm(TimeKeeper timeKeeper = null) : base(timeKeeper) {}
			
		
		/// <summary>
		/// Sets alarm to given time.
		/// </summary>
		/// <param name="time">Time in seconds.</param>
		public void Set(double time)
		{
			Enabled = true;
			Counter = time;
		}



		/// <summary>
		/// Resets alarm.
		/// </summary>
		public override void Reset()
		{
			Enabled = false;
			Counter = 0;
		}



		/// <summary>
		/// Updates alarm. Returns true, if alarm is being triggered.
		/// </summary>
		public new virtual bool Update()
		{
			if (Enabled && Counter > 0)
			{
				
				if (TimeKeeper == null)
				{
					Counter -= TimeKeeper.GlobalTime();
				}
				else
				{
					Counter -= TimeKeeper.Time();
				}		
				
				if (Counter <= 0)
				{
					TriggerEvent?.Invoke(this);
					return true;
				}
			}

			return false;
		}
	}
}
