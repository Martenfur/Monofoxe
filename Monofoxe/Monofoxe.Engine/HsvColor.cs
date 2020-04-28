using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Text;

namespace Monofoxe.Engine
{
  public struct HsvColor
  {
		public float _hue;
		public float Hue { get => _hue; set => _hue = MathHelper.Clamp(value, 0, 360); }
		public float _saturation;

		public float Saturation { get => _saturation; set => _saturation = MathHelper.Clamp(value, 0, 1); }
		public float _value;

		public float Value { get => _value; set => _value = MathHelper.Clamp(value, 0, 1); }
		public float _alpha;

		public float Alpha { get => _alpha; set => _alpha = MathHelper.Clamp(value, 0, 1); }

		public HsvColor(float hue, float saturation, float value, float alpha = 1)
		{
			_hue = MathHelper.Clamp(hue, 0, 360); ;
			_saturation = _saturation = MathHelper.Clamp(saturation, 0, 1); ;
			_value = _saturation = MathHelper.Clamp(value, 0, 1); ;
			_alpha = _saturation = MathHelper.Clamp(alpha, 0, 1); ;
		}
		/// <summary>
		/// Create a color from the HSV color model
		/// </summary>
		/// <returns>Return the <see cref="Color"/> in RGB </returns>
		public Color ToColor()
		{
			
			if (Saturation == 0)
			{
				return new Color(Value, Value, Value, Alpha);
			}
			if (Value == 0)
			{
				return Color.Black;
			}
			float HS = (Hue / 60) % 6;
			int HSI = (int)HS;
			float f = HS - HSI;
			float p = Value * (1 - Saturation);
			float q = Value * (1 - (f * Saturation));
			float t = Value * (1 - ((1 - f) * Saturation));
			switch (HSI)
			{
				case 0:
					return new Color(Value, t, p, Alpha);
				case 1:
					return new Color(q, Value, t, Alpha);
				case 2:
					return new Color(p, Value, t, Alpha);
				case 3:
					return new Color(p, q, Value, Alpha);
				case 4:
					return new Color(t, p, Value, Alpha);
				case 5:
					return new Color(Value, p, q, Alpha);
			}
			return Color.Black; // Lets keep the compiler happy
		}
	}
}
