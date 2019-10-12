using Microsoft.Xna.Framework;
using System;

namespace Monofoxe.Engine.Utils
{
	public struct Angle : IEquatable<Angle>
	{
		public double Degrees 
		{
			get => _degrees; 
			set
			{
				_degrees = value;
				Normalize();
			}
		}

		public double Radians 
		{
			get => (_degrees / 360.0) * Math.PI * 2.0;
			set 
			{
				Degrees = (value / (Math.PI * 2.0)) * 360.0;
			}
		}

		public float DegreesF => (float)Degrees;
		public float RadiansF => (float)Radians;


		private double _degrees;


		public Angle(double degrees)
		{
			_degrees = degrees;
			Normalize();
		}

		public Angle(Vector2 vector)
		{
			_degrees = Math.Atan2(vector.Y, vector.X) * 360.0 / (Math.PI * 2.0);
			Normalize();
		}

		public Angle(Vector2 point1, Vector2 point2) : this(point2 - point1) {}


		public static Angle FromRadians(double radians)
		{
			var angle = new Angle();
			angle.Radians = radians;
			return angle;
		}


		public Vector2 ToVector2() =>
			new Vector2((float)Math.Cos(Radians), (float)Math.Sin(Radians));
			
		public static double ToDegrees(double radians) =>
			(radians / (Math.PI * 2.0)) * 360.0;

		public static double ToRadians(double radians) =>
			(radians / 360.0) * Math.PI * 2.0;


		public bool Equals(Angle other) =>
			Degrees == other.Degrees;

		public override bool Equals(object obj)
		{
			if (obj is Angle)
			{
				return Equals(this);
			}
			return false;
		}

		#region Operators.

		public static Angle operator +(Angle a1, Angle a2) =>
			new Angle(a1.Degrees + a2.Degrees);

		public static Angle operator -(Angle a1, Angle a2) =>
			new Angle(a1.Degrees - a2.Degrees);

		public static Angle operator *(Angle a, double num) =>
			new Angle(a.Degrees * num);

		public static Angle operator *(Angle a, float num) =>
			new Angle(a.Degrees * num);

		public static Angle operator *(Angle a, int num) =>
			new Angle(a.Degrees * num);

		public static Angle operator /(Angle a, double num) =>
			new Angle(a.Degrees / num);

		public static Angle operator /(Angle a, float num) =>
			new Angle(a.Degrees / num);

		public static Angle operator /(Angle a, int num) =>
			new Angle(a.Degrees / num);

		public static bool operator >(Angle a1, Angle a2) =>
			a1.Degrees > a2.Degrees;

		public static bool operator <(Angle a1, Angle a2) =>
			a1.Degrees < a2.Degrees;

		public static bool operator >=(Angle a1, Angle a2) =>
			a1.Degrees >= a2.Degrees;

		public static bool operator <=(Angle a1, Angle a2) =>
			a1.Degrees <= a2.Degrees;

		#endregion Operators.

		void Normalize()
		{
			_degrees = (_degrees % 360.0 + 360.0) % 360.0;
		}

	}
}
