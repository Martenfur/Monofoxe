using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace Monofoxe.Engine.Drawing
{
	/// <summary>
	/// Wrapper of SpriteFont.
	/// </summary>
	public class Font : IFont
	{
		#region fields

		public Texture2D Texture 
		{
			get
			{
				return _spriteFont.Texture;
			}
		}
		
		public ReadOnlyCollection<char> Characters 
		{
			get
			{
				return _spriteFont.Characters;
			}
		}
		
		public char? DefaultCharacter 
		{
			get
			{
				return _spriteFont.DefaultCharacter;
			}

			set
			{
				_spriteFont.DefaultCharacter = value;
			}
		}
		
		public int LineSpacing 
		{
			get
			{
				return _spriteFont.LineSpacing;
			}

			set
			{
				_spriteFont.LineSpacing = value;
			}
		}
		
		public float Spacing
		{
			get
			{
				return _spriteFont.Spacing;
			}

			set
			{
				_spriteFont.Spacing = value;
			}
		}

		#endregion fields
		
		private SpriteFont _spriteFont;

		public Font(SpriteFont spriteFont)
		{
			_spriteFont = spriteFont;
		}

		public Dictionary<char, SpriteFont.Glyph> GetGlyphs()
		{
			return _spriteFont.GetGlyphs();
		}
		
		public Vector2 MeasureString(string text)
		{
			return _spriteFont.MeasureString(text);
		}

		public Vector2 MeasureString(StringBuilder text)
		{
			return _spriteFont.MeasureString(text);
		}

		public void Draw(string text, Vector2 pos, TextAlign halign, TextAlign valign)
		{
			string[] lines = text.Split(new []{Environment.NewLine}, StringSplitOptions.None);

			float textH = _spriteFont.MeasureString(text).Y;

			Vector2 align = new Vector2((float)halign, (float)valign) / 2f;
			Vector2 offset = Vector2.Zero;

			
			foreach(string line in lines)
			{
				Vector2 lineSize = _spriteFont.MeasureString(line);
				Vector2 lineOffset = new Vector2(lineSize.X * align.X, textH * align.Y);
				DrawCntrl.Batch.DrawString(_spriteFont, line, pos - lineOffset + offset, Color.Black);	
				offset.Y += lineSize.Y;
			}
		}
	}
}
