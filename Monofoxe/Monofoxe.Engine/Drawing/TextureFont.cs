using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace Monofoxe.Engine.Drawing
{
	public class TextureFont : IFont
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

		public TextureFont(Sprite sprite, int spacing, int lineSpacing, string characters, bool monowidth, char defaultCharacter = 'a')
		{
			Spacing = spacing;
			LineSpacing = lineSpacing;
			DefaultCharacter = defaultCharacter;
			Characters = Array.AsReadOnly(characters.ToCharArray());

			_glyphs = new Dictionary<char, SpriteFont.Glyph>();
			_frames = new Dictionary<char, Frame>();
			
			if (sprite.Frames.Length < characters.Length)
			{
				throw(new Exception("Amount of characters in sample string is larger than amount of frames in sprite!"));
			}

			Texture2D frameTexture = null;
			Color[] data = null;

			int i = 0;
			foreach(char ch in Characters)
			{
				Frame frame = sprite.Frames[i];

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
									rightBearing = frame.W - (x + (int)frame.Origin.X - frame.TexturePosition.X + 1);
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


				SpriteFont.Glyph glyph = new SpriteFont.Glyph();
				glyph.Character = ch;
				glyph.BoundsInTexture = frame.TexturePosition;
				glyph.Width = frame.W - leftBearing - rightBearing;
				glyph.WidthIncludingBearings = frame.W;
				glyph.LeftSideBearing = leftBearing;
				glyph.RightSideBearing = rightBearing;

				_glyphs.Add(ch, glyph);
				_frames.Add(ch, frame);

				i += 1;
			}

		}



		public Dictionary<char, SpriteFont.Glyph> GetGlyphs()
		{
			return _glyphs;
		}
		

		/// <summary>
		/// Measures width and height of the text.
		/// </summary>
		/// <param name="text"></param>
		/// <returns></returns>
		public Vector2 MeasureString(string text)
		{
			return new Vector2(MeasureStringWidth(text), MeasureStringHeight(text));
		}

		/// <summary>
		/// Measures width and height of the text.
		/// </summary>
		/// <param name="text"></param>
		/// <returns></returns>
		public Vector2 MeasureString(StringBuilder text)
		{
			return MeasureString(text.ToString());
		}

		/// <summary>
		/// Measures width of the text. 
		/// </summary>
		/// <param name="text"></param>
		/// <returns></returns>
		public float MeasureStringWidth(string text)
		{
			string[] lines = text.Split(new []{Environment.NewLine}, StringSplitOptions.None);
			
			// Width.
			float maxWidth = 0;

			foreach(string line in lines)
			{
				float width = 0;
				foreach(char ch in line)
				{
					try
					{
						width += _glyphs[ch].Width + Spacing;
					}
					catch(Exception)
					{
						width += _glyphs[(char)DefaultCharacter].Width + Spacing;
					}
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
		/// <param name="text"></param>
		/// <returns></returns>
		public float MeasureStringWidth(StringBuilder text)
		{
			return MeasureStringWidth(text.ToString());
		}

		
		/// <summary>
		/// Measures height of the text. 
		/// </summary>
		/// <param name="text"></param>
		/// <returns></returns>
		public float MeasureStringHeight(string text)
		{
			string[] lines = text.Split(new []{Environment.NewLine}, StringSplitOptions.None);

			return lines.Length * (_frames[(char)DefaultCharacter].H + LineSpacing) - LineSpacing;
		}
		
		
		/// <summary>
		/// Measures height of the text. 
		/// </summary>
		/// <param name="text"></param>
		/// <returns></returns>
		public float MeasureStringHeight(StringBuilder text)
		{
			return MeasureStringHeight(text.ToString());
		}


		/// <summary>
		/// Draws text. Not recommended to call on its own, use DrawMgr functions instead.
		/// </summary>
		/// <param name="text"></param>
		/// <param name="pos"></param>
		/// <param name="halign"></param>
		/// <param name="valign"></param>
		public void Draw(SpriteBatch batch, string text, Vector2 pos, TextAlign halign, TextAlign valign)
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
					try
					{
						frame = _frames[ch];
						glyph = _glyphs[ch];
					}
					catch(Exception)
					{
						frame = _frames[(char)DefaultCharacter];
						glyph = _glyphs[(char)DefaultCharacter];
					}

					var border = new Vector2(-glyph.LeftSideBearing, 0);
					var lineOffset = new Vector2(strSize.X * align.X, textH * align.Y);
					
					batch.Draw(frame.Texture, pos + offset + frame.Origin + border - lineOffset, frame.TexturePosition, DrawMgr.CurrentColor);
					offset.X += glyph.Width + Spacing;
				}
				offset.X = 0;
				offset.Y += strSize.Y + LineSpacing;
			}
		}
	}
}
