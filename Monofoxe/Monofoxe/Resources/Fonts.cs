using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace Monofoxe.Engine.Drawing
{
	public static class Fonts
	{
		private static ContentManager _content;
		
		static string Ascii = " !" + '"' + @"#$%&'()*+,-./0123456789:;<=>?@ABCDEFGHIJKLMNOPQRSTUVWXYZ[\]^_`abcdefghijklmnopqrstuvwxyz{|}~";
		
		public static IFont TexFont;
		public static IFont AnotherFont;
		public static IFont AnotherFont1;
		public static IFont Def;

		public static void Load(ContentManager content)
		{
			_content = new ContentManager(content.ServiceProvider);
			_content.RootDirectory = content.RootDirectory;
			
			TexFont = new TextureFont(Sprites.Default.SpriteFont, 3, 3, Ascii, false);
			AnotherFont = new TextureFont(Sprites.Default.AnotherFont, 1, 1, Ascii, false);
			AnotherFont1 = new TextureFont(Sprites.Default.AnotherFont, 1, 1, Ascii, true);
			
			Def = new Font(_content.Load<SpriteFont>("def"));
		}

		public static void Unload()
		{
			_content.Unload();
		}

	}
}
