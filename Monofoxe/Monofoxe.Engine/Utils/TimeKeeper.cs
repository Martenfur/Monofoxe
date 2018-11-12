using Monofoxe.Engine;

namespace Monofoxe.Engine.Utils
{
	
	/// <summary>
	/// Calculates elapsed time based on multiplier.
	/// </summary>
	public class TimeKeeper
	{
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
		/// Returns GameMgr.ElapsedTime, multiplied by global time multiplier.
		/// </summary>
		public static double GlobalTime() => 
			GameMgr.ElapsedTime * _globalTimeMultiplier;
		
		/// <summary>
		/// Returns GameMgr.ElapsedTime, multiplied by global time multiplier.
		/// </summary>
		public static float GlobalTime(float val) => 
			val * (float)(GameMgr.ElapsedTime * _globalTimeMultiplier);
		
		/// <summary>
		/// Returns GameMgr.ElapsedTime, multiplied by global time multiplier.
		/// </summary>
		public static double GlobalTime(double val) => 
			val * GameMgr.ElapsedTime * _globalTimeMultiplier;
		
		
		/// <summary>
		/// Returns GameMgr.ElapsedTime, multiplied by both 
		/// global time multiplier and local time multiplier.
		/// </summary>
		public double Time() => 
			GameMgr.ElapsedTime * _timeMultiplier * _globalTimeMultiplier;
			
		/// <summary>
		/// Returns GameMgr.ElapsedTime, multiplied by both 
		/// global time multiplier and local time multiplier.
		/// </summary>
		public float Time(float val) => 
			val * (float)(GameMgr.ElapsedTime * _timeMultiplier * _globalTimeMultiplier);

		/// <summary>
		/// Returns GameMgr.ElapsedTime, multiplied by both 
		/// global time multiplier and local time multiplier.
		/// </summary>
		public double Time(double val) => 
			val * GameMgr.ElapsedTime * _timeMultiplier * _globalTimeMultiplier;
		
	}
}
