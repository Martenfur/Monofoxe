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

		/// <summary>
		/// Returns a Dictionary of Glyphs for current font.
		/// </summary>
		/// <returns></returns>
		public Dictionary<char, SpriteFont.Glyph> GetGlyphs()
		{
			return _spriteFont.GetGlyphs();
		}
		
		/// <summary>
		/// Measures both width and height of text.
		/// </summary>
		/// <param name="text"></param>
		/// <returns></returns>
		public Vector2 MeasureString(string text)
		{
			return _spriteFont.MeasureString(text);
		}

		/// <summary>
		/// Measures both width and height of text.
		/// </summary>
		/// <param name="text"></param>
		/// <returns></returns>
		public Vector2 MeasureString(StringBuilder text)
		{
			return _spriteFont.MeasureString(text);
		}

		// A lump of shitcode.
		// SpriteFont doesn't privide functions measuring only
		// width or height, so we have to always calculate both.
		
		/// <summary>
		/// Measures width of the text. 
		/// NOTE: It is highly recommended to use MeasureString, 
		/// since under the hood it is still just a MeasureString call.
		/// </summary>
		/// <param name="text"></param>
		/// <returns></returns>
		public float MeasureStringWidth(string text)
		{
			return _spriteFont.MeasureString(text).X;
		}

		/// <summary>
		/// Measures width of the text. 
		/// NOTE: It is highly recommended to use MeasureString, 
		/// since under the hood it is still just a MeasureString call.
		/// </summary>
		/// <param name="text"></param>
		/// <returns></returns>
		public float MeasureStringWidth(StringBuilder text)
		{
			return _spriteFont.MeasureString(text).X;
		}

		
		/// <summary>
		/// Measures height of the text. 
		/// NOTE: It is highly recommended to use MeasureString, 
		/// since under the hood it is still just a MeasureString call.
		/// </summary>
		/// <param name="text"></param>
		/// <returns></returns>
		public float MeasureStringHeight(string text)
		{
			return _spriteFont.MeasureString(text).Y;
		}
		
		
		/// <summary>
		/// Measures height of the text. 
		/// NOTE: It is highly recommended to use MeasureString, 
		/// since under the hood it is still just a MeasureString call.
		/// </summary>
		/// <param name="text"></param>
		/// <returns></returns>
		public float MeasureStringHeight(StringBuilder text)
		{
			return _spriteFont.MeasureString(text).Y;
		}

		/// <summary>
		/// Draws text. Not recommended to call on its own, use DrawCntrl functions instead.
		/// </summary>
		/// <param name="text"></param>
		/// <param name="pos"></param>
		/// <param name="halign"></param>
		/// <param name="valign"></param>
		public void Draw(SpriteBatch batch, string text, Vector2 pos, TextAlign halign, TextAlign valign)
		{
			string[] lines = text.Split(new []{Environment.NewLine}, StringSplitOptions.None);

			float textH = _spriteFont.MeasureString(text).Y;

			Vector2 align = new Vector2((float)halign, (float)valign) / 2f;
			Vector2 offset = Vector2.Zero;

			
			foreach(string line in lines)
			{
				Vector2 lineSize = _spriteFont.MeasureString(line);
				Vector2 lineOffset = new Vector2(lineSize.X * align.X, textH * align.Y);
				batch.DrawString(_spriteFont, line, pos - lineOffset + offset, Color.Black);	
				offset.Y += lineSize.Y;
			}
		}
	}
}
