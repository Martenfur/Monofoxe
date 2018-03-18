using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace Monofoxe.Engine.Drawing
{
	class TextureFont : IFont
	{
		#region fields

		public Texture2D Texture {get;}
		
		public ReadOnlyCollection<char> Characters {get;}
		
		public char? DefaultCharacter {get; set;}
		
		public int LineSpacing {get; set;}
		
		public float Spacing {get; set;}

		#endregion fields
		
		private Dictionary<char, SpriteFont.Glyph> _glyphs;
		private Dictionary<char, Frame> _frames;

		public TextureFont(Sprite sprite, string characters, bool monowidth)
		{
			_glyphs = new Dictionary<char, SpriteFont.Glyph>();
			_frames = new Dictionary<char, Frame>();
			Characters = Array.AsReadOnly(characters.ToCharArray());

			if (sprite.Frames.Length < characters.Length)
			{
				throw(new Exception("Amount of characters in sample string is larger than amount of frames in sprite!"));
			}

			int i = 0;
			foreach(char ch in Characters)
			{
				SpriteFont.Glyph glyph = new SpriteFont.Glyph();
				glyph.Character = ch;
				glyph.BoundsInTexture = sprite.Frames[i].TexturePosition;
				glyph.Width = sprite.Frames[i].W;
				glyph.WidthIncludingBearings = sprite.Frames[i].W;
				glyph.LeftSideBearing = 0;
				glyph.RightSideBearing = 0;
				
				_glyphs.Add(ch, glyph);
				_frames.Add(ch, sprite.Frames[i]);

				i += 1;
			}
		}

		public Dictionary<char, SpriteFont.Glyph> GetGlyphs()
		{
			return _glyphs;
		}
		
		public Vector2 MeasureString(string text)
		{
			return Vector2.Zero;
		}

		public Vector2 MeasureString(StringBuilder text)
		{
			return Vector2.Zero;
		}

		public void Draw(string text, Vector2 pos, TextAlign halign, TextAlign valign)
		{
			string[] lines = text.Split(new []{Environment.NewLine}, StringSplitOptions.None);

			float textH = _frames['a'].H;

			Vector2 align = new Vector2((float)halign, (float)valign) / 2f;
			Vector2 offset = Vector2.Zero;

			
			foreach(string line in lines)
			{
				foreach(char ch in line)
				{
					Frame frame;
					try
					{
						frame = _frames[ch];
					}
					catch(Exception)
					{
						frame = _frames['a'];
					}

					DrawCntrl.Batch.Draw(frame.Texture, pos + offset, frame.TexturePosition, DrawCntrl.CurrentColor);
					offset.X += frame.W;
				}
				offset.X = 0;
				offset.Y += textH;
			}
		}
	}
}
