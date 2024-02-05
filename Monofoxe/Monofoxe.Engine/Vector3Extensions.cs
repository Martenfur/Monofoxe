using Microsoft.Xna.Framework;
using System;

namespace Monofoxe.Engine
{
	/// <summary>
	/// Vector3 extensions.
	/// </summary>
	public static class Vector3Extensions
	{
		/// <summary>
		/// Converts Vector3 to Vector2 by dropping Z.
		/// </summary>
		public static Vector2 ToVector2(this Vector3 v) =>
			new Vector2(v.X, v.Y);

		// Why RoundV and not Round? For some reason there is name clash with
		// Vector3 static methods on newer MG build.

		/// <summary>
		/// Rounds each vector's component.
		/// </summary>
		public static Vector3 RoundV(this Vector3 v) =>
			new Vector3((float)Math.Round(v.X), (float)Math.Round(v.Y), (float)Math.Round(v.Z));

		/// <summary>
		/// Rounds each vector's component down.
		/// </summary>
		public static Vector3 FloorV(this Vector3 v) =>
			new Vector3((float)Math.Floor(v.X), (float)Math.Floor(v.Y), (float)Math.Floor(v.Z));

		/// <summary>
		/// Rounds each vector's component up.
		/// </summary>
		public static Vector3 CeilingV(this Vector3 v) =>
			new Vector3((float)Math.Ceiling(v.X), (float)Math.Ceiling(v.Y), (float)Math.Ceiling(v.Z));


		/// <summary>
		/// Returns vector with the same direction and length of 1. 
		/// If original vector is (0;0;0), returns zero vector.
		/// </summary>
		public static Vector3 SafeNormalize(this Vector3 v)
		{
			if (v == Vector3.Zero)
			{
				return Vector3.Zero;
			}
			v.Normalize();
			return v;
		}


		public static Vector3 Projection(this Vector3 v, Vector3 other)
		{
			if (v == Vector3.Zero)
			{
				return Vector3.Zero;
			}
			return Vector3.Dot(v, other) / Vector3.Dot(v, v) * v;
		}
	}
}
