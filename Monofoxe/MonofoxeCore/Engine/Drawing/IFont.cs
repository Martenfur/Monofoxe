using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;


namespace Monofoxe.Engine.Drawing
{
	public enum TextAlign
	{	
		Left = 0,
		Right = 2,
		Top = 0,
		Bottom = 2,
		Center = 1	
	}

	public interface IFont
	{
		Texture2D Texture {get;}
		ReadOnlyCollection<char> Characters {get;}
		char? DefaultCharacter {get; set;}
		int LineSpacing {get; set;}
		float Spacing {get; set;}

		Dictionary<char, SpriteFont.Glyph> GetGlyphs();
		Vector2 MeasureString(string text);
		Vector2 MeasureString(StringBuilder text);
		float MeasureStringWidth(string text);
		float MeasureStringWidth(StringBuilder text);
		float MeasureStringHeight(string text);
		float MeasureStringHeight(StringBuilder text);


		void Draw(SpriteBatch batch, string text, Vector2 pos, TextAlign halign, TextAlign valign);
	}
}
