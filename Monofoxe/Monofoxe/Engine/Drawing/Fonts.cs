using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Monofoxe.Engine.Drawing
{
	public static class Fonts
	{
		static string ascii = " !" + '"' + @"#$%&'()*+,-./0123456789:;<=>?@ABCDEFGHIJKLMNOPQRSTUVWXYZ[\]^_`abcdefghijklmnopqrstuvwxyz{|}~";
		
		public static IFont TexFont = new TextureFont(Sprites.SpriteFont, 3, 3, ascii, false);
		public static IFont AnotherFont = new TextureFont(Sprites.AnotherFont, 1, 1, ascii, false);

		public static IFont AnotherFont1 = new TextureFont(Sprites.AnotherFont, 1, 1, ascii, true);
	}
}
