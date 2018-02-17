using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;


namespace Monofoxe.Engine
{
	public static class GameCntrl
	{
		public static Game MyGame = null;	

		public static double ElapsedTimeTotal {get; private set;} = 0;
		public static double ElapsedTime {get; private set;} = 0;

		public static double Fps {get; private set;} = 0;
		private static int _fpsCounter = 0;
		private static double _fpsAddition = 0;

		public static double FixedUpdateRate = 0.5; // Seconds.
		
		private static double _gameSpeedMultiplier = 1; 
		public static double GameSpeedMultiplier 
		{
			get 
			{return _gameSpeedMultiplier;}
			
			set 
			{
				if (value > 0)
				{_gameSpeedMultiplier = value;}
			}
		}
		
		public static double MaxGameSpeed
		{
			get
			{return (int)(1.0 / MyGame.TargetElapsedTime.TotalSeconds);}

			set
			{
				if (value > 0)
				{MyGame.TargetElapsedTime = TimeSpan.FromTicks(10000000 / (long)value);}
			}
		}
		
		private static double _minGameSpeed = 30;
		public static double MinGameSpeed // After this point game will slow down instead of skipping frames.
		{
			get
			{return _minGameSpeed;}

			set
			{
				if (value > 0)
				{_minGameSpeed = value;}
			}
		}
		

		public static void Update(GameTime gameTime)
		{
			
			// Elapsed time counters.
			ElapsedTimeTotal = gameTime.TotalGameTime.TotalSeconds;

			if (1.0 / MinGameSpeed >= gameTime.ElapsedGameTime.TotalSeconds)
			{ElapsedTime = gameTime.ElapsedGameTime.TotalSeconds;}
			else
			{ElapsedTime = 1.0 / MinGameSpeed;}
			// Elapsed time counters.
		
			// Fps counter.
			_fpsAddition += ElapsedTime;
			_fpsCounter += 1;

			if (_fpsAddition >= 1)
			{
				Fps = _fpsCounter;
				_fpsAddition -= 1;
				_fpsCounter = 0;
			}
			// Fps counter.

			
			Input.Update();
			Objects.Update(gameTime);

		}

		public static double Time(double val)
		{return val * ElapsedTime * GameSpeedMultiplier;}

	}
}
