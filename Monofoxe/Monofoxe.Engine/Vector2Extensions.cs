using System;
using Microsoft.Xna.Framework;
using Monofoxe.Engine.Utils;

namespace Monofoxe.Engine
{
	/// <summary>
	/// Vector2 extensions.
	/// </summary>
	public static class Vector2Extensions
	{
		// Why RoundV and not Round? For some reason there is name clash with
		// Vector2 static methods on newer MG build.

		/// <summary>
		/// Rounds each vector's component.
		/// </summary>
		public static Vector2 RoundV(this Vector2 v) =>
			new Vector2((float)Math.Round(v.X), (float)Math.Round(v.Y));

		/// <summary>
		/// Rounds each vector's component down.
		/// </summary>
		public static Vector2 FloorV(this Vector2 v) =>
			new Vector2((float)Math.Floor(v.X), (float)Math.Floor(v.Y));

		/// <summary>
		/// Rounds each vector's component up.
		/// </summary>
		public static Vector2 CeilingV(this Vector2 v) =>
			new Vector2((float)Math.Ceiling(v.X), (float)Math.Ceiling(v.Y));


		/// <summary>
		/// Returns vector with the same direction and length of 1. 
		/// If original vector is (0;0), returns zero vector.
		/// </summary>
		public static Vector2 GetSafeNormalize(this Vector2 v)
		{
			if (v == Vector2.Zero)
			{
				return Vector2.Zero;
			}
			v.Normalize();
			return v;
		}

		/// <summary>
		/// Converts Vector2 to Vector3 with z axis of 0.
		/// </summary>
		public static Vector3 ToVector3(this Vector2 v) =>
			new Vector3(v.X, v.Y, 0);

		/// <summary>
		/// Returns an angle based on the vector's direction.
		/// </summary>
		public static Angle ToAngle(this Vector2 v) =>
			new Angle(v);


		/// <summary>
		/// Swaps X and Y places.
		/// </summary>
		public static Vector2 Swap(this Vector2 v) =>
			new Vector2(v.Y, v.X);
		
		/// <summary>
		/// Rotates vector by 90 degrees.
		/// </summary>
		public static Vector2 Rotate90(this Vector2 v) =>
			new Vector2(v.Y, -v.X);

		/// <summary>
		/// Rotates vector by the given angle.
		/// </summary>
		public static Vector2 Rotate(this Vector2 v, Angle angle)
		{
			var e = angle.ToVector2();

			return new Vector2(
				v.X * e.X - v.Y * e.Y,
				v.X * e.Y + v.Y * e.X
			);
		}

		/// <summary>
		/// Rotates vector by the given direction vector.
		/// NOTE: Rotation vector should be unit vector.
		/// </summary>
		public static Vector2 Rotate(this Vector2 v, Vector2 rotationVector)
		{
			return new Vector2(
				v.X * rotationVector.X - v.Y * rotationVector.Y,
				v.X * rotationVector.Y + v.Y * rotationVector.X
			);
		}


		public static Vector2 Projection(this Vector2 v, Vector2 other) =>
			Vector2.Dot(v, other) / Vector2.Dot(v, v) * v;
	}
}
