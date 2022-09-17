using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

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


		/// <summary>
		/// Converts Color to its hex value.
		/// </summary>
		public static string ToHex(this Color color)
		{
			var r = $"{color.R:x2}";
			var g = $"{color.G:x2}";
			var b = $"{color.B:x2}";
			if (color.A == 0)
			{
				return $"#{r}{g}{b}";
			}
			var a = $"{color.A:x2}";
			return $"#{r}{g}{b}{a}";
		}


		/// <summary>
		/// Converts #RRGGBB or #RRGGBBAA hex value to Color.
		/// </summary>
		public static Color HexToColor(string colorStr)
		{
			colorStr = colorStr.Replace("#", "");

			var channels = new byte[colorStr.Length / 2];

			for (var i = 0; i < channels.Length; i += 1)
			{
				channels[i] = Convert.ToByte(colorStr.Substring(i * 2, 2), 16);
			}

			if (channels.Length == 3)
			{
				// #RRGGBB
				return new Color(channels[0], channels[1], channels[2]);
			}
			else
			{
				// #RRGGBBAA
				return new Color(channels[0], channels[1], channels[2], channels[3]);
			}
		}

		private static readonly Dictionary<string, Color> _colorsByName = typeof(Color)
			.GetRuntimeProperties()
			.Where(p => p.PropertyType == typeof(Color))
			.ToDictionary(p => p.Name, p => (Color)p.GetValue(null), StringComparer.OrdinalIgnoreCase);

		public static Color NameToColor(string name)
		{
			// Ripped straight off Monogame.Extended, credit to them or whatever.
			Color color;

			if (_colorsByName.TryGetValue(name, out color))
			{
				return color;
			}

			throw new InvalidOperationException($"{name} is not a valid color");
		}
	}
}
