#class_prefix = "public static Sprite <sprite_name>;"
#class_constructor = "<sprite_name> = sprites[<hash_sprite_name>];"
// Template tags: 
// <group_name> - Name of current group.
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
			var i = 0;
			var graphicsPath = GameCntrl.ContentDir + '/' + GameCntrl.GraphicsDir +  '/' + _groupName;
			
			Loaded = true;	
		
			var sprites = _content.Load<Dictionary<string, Sprite>>(graphicsPath + i);
					
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