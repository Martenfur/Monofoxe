using Microsoft.Xna.Framework;

namespace Monofoxe.Engine.Collisions
{
	public static class UnitConversionExtensions
	{
		/// <summary>
		/// Converts pixels to meters which are used by the collision detection.
		/// </summary>
		public static Vector2 ToMeters(this Vector2 v) =>
			v * CollisionSettings.OneOverWorldScale;

		/// <summary>
		/// Converts meters to pixels which are used for rendering and everything else.
		/// </summary>
		public static Vector2 ToPixels(this Vector2 v) =>
			v * CollisionSettings.WorldScale;

		/// <summary>
		/// Converts pixels to meters which are used by the collision detection.
		/// </summary>
		public static float ToMeters(this int v) =>
			v * CollisionSettings.OneOverWorldScale;

		/// <summary>
		/// Converts meters to pixels which are used for rendering and everything else.
		/// </summary>
		public static float ToPixels(this int v) =>
			v * CollisionSettings.WorldScale;

		/// <summary>
		/// Converts pixels to meters which are used by the collision detection.
		/// </summary>
		public static float ToMeters(this float v) =>
			v * CollisionSettings.OneOverWorldScale;

		/// <summary>
		/// Converts meters to pixels which are used for rendering and everything else.
		/// </summary>
		public static float ToPixels(this float v) =>
			v * CollisionSettings.WorldScale;

		/// <summary>
		/// Converts pixels to meters which are used by the collision detection.
		/// </summary>
		public static double ToMeters(this double v) =>
			v * CollisionSettings.OneOverWorldScale;

		/// <summary>
		/// Converts meters to pixels which are used for rendering and everything else.
		/// </summary>
		public static double ToPixels(this double v) =>
			v * CollisionSettings.WorldScale;
	}
}
