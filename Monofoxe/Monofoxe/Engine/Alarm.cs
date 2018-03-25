using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Monofoxe.Engine
{
	public class Alarm
	{
		/// <summary>
		/// Tells how much time is left in seconds.
		/// </summary>
		private double _counter;
		
		/// <summary>
		/// Alarm won't update if it's inactive.
		/// </summary>
		public bool Active = false;

		/// <summary>
		/// Tells if alarm was triggered.
		/// </summary>
		public bool Triggered = false;

		/// <summary>
		/// Sets alarm to given time.
		/// </summary>
		/// <param name="time">Time in seconds.</param>
		public void Set(float time)
		{
			Active = true;
			_counter = time;
		}

		/// <summary>
		/// Resets alarm.
		/// </summary>
		public void Reset()
		{
			Active = false;
			_counter = 0;
		}

		/// <summary>
		/// Updates alarm. Also can be used to check for triggering.
		/// </summary>
		/// <returns></returns>
		public bool Update()
		{
			Triggered = false;
			if (Active)
			{
				_counter -= GameCntrl.ElapsedTime;

				if (_counter <= 0)
				{
					Triggered = true;
					Active = false;
					_counter = 0;
				}
			}

			return Triggered;
		}
	}
}
