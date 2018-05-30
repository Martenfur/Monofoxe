#class_prefix = "public static Sprite <sprite_name>;"
#class_constructor = "<sprite_name> = new Sprite(sprites[<hash_sprite_name>], <args>);"

using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework.Content;
using Monofoxe.Engine;

namespace Monofoxe.Engine.Drawing
{
	public static class <class_name>
	{
		#region sprites
		<class_prefix>		
		#endregion sprites
		
		private static string _groupName = "<group_name>";
		private static ContentManager = new ContentManager(GameCntrl.Game.Services);
		
		public static void Load()
		{
			var sprites = new Dictionary<string, Frame[]>();
			
			var i = 0;
			string graphicsPath = GraphicsDir +  '/' + _groupName + '_';
			
			// Loading all atlasses.
			while(true)
			{
				try
				{
					var atlassSprites = content.Load<Dictionary<string, Frame[]>>(graphicsPath + i);
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
	}
}