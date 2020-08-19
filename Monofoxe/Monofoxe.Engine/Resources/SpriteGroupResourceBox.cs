using Microsoft.Xna.Framework.Content;
using Monofoxe.Engine;
using Monofoxe.Engine.Drawing;
using Monofoxe.Engine.Resources;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;

namespace Monofoxe.Resources
{
  public class SpriteGroupResourceBox : ResourceBox<Sprite>
	{
		
		private ContentManager _content = new ContentManager(GameMgr.Game.Services);

		/// <summary>
		/// Used when file name begins with a char, not allowed in variable names.
		/// </summary>
		private static string _paddingStr = "S";

		private readonly string _resourcePath;

		public SpriteGroupResourceBox(string name, string spriteGroupPath) : base(name)
		{
			_resourcePath = spriteGroupPath;
		}

		public override void Load()
		{
			if (Loaded)
			{
				return;
			}
			Loaded = true;
			var graphicsPath = Path.Combine(ResourceInfoMgr.ContentDir, _resourcePath);
			var sprites = _content.Load<Dictionary<string, Sprite>>(graphicsPath);

			foreach (var spritePair in sprites)
			{
				AddResource(Path.GetFileNameWithoutExtension(spritePair.Key), spritePair.Value);
			}

		}

		public override void Unload()
		{
			if (!Loaded)
			{
				return;
			}
			Loaded = false;
			_content.Unload();
		}

		/// <summary>
		/// Converts regular string to compiler-accepted CamelCase variable name.
		/// some_stuff => SomeStuff
		/// </summary>
		public static string ToCamelCase(string str)
		{
			// Removing prohibited symbols from string.
			var rgx = new Regex("[^a-zA-Z0-9 _]");
			str = rgx.Replace(str, "");

			if (str.Length == 0)
			{
				return "";
			}
			// Removing prohibited symbols from string.

			var words = str.Split(new char[] { '_', ' ' });
			var upper = new StringBuilder();

			if (char.IsDigit(str[0])) // Variable name cannot begin with a digit.
			{
				upper.Append(_paddingStr);
			}
			if (str[0] == '_') // For variables which begin with underscore.
			{
				upper.Append('_');
			}

			// Making each word begin with an uppercase letter.
			foreach (var word in words)
			{
				if (word.Length > 0)
				{
					upper.Append(char.ToUpper(word[0]) + word.Substring(1, word.Length - 1));
				}
			}
			// Making each word begin with an uppercase letter.

			return upper.ToString();
		}
	}
}
