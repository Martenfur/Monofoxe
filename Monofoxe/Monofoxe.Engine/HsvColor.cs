using Microsoft.Xna.Framework;
using System;

namespace Monofoxe.Engine
{
	public struct HsvColor : IEquatable<HsvColor>, IComparable<HsvColor>
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


		public override bool Equals(object obj)
		{
			if (obj is HsvColor)
			{
				return Equals((HsvColor)obj);
			}

			return base.Equals(obj);
		}


		public bool Equals(HsvColor value) =>
			H.Equals(value.H) && S.Equals(value.S) && V.Equals(value.V);


		public override int GetHashCode() =>
			H.GetHashCode() ^ S.GetHashCode() ^ V.GetHashCode();


		public int CompareTo(HsvColor other) =>
			H.CompareTo(other.H) * 100 + S.CompareTo(other.S) * 10 + V.CompareTo(V);


		public void CopyTo(out HsvColor destination) =>
			destination = new HsvColor(H, S, V);


		public static HsvColor operator +(HsvColor a, HsvColor b) =>
			new HsvColor(a.H + b.H, a.S + b.S, a.V + b.V);

		public static bool operator ==(HsvColor x, HsvColor y) =>
			x.Equals(y);
		


		public static bool operator !=(HsvColor x, HsvColor y) =>
			!x.Equals(y);


		public static HsvColor operator -(HsvColor a, HsvColor b) =>
			new HsvColor(a.H - b.H, a.S - b.S, a.V - b.V);


		public static HsvColor Lerp(HsvColor c1, HsvColor c2, float t)
		{
			// loop around if c2.H < c1.H
			var h2 = c2.H >= c1.H ? c2.H : c2.H + 360;
			return new HsvColor(
				c1.H + t * (h2 - c1.H),
				c1.S + t * (c2.S - c1.S),
				c1.V + t * (c2.V - c2.V)
			);
		}
	}
}
