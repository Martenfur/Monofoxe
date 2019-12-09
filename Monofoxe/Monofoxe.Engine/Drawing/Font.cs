using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Monofoxe.Engine.Drawing
{
	/// <summary>
	/// Wrapper of SpriteFont.
	/// </summary>
	public class Font : IFont
	{
		private static SpriteBatch _batch;

		public Texture2D Texture => _spriteFont.Texture;

		public ReadOnlyCollection<char> Characters => _spriteFont.Characters;

		public char? DefaultCharacter
		{
			get => _spriteFont.DefaultCharacter;
			set => _spriteFont.DefaultCharacter = value;
		}

		public int LineSpacing
		{
			get => _spriteFont.LineSpacing;
			set => _spriteFont.LineSpacing = value;
		}

		public float Spacing
		{
			get => _spriteFont.Spacing;
			set => _spriteFont.Spacing = value;
		}
		
		private SpriteFont _spriteFont;

		public Font(SpriteFont spriteFont)
		{
			if (_batch == null)
			{
				_batch = new SpriteBatch(GraphicsMgr.Device);
			}
			_spriteFont = spriteFont;
		}

		/// <summary>
		/// Returns a Dictionary of Glyphs for current font.
		/// </summary>
		public Dictionary<char, SpriteFont.Glyph> GetGlyphs() => 
			_spriteFont.GetGlyphs();

		/// <summary>
		/// Measures both width and height of text.
		/// </summary>
		public Vector2 MeasureString(string text) => 
			_spriteFont.MeasureString(text);

		/// <summary>
		/// Measures both width and height of text.
		/// </summary>
		public Vector2 MeasureString(StringBuilder text) => 
			_spriteFont.MeasureString(text);

		// A lump of shitcode.
		// SpriteFont doesn't provide functions measuring only
		// width or height, so we have to always calculate both.

		/// <summary>
		/// Measures width of the text. 
		/// NOTE: It is highly recommended to use MeasureString, 
		/// since under the hood it is still just a MeasureString call.
		/// </summary>
		public float MeasureStringWidth(string text) => 
			_spriteFont.MeasureString(text).X;

		/// <summary>
		/// Measures width of the text. 
		/// NOTE: It is highly recommended to use MeasureString, 
		/// since under the hood it is still just a MeasureString call.
		/// </summary>
		public float MeasureStringWidth(StringBuilder text) => 
			_spriteFont.MeasureString(text).X;


		/// <summary>
		/// Measures height of the text. 
		/// NOTE: It is highly recommended to use MeasureString, 
		/// since under the hood it is still just a MeasureString call.
		/// </summary>
		public float MeasureStringHeight(string text) => 
			_spriteFont.MeasureString(text).Y;


		/// <summary>
		/// Measures height of the text. 
		/// NOTE: It is highly recommended to use MeasureString, 
		/// since under the hood it is still just a MeasureString call.
		/// </summary>
		public float MeasureStringHeight(StringBuilder text) => 
			_spriteFont.MeasureString(text).Y;

		/// <summary>
		/// Draws text. Not recommended to call on its own, use Text class instead.
		/// </summary>
		public void Draw(string text, Vector2 position, TextAlign halign, TextAlign valign)
		{
			string[] lines = text.Split(new []{Environment.NewLine}, StringSplitOptions.None);

			float textH = _spriteFont.MeasureString(text).Y;

			var align = new Vector2((float)halign, (float)valign) / 2f;
			var offset = Vector2.Zero;

			GraphicsMgr.VertexBatch.FlushBatch();

			_batch.Begin(
				SpriteSortMode.Deferred, 
				GraphicsMgr.VertexBatch.BlendState, 
				GraphicsMgr.VertexBatch.SamplerState,
				GraphicsMgr.VertexBatch.DepthStencilState,
				GraphicsMgr.VertexBatch.RasterizerState,
				GraphicsMgr.VertexBatch.Effect,
				GraphicsMgr.VertexBatch.View
			);
			foreach(var line in lines)
			{
				Vector2 lineSize = _spriteFont.MeasureString(line);
				Vector2 lineOffset = new Vector2(lineSize.X * align.X, textH * align.Y);
				_batch.DrawString(_spriteFont, line, position - lineOffset + offset, GraphicsMgr.CurrentColor);	
				offset.Y += lineSize.Y;
			}
			_batch.End();
		}
	}
}
