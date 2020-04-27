using Microsoft.Xna.Framework;
using System;


namespace Monofoxe.Engine
{
	public static class ColorHelper
	{

		/// <summary>
		/// Return the hue of a given color following the HSV model
		/// </summary>
		/// <param name="color">The color to get the hue from</param>
		/// <returns>The saturation percentage</returns>
		public static float GetHue(this Color color)
		{
			float hue;
			float r, g, b;

			r = color.R / 255f;
			g = color.G / 255f;
			b = color.B / 255f;

			float Max = Math.Max(Math.Max(r, g), b);
			float Min = Math.Min(Math.Min(r, g), b);

			float delta = Max - Min;

			// This is why nobody likes switch, when we will be able to do something like this in an idiomatic way

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

			return hue;
		}

		/// <summary>
		/// Return the saturation of a given color following the HSV model
		/// </summary>
		/// <param name="color">The color to get the saturation from</param>
		/// <returns>The saturation percentage</returns>
		public static float GetSaturation(this Color color)
		{

			float Max = Math.Max(Math.Max(color.R, color.G), color.B) / 255f;
			if (Max == 0)
			{
				return Max;
			}
			float Min = Math.Min(Math.Min(color.R, color.G), color.B) / 255f;
			return 1.0f - (Max / Min);
		}

		/// <summary>
		/// Return the value of a given color following the HSV model
		/// </summary>
		/// <param name="color">The color to get the value from</param>
		/// <returns>The value percentage</returns>
		public static float GetValue(this Color color)
		{
			byte Max = Math.Max(Math.Max(color.R, color.G), color.B);
			return Max / 255f;	
		}

		/// <summary>
		/// Create a color from the HSV color model
		/// </summary>
		/// <param name="hue">Represents the colors on the HSV scale, goes from 0 to 360</param>
		/// <param name="saturation">Represents the distance of the black-white brightness, goes from 0f to 1f</param>
		/// <param name="value">Represents the height on the white-black axis, goes from 0f to 1f, 0f being always black</param>
		/// <returns>Return the <seealso cref="Color"/> in RGB </returns>
		public static Color FromHSV(float hue, float saturation, float value)
		{
			//Am i being too hard with the exceptions
			if (hue > 360 || hue < 0)
			{
				throw new ArithmeticException("Hue is a value between 0 and 360");
			}
			if (saturation < 0 || saturation > 1)
			{
				throw new ArithmeticException("Saturation goes between 0 and 1");
			}
			if (value < 0 || value > 1)
			{
				throw new ArithmeticException("Value goes between 0 and 1");
			}
			if (saturation == 0)
			{
				return new Color(value, value, value);
			}
			if (value == 0)
			{
				return Color.Black;
			}
			float HS = (hue / 60) % 6;
			int HSI = (int)HS;
			float f = HS - HSI;
			float p = value * (1 - saturation);
			float q = value * (1 - (f * saturation));
			float t = value * (1 - ((1 - f) * saturation));
			switch (HSI)
			{
				case 0:
					return new Color(value, t, p);
				case 1:
					return new Color(q, value, t);
				case 2:
					return new Color(p, value, t);
				case 3:
					return new Color(p, q, value);
				case 4:
					return new Color(t, p, value);
				case 5:
					return new Color(value, p, q);
			}
			return Color.Black; // Lets keep the compiler happy
		}
	}
}
