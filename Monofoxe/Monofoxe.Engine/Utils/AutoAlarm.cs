using System;

namespace Monofoxe.Engine.Utils
{
	/// <summary>
	/// Counts down seconds. Needs to be updated manually. Sets itself automatically after triggering.
	/// </summary>
	public class AutoAlarm : Alarm
	{
		public double Time;

		public AutoAlarm(double time)
		{
			Time = time;
			Set(Time);
		}

		public AutoAlarm(double time, TimeKeeper timeKeeper) : base(timeKeeper)
		{
			Time = time;
			Set(Time);
		}


		/// <summary>
		/// Updates alarm. Returns true, if alarm is being triggered.
		/// </summary>
		public override bool Update()
		{
			if (base.Update())
			{
				Counter += Time;
				return true;
			}
			return false;
		}
	}
}
