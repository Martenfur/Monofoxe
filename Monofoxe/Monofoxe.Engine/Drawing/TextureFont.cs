using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Monofoxe.Engine.Drawing
{
	public class TextureFont : IFont
	{
		/// <summary>
		/// Unused, because TextureFont can have multiple textures.
		/// </summary>
		public Texture2D Texture => null;
		
		public ReadOnlyCollection<char> Characters {get;}
		
		public char? DefaultCharacter {get; set;}
		
		public int LineSpacing {get; set;}
		
		public float Spacing {get; set;}

		
		private Dictionary<char, SpriteFont.Glyph> _glyphs;
		private Dictionary<char, Frame> _frames;

		public TextureFont(Sprite sprite, int spacing, int lineSpacing, string characters, bool monowidth, char defaultCharacter = 'a')
		{
			Spacing = spacing;
			LineSpacing = lineSpacing;
			DefaultCharacter = defaultCharacter;
			Characters = Array.AsReadOnly(characters.ToCharArray());

			_glyphs = new Dictionary<char, SpriteFont.Glyph>();
			_frames = new Dictionary<char, Frame>();
			
			if (sprite.FramesCount < characters.Length)
			{
				throw new Exception("Amount of characters in sample string is larger than amount of frames in sprite!");
			}

			Texture2D frameTexture = null;
			Color[] data = null;

			int i = 0;
			foreach(var ch in Characters)
			{
				var frame = sprite[i];

				if (frame.Texture != frameTexture)
				{
					data = new Color[frame.Texture.Width * frame.Texture.Height];
					frame.Texture.GetData(data);
					frameTexture = frame.Texture;
				}
				
				int leftBearing, rightBearing;

				if (monowidth)
				{
					leftBearing = 0;
					rightBearing = 0;
				}
				else
				{
					leftBearing = -1;
					rightBearing = -1;

					// Scanning from left side. If non-transparent pixel was found -- stop.
					for(var x = frame.TexturePosition.X; x < frame.TexturePosition.X + frame.TexturePosition.Width; x += 1)
					{
						for(var y = frame.TexturePosition.Y; y < frame.TexturePosition.Y + frame.TexturePosition.Height; y += 1)
						{
							if (data[frame.Texture.Width * y + x].A != 0)
							{
								leftBearing = x + (int)frame.Origin.X - frame.TexturePosition.X;
								break;
							}
						}
						if (leftBearing != -1)
						{
							break;
						}
					}
			
					if (leftBearing == -1) // This means that image is fully empty and there's no need to check other side. Usually it's a space.
					{
						leftBearing = 0;
						rightBearing = 0;
					}
					else
					{
						// Scanning from right side. If non-transparent pixel was found -- stop.
						for(var x = frame.TexturePosition.X + frame.TexturePosition.Width - 1; x >= frame.TexturePosition.X; x -= 1)
						{
							for(var y = frame.TexturePosition.Y; y < frame.TexturePosition.Y + frame.TexturePosition.Height; y += 1)
							{
								if (data[frame.Texture.Width * y + x].A != 0)
								{
									rightBearing = frame.Width - (x + (int)frame.Origin.X - frame.TexturePosition.X + 1);
									break;
								}
							}
							if (rightBearing != -1)
							{
								break;
							}
						}
					}
				}


				var glyph = new SpriteFont.Glyph
				{
					Character = ch,
					BoundsInTexture = frame.TexturePosition,
					Width = frame.Width - leftBearing - rightBearing,
					WidthIncludingBearings = frame.Width,
					LeftSideBearing = leftBearing,
					RightSideBearing = rightBearing
				};

				_glyphs.Add(ch, glyph);
				_frames.Add(ch, frame);

				i += 1;
			}

		}



		public Dictionary<char, SpriteFont.Glyph> GetGlyphs() =>
			_glyphs;
		

		/// <summary>
		/// Measures width and height of the text.
		/// </summary>
		public Vector2 MeasureString(string text) =>
			new Vector2(MeasureStringWidth(text), MeasureStringHeight(text));

		/// <summary>
		/// Measures width and height of the text.
		/// </summary>
		public Vector2 MeasureString(StringBuilder text) =>
			MeasureString(text.ToString());

		/// <summary>
		/// Measures width of the text. 
		/// </summary>
		public float MeasureStringWidth(string text)
		{
			var lines = text.Split(new []{Environment.NewLine}, StringSplitOptions.None);
			
			// Width.
			float maxWidth = 0;

			foreach(var line in lines)
			{
				float width = 0;
				foreach(var ch in line)
				{
					var currentChar = ch;
					if (!_glyphs.ContainsKey(ch))
					{
						currentChar = (char)DefaultCharacter;
					}
					width += _glyphs[currentChar].Width + Spacing;
				}
				
				width -= Spacing;
				if (width > maxWidth)
				{
					maxWidth = width;
				}
			}

			return maxWidth;
		}

		/// <summary>
		/// Measures width of the text. 
		/// </summary>
		public float MeasureStringWidth(StringBuilder text) =>
			MeasureStringWidth(text.ToString());

		
		/// <summary>
		/// Measures height of the text. 
		/// </summary>
		public float MeasureStringHeight(string text)
		{
			string[] lines = text.Split(new []{Environment.NewLine}, StringSplitOptions.None);

			return lines.Length * (_frames[(char)DefaultCharacter].Height + LineSpacing) - LineSpacing;
		}
		
		
		/// <summary>
		/// Measures height of the text. 
		/// </summary>
		public float MeasureStringHeight(StringBuilder text) =>
			MeasureStringHeight(text.ToString());


		/// <summary>
		/// Draws text. Not recommended to call on its own, use Text class instead.
		/// </summary>
		public void Draw(SpriteBatch batch, string text, Vector2 position, TextAlign halign, TextAlign valign)
		{
			string[] lines = text.Split(new []{Environment.NewLine}, StringSplitOptions.None);

			float textH = MeasureStringHeight(text);

			var align = new Vector2((float)halign, (float)valign) / 2f;
			var offset = Vector2.Zero;

			
			foreach(string line in lines)
			{
				Vector2 strSize = MeasureString(line);

				foreach(char ch in line)
				{
					Frame frame;
					SpriteFont.Glyph glyph;
					
					var currentChar = ch;
					if (!_glyphs.ContainsKey(ch))
					{
						currentChar = (char)DefaultCharacter;
					}
					
					frame = _frames[currentChar];
					glyph = _glyphs[currentChar];

					var border = new Vector2(-glyph.LeftSideBearing, 0);
					var lineOffset = new Vector2(strSize.X * align.X, textH * align.Y);
					
					batch.Draw(frame.Texture, position + offset + frame.Origin + border - lineOffset, frame.TexturePosition, GraphicsMgr.CurrentColor);
					offset.X += glyph.Width + Spacing;
				}
				offset.X = 0;
				offset.Y += strSize.Y + LineSpacing;
			}
		}
	}
}
