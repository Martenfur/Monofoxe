using Microsoft.Xna.Framework;
using System;
using Monofoxe.Engine.Audio;

namespace Monofoxe.Engine
{
	public static class GameCntrl
	{
		/// <summary>
		/// Root directory of the game content.
		/// </summary>
		public static string ContentDir = "Content";
		
		/// <summary>
		/// Root directory of the graphics.
		/// NOTE: This directory is located inside ContentDir.
		/// </summary>
		public static string GraphicsDir = "Graphics";

		/// <summary>
		/// Main Game class.
		/// </summary>
		public static Game Game;	
		
		/// <summary>
		/// Window manager. Can be used for screen and window stuff.
		/// </summary>
		public static WindowManager WindowManager;

		/// <summary>
		/// Time in seconds, elapsed since game start.
		/// </summary>
		public static double ElapsedTimeTotal {get; private set;}
		/// <summary>
		/// Time in seconds, elapsed since previous frame.
		/// </summary>
		public static double ElapsedTime {get; private set;}

		public static int Fps => _fpsCounter.Value;
		static FpsCounter _fpsCounter = new FpsCounter();

		public static double FixedUpdateRate = 0.5; // Seconds.
		
		/// <summary>
		/// If more than one, game will speed up.
		/// If less than one, game will slow down.
		/// </summary>
		public static double GameSpeedMultiplier
		{
			get => _gameSpeedMultiplier;
			
			set 
			{
				if (value > 0)
				{
					_gameSpeedMultiplier = value;
				}
			}
		}
		private static double _gameSpeedMultiplier = 1; 
		

		public static double MaxGameSpeed
		{
			get => (int)(1.0 / Game.TargetElapsedTime.TotalSeconds);

			set
			{
				if (value > 0)
				{
					Game.TargetElapsedTime = TimeSpan.FromTicks(10000000 / (long)value);
				}
			}
		}
		
		/// <summary>
		/// After this point game will slow down instead of skipping frames.
		/// </summary>
		public static double MinGameSpeed 
		{
			get => _minGameSpeed;

			set
			{
				if (value > 0)
				{
					_minGameSpeed = value;
				}
			}
		}
		private static double _minGameSpeed = 30;


		
		/// <summary>
		/// Counts frames per second.
		/// </summary>
		class FpsCounter
		{
			public int Value;

			private int _fpsCount;
			private double _fpsAddition;
			
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

		
		public static void Init(Game game)
		{
			Game = game;
			game.IsMouseVisible = true;
			
			game.Window.TextInput += Input.TextInput;
			Input.MaxGamepadCount = 2;

			WindowManager = new WindowManager(game);

			AudioMgr.Init();
		}

		
		public static void Update(GameTime gameTime)
		{
			// Elapsed time counters.
			ElapsedTimeTotal = gameTime.TotalGameTime.TotalSeconds;

			if (1.0 / MinGameSpeed >= gameTime.ElapsedGameTime.TotalSeconds)
			{
				ElapsedTime = gameTime.ElapsedGameTime.TotalSeconds;
			}
			else
			{
				ElapsedTime = 1.0 / MinGameSpeed;
			}
			// Elapsed time counters.
			
			Input.Update();
			Objects.Update(gameTime);
			AudioMgr.Update();
		}

		
		
		public static void UpdateFps(GameTime gameTime) => 
			_fpsCounter.Update(gameTime);



		public static double Time(double val = 1) => 
			val * ElapsedTime * GameSpeedMultiplier;



		/// <summary>
		/// Closes the game.
		/// </summary>
		public static void ExitGame() => 
			Game.Exit();

	}
}
