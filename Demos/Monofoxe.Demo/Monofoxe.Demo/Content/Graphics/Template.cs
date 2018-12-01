#class_prefix = "public static Sprite <sprite_name>;"
#class_constructor = "<sprite_name> = sprites[<hash_sprite_name>];"
// Template tags: 
// <class_name> - Name of output class.
// <group_name> - Name of current group.
// <sprite_name> - Name of each sprite.
// <sprite_hash_name> - Hash name of each sprite.

using Microsoft.Xna.Framework.Content;
using Monofoxe.Engine;
using Monofoxe.Engine.Drawing;
using System.Collections.Generic;

namespace Resources.Sprites
{
	public static class <class_name>
	{
		#region Sprites.
		<class_prefix>		
		#endregion Sprites.
		
		private static string _groupName = "<group_name>";
		private static ContentManager _content = new ContentManager(GameMgr.Game.Services);
		
		public static bool Loaded = false;
		
		public static void Load()
		{
			Loaded = true;	
			var graphicsPath = AssetMgr.ContentDir + '/' + AssetMgr.GraphicsDir +  '/' + _groupName;
			var sprites = _content.Load<Dictionary<string, Sprite>>(graphicsPath);
					
			#region Sprite constructors.

			<class_constructor>

			#endregion Sprite constructors.
		}
		
		public static void Unload()
		{
			_content.Unload();
			Loaded = false;
		}
	}
}