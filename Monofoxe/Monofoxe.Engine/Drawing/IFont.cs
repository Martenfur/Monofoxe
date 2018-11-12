using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;


namespace Monofoxe.Engine.Drawing
{
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
