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
			float value = 0 ;
			float r, g, b;

			r = color.R / 255f;
			g = color.G / 255f;
			b = color.B / 255f;

			float Max = Math.Max(Math.Max(r, g), b);
			float Min = Math.Min(Math.Min(r, g), b);

			float delta = Max - Min;


			// This is why nobody likes switch, when we will be able to do something like this in an idiomatic way
			if (delta > 0)
			{
				if (Max == r)
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
				else if (Max == g)
				{
					hue = 60 * ((b - r) / delta) + 120;
				}
				else if (Max == b)
				{
					hue = 60 * ((r - g) / delta) + 240;
				}
				else
				{
					throw new Exception("Somehow foxes told us something that was not defined converting to HSV");
				}

				saturation = Max == 0 ? 0 : delta/Max;
				value = Max / 255f;
			}


			return new HsvColor(hue, saturation, value, color.A);
		}


	}
}
