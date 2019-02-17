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
		
		internal static double _elapsedTime;


		/// <summary>
		/// If more than one, time will speed up.
		/// If less than one, time will slow down.
		/// </summary>
		public static double GlobalTimeMultiplier
		{
			get => _globalTimeMultiplier;
			
			set 
			{
				if (value > 0)
				{
					_globalTimeMultiplier = value;
				}
			}
		}
		private static double _globalTimeMultiplier = 1; 
		

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
		public static double GlobalTime() => 
			_elapsedTime * _globalTimeMultiplier;
	

		/// <summary>
		/// Returns elapsed time, multiplied by global time multiplier.
		/// 
		/// Use this for constant speeds.
		/// </summary>
		public static double GlobalTime(double val) => 
			val * _elapsedTime * _globalTimeMultiplier;
		
		/// <summary>
		/// Returns elapsed time, multiplied by global time multiplier 
		/// and time-corrected for acceleration.
		/// 
		/// Use this for accelerating values.
		/// </summary>
		public static double GlobalTime(double speed, double acceleration) => 
			(speed + GlobalTime(Math.Abs(acceleration) * 0.5)) * _elapsedTime * _globalTimeMultiplier;
		
		
		
		/// <summary>
		/// Returns elapsed time, multiplied by global time multiplier.
		/// </summary>
		public static float GlobalTime(float val) => 
			(float)GlobalTime((double)val);
		
		/// <summary>
		/// Returns elapsed time, multiplied by global time multiplier 
		/// and time-corrected for acceleration.
		/// 
		/// Use this for accelerating values.
		/// </summary>
		public static float GlobalTime(float speed, float acceleration) => 
			(float)GlobalTime((double)speed, acceleration);



		/// <summary>
		/// Returns elapsed time, multiplied by global time multiplier.
		/// 
		/// Use this for constant speeds.
		/// </summary>
		public static Vector2 GlobalTime(Vector2 val) => 
				new Vector2(GlobalTime(val.X), GlobalTime(val.Y));
	
		/// <summary>
		/// Returns elapsed time, multiplied by global time multiplier 
		/// and time-corrected for acceleration.
		/// 
		/// Use this for accelerating values.
		/// </summary>
		public static Vector2 GlobalTime(Vector2 speed, Vector2 acceleration) => 
			new Vector2(GlobalTime(speed.X, acceleration.X), GlobalTime(speed.Y, acceleration.Y));
		
		
		
		/// <summary>
		/// Returns elapsed time, multiplied by both 
		/// global time multiplier and local time multiplier.
		/// </summary>
		public double Time() => 
			GlobalTime() * _timeMultiplier;
			

		/// <summary>
		/// Returns elapsed time, multiplied by both 
		/// global time multiplier and local time multiplier.
		/// </summary>
		public float Time(float val) => 
			GlobalTime(val) * (float)_timeMultiplier;

		/// <summary>
		/// Returns elapsed time, multiplied by global time multiplier 
		/// and time-corrected for acceleration.
		/// 
		/// Use this for accelerating values.
		/// </summary>
		public float Time(float speed, float acceleration) => 
			(float)GlobalTime(speed * _timeMultiplier, acceleration * _timeMultiplier);


		/// <summary>
		/// Returns elapsed time, multiplied by both 
		/// global time multiplier and local time multiplier.
		/// </summary>
		public double Time(double val) => 
			GlobalTime(val) * _timeMultiplier;

		/// <summary>
		/// Returns elapsed time, multiplied by global time multiplier, 
		/// local time multiplier and time-corrected for acceleration.
		/// 
		/// Use this for accelerating values.
		/// </summary>
		public double Time(double speed, double acceleration) => 
			GlobalTime(speed * _timeMultiplier, acceleration * _timeMultiplier);

		
		/// <summary>
		/// Returns elapsed time, multiplied by both 
		/// global time multiplier and local time multiplier.
		/// 
		/// Use this for constant speeds.
		/// </summary>
		public Vector2 Time(Vector2 val) => 
				new Vector2(GlobalTime(val.X), GlobalTime(val.Y)) * (float)_timeMultiplier;
	
		/// <summary>
		/// Returns elapsed time, multiplied by global time multiplier, 
		/// local time multiplier and time-corrected for acceleration.
		/// 
		/// Use this for accelerating values.
		/// </summary>
		public Vector2 Time(Vector2 speed, Vector2 acceleration) => 
			new Vector2(
				(float)GlobalTime(speed.X * _timeMultiplier, acceleration.X * _timeMultiplier), 
				(float)GlobalTime(speed.Y * _timeMultiplier, acceleration.Y * _timeMultiplier)
			);
		
	}
}
