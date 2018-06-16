#class_prefix = "public static Sprite <sprite_name>;"
#class_constructor = "<sprite_name> = new Sprite(sprites[<hash_sprite_name>], <args>);"
//Template: <group_name> is a built-in variable.

using Microsoft.Xna.Framework.Content;
using Monofoxe.Engine;
using Monofoxe.Engine.Drawing;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Sprites
{
	public static class <group_name>
	{
		#region sprites
		<class_prefix>		
		#endregion sprites
		
		private static string _groupName = "<group_name>";
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

			<class_constructor>

			#endregion sprite_constructors
		}
		
		public static void Unload()
		{
			_content.Unload();
			Loaded = false;
		}
	}
}