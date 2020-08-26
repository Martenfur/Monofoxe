using Microsoft.Xna.Framework;
using System;

namespace Monofoxe.Engine
{
	/// <summary>
	/// <see cref="Color"/> extensions
	/// </summary>
	public static class ColorExtensions
	{

		/// <summary>
		/// Return the HsvColor equivalent of a given color 
		/// </summary>
		/// <param name="color">The color that will be converted</param>
		/// <returns><see cref="HsvColor"/> equivalent of the color</returns>
		public static HsvColor ToHsvColor(this Color color)
		{
			float hue = 0;
			float saturation = 0;
			float value = 0;
			float r, g, b;

			r = color.R / 255f;
			g = color.G / 255f;
			b = color.B / 255f;

			float max = Math.Max(Math.Max(r, g), b);
			float min = Math.Min(Math.Min(r, g), b);

			float delta = max - min;


			// This is why nobody likes switch, when we will be able to do something like this in an idiomatic way.
			if (delta != 0)
			{
				if (max == r)
				{
					if (g >= b)
					{
						hue = 60 * ((g - b) / delta);
					}
					else
					{
						hue = (60 * ((g - b) / delta)) + 360;
					}
				}
				else if (max == g)
				{
					hue = 60 * ((b - r) / delta) + 120;
				}
				else if (max == b)
				{
					hue = 60 * ((r - g) / delta) + 240;
				}
			}

			saturation = max == 0 ? 0 : delta / max;

			value = max;

			return new HsvColor(hue, saturation, value, color.A);
		}


	}
}
