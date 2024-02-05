using Microsoft.Xna.Framework;
using System.Diagnostics.CodeAnalysis;

namespace Monofoxe.Engine.Utils
{
	/// <summary>
	/// An axis aligned bounding box.
	/// </summary>
	public struct AABB
	{
		/// <summary>
		/// The lower vertex
		/// </summary>
		public Vector2 TopLeft;

		/// <summary>
		/// The upper vertex
		/// </summary>
		public Vector2 BottomRight;

		public AABB(Vector2 min, Vector2 max)
			: this(ref min, ref max)
		{
		}

		public AABB(ref Vector2 min, ref Vector2 max)
		{
			TopLeft = min;
			BottomRight = max;
		}

		public AABB(Vector2 center, float width, float height)
		{
			TopLeft = center - new Vector2(width / 2, height / 2);
			BottomRight = center + new Vector2(width / 2, height / 2);
		}


		public Vector2 Size => BottomRight - TopLeft;


		/// <summary>
		/// Get the center of the AABB.
		/// </summary>
		public Vector2 Center
		{
			get { return 0.5f * (TopLeft + BottomRight); }
		}

		/// <summary>
		/// Get the extents of the AABB (half-widths).
		/// </summary>
		public Vector2 Extents
		{
			get { return 0.5f * (BottomRight - TopLeft); }
		}


		/// <summary>
		/// Verify that the bounds are sorted.
		/// </summary>
		/// <returns>
		/// 	<c>true</c> if this instance is valid; otherwise, <c>false</c>.
		/// </returns>
		public bool IsValid()
		{
			Vector2 d = BottomRight - TopLeft;
			return d.X >= 0.0f && d.Y >= 0.0f;
		}

		/// <summary>
		/// Combine an AABB into this one.
		/// </summary>
		/// <param name="aabb">The aabb.</param>
		public void Combine(ref AABB aabb)
		{
			Vector2.Min(ref TopLeft, ref aabb.TopLeft, out TopLeft);
			Vector2.Max(ref BottomRight, ref aabb.BottomRight, out BottomRight);
		}

		/// <summary>
		/// Combine two AABBs into this one.
		/// </summary>
		/// <param name="aabb1">The aabb1.</param>
		/// <param name="aabb2">The aabb2.</param>
		public void Combine(ref AABB aabb1, ref AABB aabb2)
		{
			Vector2.Min(ref aabb1.TopLeft, ref aabb2.TopLeft, out TopLeft);
			Vector2.Max(ref aabb1.BottomRight, ref aabb2.BottomRight, out BottomRight);
		}

		/// <summary>
		/// Test if the two AABBs overlap.
		/// </summary>
		/// <param name="a">The first AABB.</param>
		/// <param name="b">The second AABB.</param>
		/// <returns>True if they are overlapping.</returns>
		public static bool TestOverlap(ref AABB a, ref AABB b)
		{
			if (b.TopLeft.X > a.BottomRight.X || b.TopLeft.Y > a.BottomRight.Y)
				return false;

			if (a.TopLeft.X > b.BottomRight.X || a.TopLeft.Y > b.BottomRight.Y)
				return false;

			return true;
		}
	}
}
