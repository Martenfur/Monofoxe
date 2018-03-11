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

		public static int Fps 
		{
			get
			{
				return _fpsCounter.Value;
			}
		}
		static FpsCounter _fpsCounter = new FpsCounter();

		
		public static int Tps 
		{
			get
			{
				return _tpsCounter.Value;
			}
		}
		static FpsCounter _tpsCounter = new FpsCounter();


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


		
		/// <summary>
		/// Counts frames per second.
		/// </summary>
		class FpsCounter
		{
			public int Value = 0;

			private int _fpsCount = 0;
			private double _fpsAddition = 0;
			
			public void Update(GameTime gameTime)
			{
				_fpsAddition += gameTime.ElapsedGameTime.TotalSeconds;
				_fpsCount += 1;

				if (_fpsAddition >= 1) // Every second value updates and counters reset.
				{
					Value = _fpsCount;
					_fpsAddition -= 1;
					_fpsCount = 0;
				}
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
			
			_tpsCounter.Update(gameTime);
			
			Input.Update();
			Objects.Update(gameTime);

		}

		
		
		public static void UpdateFps(GameTime gameTime)
		{
			_fpsCounter.Update(gameTime);
		}



		public static double Time(double val)
		{return val * ElapsedTime * GameSpeedMultiplier;}

	}
}
