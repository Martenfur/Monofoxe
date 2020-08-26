using Microsoft.Xna.Framework;

namespace Monofoxe.Engine
{
  public struct HsvColor
	{
		public float H 
		{ 
			get => _h; 
			set => _h = MathHelper.Clamp(value, 0, 360); 
		}
		public float _h;

		public float S 
		{ 
			get => _s; 
			set => _s = MathHelper.Clamp(value, 0, 1); 
		}
		public float _s;

		public float V 
		{ 
			get => _v; 
			set => _v = MathHelper.Clamp(value, 0, 1); 
		}
		public float _v;

		public float A 
		{ 
			get => _a; 
			set => _a = MathHelper.Clamp(value, 0, 1); 
		}
		public float _a;


		public HsvColor(Color color)
		{ 
			var hsv = color.ToHsvColor();
			_h = hsv.H;
			_s = hsv.S;
			_v = hsv.V;
			_a = hsv.A;
		}


		public HsvColor(float h, float s, float v, float a = 1f)
		{
			_h = MathHelper.Clamp(h, 0, 360);
			_s = MathHelper.Clamp(s, 0, 1);
			_v = MathHelper.Clamp(v, 0, 1);
			_a = MathHelper.Clamp(a, 0, 1);
		}


		/// <summary>
		/// Creates a color from the HSV color model.
		/// </summary>
		/// <returns>Return the <see cref="Color"/> in RGB </returns>
		public Color ToColor()
		{

			if (S == 0)
			{
				return new Color(V, V, V, A);
			}
			if (V == 0)
			{
				return Color.Black;
			}
			float HS = (H / 60) % 6;
			int HSI = (int)HS;
			float f = HS - HSI;
			float p = V * (1 - S);
			float q = V * (1 - (f * S));
			float t = V * (1 - ((1 - f) * S));
			switch (HSI)
			{
				case 0:
					return new Color(V, t, p, A);
				case 1:
					return new Color(q, V, t, A);
				case 2:
					return new Color(p, V, t, A);
				case 3:
					return new Color(p, q, V, A);
				case 4:
					return new Color(t, p, V, A);
				case 5:
					return new Color(V, p, q, A);
			}
			return Color.Black; // Keeps the compiler happy.
		}
	}
}
