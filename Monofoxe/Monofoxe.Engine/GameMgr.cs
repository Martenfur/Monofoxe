using System;
using System.Collections.Generic;
using System.Reflection;
using Microsoft.Xna.Framework;
using Monofoxe.Engine.ECS;
using Monofoxe.Engine.SceneSystem;
using Monofoxe.Engine.Drawing;

namespace Monofoxe.Engine
{
	public static class GameMgr
	{
		
		/// <summary>
		/// Main Game class.
		/// </summary>
		public static Game Game;	
		
		/// <summary>
		/// Window manager. Can be used for screen and window stuff.
		/// </summary>
		public static WindowMgr WindowManager;

		/// <summary>
		/// Time in seconds, elapsed since game start.
		/// </summary>
		public static double ElapsedTimeTotal {get; private set;}
		/// <summary>
		/// Time in seconds, elapsed since previous frame.
		/// </summary>
		public static double ElapsedTime {get; private set;}

		
		public static double FixedUpdateRate = 0.5; // Seconds.
		
		
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
				else
				{
					throw new InvalidOperationException("Game speed cannot be less than 1!");
				}
			}
		}
		private static double _minGameSpeed = 30;


		/// <summary>
		/// All game's assemblies, including ones from libraries.
		/// </summary>
		public static Dictionary<string, Assembly> Assemblies;
		
		/// <summary>
		/// All of game's types.
		/// </summary>
		public static Dictionary<string, Type> Types;
		
		public static int Fps {get; private set;}
		private static int _fpsCount;
		private static double _fpsAddition;

		
		public static void Init(Game game)
		{
			Game = game;
			Game.IsMouseVisible = true;
			
			Input.MaxGamepadCount = 2;
			
			WindowManager = new WindowMgr(game);

			LoadAssembliesAndTypes(game.GetType().Assembly);
			
			AssetMgr.Init();

			var defScene = SceneMgr.CreateScene("default");
			defScene.CreateLayer("default");

			EntityTemplatePool.InitTemplatePool();
		}

		
		/// <summary>
		/// Performs update-related routines and calls Update events for entities and systems.
		/// </summary>
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

			SceneMgr.PreUpdateRoutine();
			SceneMgr.CallFixedUpdateEvents(gameTime);
			SceneMgr.CallUpdateEvents(gameTime);
			SceneMgr.PostUpdateRoutine();
		}


		
		/// <summary>
		/// Performs drawing-related routines and calls Draw events for entities and systems.
		/// </summary>
		public static void Draw(GameTime gameTime)
		{
			_fpsAddition += gameTime.ElapsedGameTime.TotalSeconds;
			_fpsCount += 1;

			if (_fpsAddition >= 1) // Every second value updates and counters reset.
			{
				Fps = _fpsCount;
				_fpsAddition -= 1;
				_fpsCount = 0;
			}

			GraphicsMgr.Update(gameTime);
		}
		


		/// <summary>
		/// Closes the game.
		/// </summary>
		public static void ExitGame() => 
			Game.Exit();


		
		#region Assembly loading.
		
		/// <summary>
		/// Loads all assemblies and extracts types form them.
		/// </summary>
		private static void LoadAssembliesAndTypes(Assembly entryAssembly)
		{
			// Loading all assemblies.
			Assemblies = new Dictionary<string, Assembly>();

			foreach(var asm in AppDomain.CurrentDomain.GetAssemblies())
			{
				Assemblies.Add(asm.FullName, asm);
			}

			LoadAllReferencedAssemblies(entryAssembly, 0);
			// Loading all assemblies.


			// Extracting all types from assemblies.
			Types = new Dictionary<string, Type>();

			foreach(var asm in Assemblies)
			{
				foreach(var type in asm.Value.GetTypes())
				{
					if (!Types.ContainsKey(type.FullName))
					{
						Types.Add(type.FullName, type);
					}
				}
			}
			// Extracting all types from assemblies.
		}
		
		/// <summary>
		/// Loads all referenced assemblies of an assembly.
		/// </summary>
		private static void LoadAllReferencedAssemblies(Assembly assembly, int level)
		{
			if (level > 128) // Safety check. I must be sure, engine won't do stack overflow at random.
			{
				return;
			}

			foreach(var refAssembly in assembly.GetReferencedAssemblies())
			{
				if (!Assemblies.ContainsKey(refAssembly.FullName))
				{
					var asm = Assembly.Load(refAssembly);
					Assemblies.Add(refAssembly.FullName, asm);
					
					LoadAllReferencedAssemblies(asm, level + 1);
				}
			}
		}

		#endregion Assembly loading.

	}
}
