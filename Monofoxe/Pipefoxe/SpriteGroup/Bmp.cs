using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content.Pipeline.Serialization.Compiler;
using StbImageSharp;
using System.IO;

namespace Pipefoxe.SpriteGroup
{
	public class Bmp
	{
		private Color[] _pixels;

		public readonly int Width;
		public readonly int Height;

		public Bmp(string filePath)
		{
			using (var stream = File.OpenRead(filePath))
			{
				var image = ImageResult.FromStream(stream, ColorComponents.RedGreenBlueAlpha);
				var data = image.Data;
				Width = image.Width;
				Height = image.Height;

				_pixels = new Color[Width * Height];
				for (var i = 0; i < data.Length / 4; i += 1)
				{
					var alpha = data[i * 4 + 3];
					if (alpha != 0)
					{
						_pixels[i] = new Color(data[i * 4], data[i * 4 + 1], data[i * 4 + 2], alpha);
					}
					else
					{ 
						_pixels[i] = Color.Transparent; // XNA zeroes pixels with 0 alpha. 
					}
				}
			}
		}


		public Bmp(int width, int height)
		{
			Width = width;
			Height = height;

			_pixels = new Color[Width * Height];
		}


		/// <summary>
		/// Draws given bitmap on a current one.
		/// </summary>
		public void Draw(Bmp bmp, int x, int y)
		{
			for (var yy = 0; yy < bmp.Height; yy += 1)
			{
				for (var xx = 0; xx < bmp.Width; xx += 1)
				{
					DrawPixel(bmp.GetPixel(xx, yy), x + xx, y + yy);
				}
			}
		}


		/// <summary>
		/// Draws given bitmap on a current one.
		/// </summary>
		public void Draw(Bmp bmp, int x, int y, Rectangle srcRectangle)
		{
			for (var yy = srcRectangle.Y; yy < srcRectangle.Height; yy += 1)
			{
				for (var xx = srcRectangle.X; xx < srcRectangle.Width; xx += 1)
				{
					DrawPixel(bmp.GetPixel(xx, yy), x + xx, y + yy);
				}
			}
		}


		public void Write(ContentWriter writer)
		{
			writer.Write(Width);
			writer.Write(Height);
			for(var i = 0; i < _pixels.Length; i += 1)
			{
				writer.Write(_pixels[i]);
			}
		}


		private Color GetPixelUnchecked(int x, int y) =>
			_pixels[Width * y + x];
	

		private Color GetPixel(int x, int y)
		{
			if (x < 0)
			{
				x = 0;
			}
			if (y < 0)
			{
				y = 0;
			}
			if (x >= Width)
			{
				x = Width - 1;
			}
			if (y >= Height)
			{
				y = Height - 1;
			}

			return GetPixelUnchecked(x, y);
		}


		private void DrawPixel(Color pixel, int x, int y)
		{
			if (x < 0 || y < 0 || x >= Width || y >= Height)
			{
				return;
			}
			_pixels[Width * y + x] = pixel;
		}
	}
}
