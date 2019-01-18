using System;

namespace Monofoxe.Engine.Utils
{
	// TODO: TEST!
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
		public Action<Alarm> TriggerAction;


		public Alarm() {}
		public Alarm(TimeKeeper timeKeeper, Action<Alarm> triggerAction) : base(timeKeeper) =>
			TriggerAction = triggerAction;
		

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
					TriggerAction?.Invoke(this);
					return true;
				}
			}

			return false;
		}
	}
}
