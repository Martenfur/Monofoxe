using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content.Pipeline.Graphics;

namespace Pipefoxe.SpriteGroup
{
	public class Bmp
	{
		private Color[] _pixels;

		public readonly int Width;
		public readonly int Height;

		public Bmp(Texture2DContent texture)
		{
			var textureBitmap = texture.Mipmaps[0];
			var data = textureBitmap.GetPixelData();
			Width = textureBitmap.Width;
			Height = textureBitmap.Height;

			_pixels = new Color[Width * Height];
			for (var i = 0; i < data.Length / 4; i += 1)
			{
				_pixels[i] = new Color(data[i * 4], data[i * 4 + 1], data[i * 4 + 2], data[i * 4 + 3]);
			}
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
		public void Draw(Bmp bmp, Rectangle rectangle)
		{
			for (var y = rectangle.Y; y < rectangle.Height; y += 1)
			{
				for (var x = rectangle.X; x < rectangle.Width; x += 1)
				{
					DrawPixel(bmp.GetPixel(x, y), x, y);
				}
			}
		}


		public Texture2DContent ToTexture2DContent()
		{
			var content = new Texture2DContent();
			content.Mipmaps = new MipmapChain();
			var bitmap = new PixelBitmapContent<Color>(Width, Height);
			for (var y = 0; y < Height; y += 1)
			{
				for (var x = 0; x < Width; x += 1)
				{
					bitmap.SetPixel(x, y, GetPixelUnchecked(x, y));
				}
			}

			return content;
		}


		private Color GetPixelUnchecked(int x, int y) =>
			_pixels[Height * y + x];


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
			_pixels[Height * y + x] = pixel;
		}
	}
}
