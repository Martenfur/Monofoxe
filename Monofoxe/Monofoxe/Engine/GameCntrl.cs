using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System.IO;
using Monofoxe.Engine.Drawing;
//using System.Xml;
using System.Diagnostics;
using System.Linq;

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
		public static string GraphicsDir = "Atlasses";

		/// <summary>
		/// Name of texture atlasses.
		/// </summary>
		public static string AtlassFileName = "texture";

		/// <summary>
		/// Name of the file where info about separate textures is stored. 
		/// </summary>
		public static string TextureInfoFileName = "textures_3D.txt";
		
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

		public static int Fps 
		{
			get => _fpsCounter.Value;
		}
		static FpsCounter _fpsCounter = new FpsCounter();

		
		public static int Tps 
		{
			get => _tpsCounter.Value;
		}
		static FpsCounter _tpsCounter = new FpsCounter();


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

			WindowManager = new WindowManager(game);
		}



		public static void Begin()
		{
			new TestObj();
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
			
			_tpsCounter.Update(gameTime);
			
			Input.Update();
			Objects.Update(gameTime);
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



		public static void LoadGraphics(ContentManager content)
		{
			Dictionary<string, Frame[]> atlasses = new Dictionary<string, Frame[]>();
			
			Texture2D atlass;
			var i = 0;
			string graphicsPath = GraphicsDir +  '/' + AtlassFileName + '_';
			
			// Loading all atlasses.
			while(true)
			{
				try
				{
					Debug.WriteLine("Loading " +  ContentDir + '/' + graphicsPath + i + ".json");
					//atlass = content.Load<Texture2D>(graphicsPath + i);
					//LoadFrames(atlasses, atlass, ContentDir + '/' + graphicsPath + i + ".json");
					var d2 = content.Load<Dictionary<string, Frame[]>>(graphicsPath + i);
					atlasses = atlasses.Concat(d2).ToDictionary(x=> x.Key, x=> x.Value);
					Debug.WriteLine(atlasses.Count);
				}
				catch(Exception) // If content file doesn't exist, this means we've loaded all atlasses.
				{
					break;
				}

				i += 1;
			}
			// Loading all atlasses.

			// Loading 3D textures.
			string path = ContentDir + '/' + GraphicsDir + '/' + TextureInfoFileName;
			if (File.Exists(path))
			{
				LoadTextures(atlasses, content, path);
			}
			// Loading 3D textures.

			Sprites.Init(atlasses);
		}


		


		/// <summary>
		/// Loads individual textures using provided text file and adds them to provided dictionary.
		/// </summary>
		/// <param name="dictionary">Dictionaty, to which frame arrays will be written.</param>
		/// <param name="content">Content, using which textures will be loaded.</param>
		/// <param name="txtPath">Path to text file with info about textures.</param>
		private static void LoadTextures(Dictionary<string, Frame[]> dictionary, ContentManager content, string txtPath)
		{
			// Algorhitm is almost the same as in LoadFrames().

			string[] lines = File.ReadAllLines(txtPath);
			
			int previousFrameId = -1;
			string previousFrameKey = "";

			List<Frame> frameList = new List<Frame>();


			foreach(string line in lines)
			{
				Texture2D tex = content.Load<Texture2D>(GraphicsDir + '/' + line);

				string filename = line;
				
				int frameIdPos = filename.LastIndexOf('_');
				int frameId = Int32.Parse(filename.Substring(frameIdPos + 1));
				
				string frameKey = filename.Remove(frameIdPos, filename.Length - frameIdPos);


				if (previousFrameKey.Length == 0)
				{
					previousFrameKey = frameKey;
				}

				Frame frame = new Frame(tex, new Rectangle(0, 0, tex.Width, tex.Height), Vector2.Zero, tex.Width, tex.Height);
				
				if (frameId <= previousFrameId)
				{
					if (frameList.Count > 0)
					{
						dictionary.Add(previousFrameKey, frameList.ToArray());
						
						Debug.WriteLine(previousFrameKey + ": " + frameList.Count);
						previousFrameKey = frameKey;

						frameList.Clear();
					}
				}
				
				previousFrameId = frameId;
				frameList.Add(frame);
			}

			if (frameList.Count > 0)
			{
				dictionary.Add(previousFrameKey, frameList.ToArray());
				frameList.Clear();
			}
		}


	}
}
