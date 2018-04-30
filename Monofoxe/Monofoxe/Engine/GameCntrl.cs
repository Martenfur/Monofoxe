using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System.IO;
using Monofoxe.Engine.Drawing;
using System.Xml;
using System.Diagnostics;


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
		public static double ElapsedTimeTotal {get; private set;} = 0;
		/// <summary>
		/// Time in seconds, elapsed since previous frame.
		/// </summary>
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
			{return (int)(1.0 / Game.TargetElapsedTime.TotalSeconds);}

			set
			{
				if (value > 0)
				{Game.TargetElapsedTime = TimeSpan.FromTicks(10000000 / (long)value);}
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

		
		
		public static void UpdateFps(GameTime gameTime)
		{
			_fpsCounter.Update(gameTime);
		}



		public static double Time(double val = 1)
		{
			return val * ElapsedTime * GameSpeedMultiplier;
		}



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
					Debug.WriteLine("Loading " +  ContentDir + '/' + graphicsPath + i + ".xml");
					atlass = content.Load<Texture2D>(graphicsPath + i);
					LoadFrames(atlasses, atlass, ContentDir + '/' + graphicsPath + i + ".xml");
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
		/// Creates a Dictionary of Frame arrays using provided texture atlass and XML document.
		/// </summary>
		/// <param name="dictionary">Dictionaty, to which frame arrays will be written.</param>
		/// <param name="atlass">Texture atlass.</param>
		/// <param name="xmlPath">Path to XML document which contains info about sprites in the atlass.</param>
		public static void LoadFrames(Dictionary<string, Frame[]> dictionary, Texture2D atlass, string xmlPath)
		{
			// Parsing XML.
			XmlDocument xml = new XmlDocument();
			xml.Load(xmlPath);
			
			XmlNodeList nodes = xml.DocumentElement.SelectNodes("sprite");
			// Parsing XML.

			int previousFrameId = -1;
			string previousFrameKey = "";

			List<Frame> frameList = new List<Frame>();

			foreach(XmlNode node in nodes)
			{
				string filename = node.Attributes["n"].Value;
				
				int frameIdPos = filename.LastIndexOf('_');
				int frameId = Int32.Parse(filename.Substring(frameIdPos + 1));
				
				string frameKey = filename.Remove(frameIdPos, filename.Length - frameIdPos);

				if (previousFrameKey.Length == 0)
				{
					previousFrameKey = frameKey;
				}

				Vector2 origin;
				if (node.Attributes["oX"] != null) // There may not be an origin field.
				{
					origin = new Vector2(Int32.Parse(node.Attributes["oX"].Value), Int32.Parse(node.Attributes["oY"].Value));
				}
				else
				{
					origin = Vector2.Zero;
				}
				
				int frameW, frameH;

				if (node.Attributes["oW"] != null) // For sprites with transparent cut-outs. 
				{
					frameW = Int32.Parse(node.Attributes["oW"].Value);
					frameH = Int32.Parse(node.Attributes["oH"].Value);
				}
				else
				{
					frameW = Int32.Parse(node.Attributes["w"].Value);
					frameH = Int32.Parse(node.Attributes["h"].Value);
				}


				Frame frame = new Frame(
					atlass, 
					new Rectangle(
						Int32.Parse(node.Attributes["x"].Value),
						Int32.Parse(node.Attributes["y"].Value),
						Int32.Parse(node.Attributes["w"].Value),
						Int32.Parse(node.Attributes["h"].Value)
					),
					origin,
					frameW, 
					frameH
				);
				
				// If current frame index is lesser than previous, we got new sprite sheet.
				if (frameId <= previousFrameId && frameList.Count > 0) 
				{			
					// Adding frame array to dictionary with corresponding key and clearing buffer list.
					dictionary.Add(previousFrameKey, frameList.ToArray());
					previousFrameKey = frameKey;
					frameList.Clear();
					// Adding frame array to dictionary with corresponding key and clearing buffer list.
				}
				
				previousFrameId = frameId;
				frameList.Add(frame);
			}

			if (frameList.Count > 0) // If there are any frames left -- we need them too.
			{
				dictionary.Add(previousFrameKey, frameList.ToArray());
			}
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
