using System;
using Microsoft.Xna.Framework;

namespace Monofoxe.Engine.Utils
{
	/// <summary>
	/// Calculates elapsed time based on multiplier.
	/// Should be used for fps adjusting and slowing down\speeding up time.
	/// </summary>
	public class TimeKeeper
	{

		public static readonly TimeKeeper Global = new TimeKeeper();

		internal static double _elapsedTime;


		/// <summary>
		/// If more than one, time will speed up.
		/// If less than one, time will slow down.
		/// </summary>
		public double TimeMultiplier
		{
			get => _timeMultiplier;
			
			set 
			{
				if (value > 0)
				{
					_timeMultiplier = value;
				}
			}
		}
		private double _timeMultiplier = 1; 
		

		/// <summary>
		/// Returns elapsed time, multiplied by global time multiplier.
		/// </summary>
		public double Time() => 
			_elapsedTime * _timeMultiplier;
	

		/// <summary>
		/// Returns elapsed time, multiplied by global time multiplier.
		/// 
		/// Use this for constant speeds.
		/// </summary>
		public double Time(double val) => 
			val * _elapsedTime * _timeMultiplier;
		
		/// <summary>
		/// Returns elapsed time, multiplied by global time multiplier 
		/// and time-corrected for acceleration.
		/// 
		/// Use this for accelerating values.
		/// </summary>
		public double Time(double speed, double acceleration) => 
			(speed + Time(Math.Abs(acceleration) * 0.5)) * _elapsedTime * _timeMultiplier;
		
		
		
		/// <summary>
		/// Returns elapsed time, multiplied by global time multiplier.
		/// </summary>
		public float Time(float val) => 
			(float)Time((double)val);
		
		/// <summary>
		/// Returns elapsed time, multiplied by global time multiplier 
		/// and time-corrected for acceleration.
		/// 
		/// Use this for accelerating values.
		/// </summary>
		public float Time(float speed, float acceleration) => 
			(float)Time((double)speed, acceleration);



		/// <summary>
		/// Returns elapsed time, multiplied by global time multiplier.
		/// 
		/// Use this for constant speeds.
		/// </summary>
		public Vector2 Time(Vector2 val) => 
				new Vector2(Time(val.X), Time(val.Y));
	
		/// <summary>
		/// Returns elapsed time, multiplied by global time multiplier 
		/// and time-corrected for acceleration.
		/// 
		/// Use this for accelerating values.
		/// </summary>
		public Vector2 Time(Vector2 speed, Vector2 acceleration) => 
			new Vector2(Time(speed.X, acceleration.X), Time(speed.Y, acceleration.Y));
		
	}
}
