using Monofoxe.Engine;

namespace Monofoxe.Utils
{
	/// <summary>
	/// Counts seconds. Needs to be updated manually. Sets itself automatically after triggering.
	/// </summary>
	public class AutoAlarm : Alarm
	{
		public double Time;

		public AutoAlarm(double time)
		{
			time = Time;
			Set(Time);
		}

		/// <summary>
		/// Updates alarm. Also can be used to check for triggering.
		/// </summary>
		/// <returns></returns>	
		public override bool Update()
		{
			Triggered = false;
			if (Active)
			{
				_counter -= GameCntrl.Time();

				if (_counter <= 0)
				{
					Triggered = true;
					_counter += Time;
				}
			}

			return Triggered;
		}
	}
}
