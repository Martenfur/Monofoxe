using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Diagnostics;
using Monofoxe.Engine.Drawing;

namespace Monofoxe.Engine.Drawing
{
	public static class Fonts
	{
		//SpriteFont
		public static IFont TexFont = new TextureFont(Sprites.SpriteFont, " !" + '"' + @"#$%&'()*+,-./0123456789:;<=>?@ABCDEFGHIJKLMNOPQRSTUVWXYZ[\]^_`abcdefghijklmnopqrstuvwxyz{|}~", true);
	}
}
