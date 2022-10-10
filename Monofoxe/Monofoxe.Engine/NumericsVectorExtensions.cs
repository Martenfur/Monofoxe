using Microsoft.Xna.Framework;

namespace Monofoxe.Engine
{
	public static class NumericsVectorExtensions
	{
		public static System.Numerics.Vector2 ToNumericsVector(this Vector2 v) =>
			new System.Numerics.Vector2(v.X, v.Y);

		public static Vector2 ToVector2(this System.Numerics.Vector2 v) =>
			new Vector2(v.X, v.Y);

		public static System.Numerics.Vector2 ToNumericsVector2(this Vector3 v) =>
			new System.Numerics.Vector2(v.X, v.Y);

		public static System.Numerics.Vector3 ToNumericsVector(this Vector3 v) =>
			new System.Numerics.Vector3(v.X, v.Y, v.Z);

		public static System.Numerics.Vector4 ToNumericsVector(this Vector4 v) =>
			new System.Numerics.Vector4(v.X, v.Y, v.Z, v.W);

		public static Vector3 ToVector3(this System.Numerics.Vector3 v) =>
			new Vector3(v.X, v.Y, v.Z);

		public static Vector3 ToVector3(this System.Numerics.Vector2 v, float z = 0f) =>
			new Vector3(v.X, v.Y, z);

		public static Vector4 ToVector4(this System.Numerics.Vector4 v) =>
			new Vector4(v.X, v.Y, v.Z, v.W);

		public static System.Numerics.Vector4 ToNumericsVector4(this Color c) =>
			new System.Numerics.Vector4(c.R / 255f, c.G / 255f, c.B / 255f, c.A / 255f);

		public static System.Numerics.Vector3 ToNumericsVector3(this Color c) =>
			new System.Numerics.Vector3(c.R / 255f, c.G / 255f, c.B / 255f);

		public static Color ToColor(this System.Numerics.Vector4 v) =>
			new Color(v.X, v.Y, v.Z, v.W);

		public static Color ToColor(this System.Numerics.Vector3 v) =>
			new Color(v.X, v.Y, v.Z, 1);

		public static RectangleF ToRectangleF(this System.Numerics.Vector4 v) =>
			new RectangleF(v.X, v.Y, v.Z, v.W);

		public static System.Numerics.Vector4 ToNumericsVector(this RectangleF r) =>
			new System.Numerics.Vector4(r.X, r.Y, r.Width, r.Height);
	}
}
