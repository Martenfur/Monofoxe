#class_prefix = "public static Sprite <sprite_name>;"
#class_constructor = "<sprite_name> = new Sprite(frames[<hash_sprite_name>], <args>);"

using System;
using System.Collections.Generic;
using Monofoxe.Engine.Drawing;

namespace Monofoxe.Engine.Drawing
{
	public static class Sprites
	{
		#region sprites
		<class_prefix>		
		#endregion sprites
		
		public static void Init(Dictionary<string, Frame[]> frames)
		{	
			#region sprite_constructors

			<class_constructor>

			#endregion sprite_constructors
		}
	}
}