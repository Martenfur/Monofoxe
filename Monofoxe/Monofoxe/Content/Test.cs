// Template tags: 
// Test - Name of current group.
// <sprite_name> - Name of each sprite.
// <sprite_hash_name> - Hash name of each sprite.

using Microsoft.Xna.Framework.Content;
using Monofoxe.Engine;
using Monofoxe.Engine.Drawing;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Sprites
{
	public static class Test
	{
		#region sprites
		public static Sprite Font;
		public static Sprite DemonFire;
		public static Sprite Happyded;
		public static Sprite Knight;
		public static Sprite Priest;
		public static Sprite BarnRoof;
		#endregion sprites
		
		private static string _groupName = "Test";
		private static ContentManager _content = new ContentManager(GameCntrl.Game.Services);
		
		public static bool Loaded = false;
		
		public static void Load()
		{
			var sprites = new Dictionary<string, Frame[]>();
			
			var i = 0;
			var graphicsPath = GameCntrl.ContentDir + '/' + GameCntrl.GraphicsDir +  '/' + _groupName + '_';
			
			Loaded = true;
			
			// Loading all atlasses.
			while(true)
			{
				try
				{
					var atlassSprites = _content.Load<Dictionary<string, Frame[]>>(graphicsPath + i);
					sprites = sprites.Concat(atlassSprites).ToDictionary(x => x.Key, x => x.Value);
				}
				catch(Exception) // If content file doesn't exist, this means we've loaded all atlasses.
				{
					break;
				}
				
				i += 1;
			}
			// Loading all atlasses.
			
			#region sprite_constructors
			
			Font = new Sprite(sprites["font"]);
			DemonFire = new Sprite(sprites["demon_fire"]);
			Happyded = new Sprite(sprites["Textures/happyded"]);
			Knight = new Sprite(sprites["Textures/knight"]);
			Priest = new Sprite(sprites["Textures/priest"]);
			BarnRoof = new Sprite(sprites["Textures/New/barn_roof"]);
			
			#endregion sprite_constructors
		}
		
		public static void Unload()
		{
			_content.Unload();
			Loaded = false;
		}
	}
}
