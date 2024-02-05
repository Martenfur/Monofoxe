using Microsoft.Xna.Framework;
using System.Runtime.CompilerServices;


/// This code is a port of Kroitor's GJK implementation.
/// https://github.com/kroitor/gjk.c

namespace Monofoxe.Engine.Collisions.Algorithms
{
	internal static class GJK
	{
		//-----------------------------------------------------------------------------
		// Basic vector arithmetic operations
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		private static Vector2 Negate(Vector2 v) { v.X = -v.X; v.Y = -v.Y; return v; }
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		private static Vector2 Perpendicular(Vector2 v) { return new Vector2(v.Y, -v.X); }
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		private static float DotProduct(Vector2 a, Vector2 b) { return a.X * b.X + a.Y * b.Y; }
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		private static Vector2 SafeNormalize(Vector2 v)
		{
			if (v == Vector2.Zero)
			{
				return Vector2.Zero;
			}

			v.Normalize();
			return v;
		}


		//-----------------------------------------------------------------------------
		// Triple product expansion is used to calculate perpendicular normal vectors 
		// which kinda 'prefer' pointing towards the Origin in Minkowski space
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		private static Vector2 TripleProduct(Vector2 a, Vector2 b, Vector2 c)
		{
			Vector2 r;

			float ac = a.X * c.X + a.Y * c.Y; // perform a.dot(c)
			float bc = b.X * c.X + b.Y * c.Y; // perform b.dot(c)

			// perform b * a.dot(c) - a * b.dot(c)
			r.X = b.X * ac - a.X * bc;
			r.Y = b.Y * ac - a.Y * bc;
			return r;
		}


		//-----------------------------------------------------------------------------
		// This is to compute average center (roughly). It might be different from
		// Center of Gravity, especially for bodies with nonuniform density,
		// but this is ok as initial direction of simplex search in GJK.
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		private static Vector2 AveragePoint(Vector2[] vertices, int count)
		{
			Vector2 avg = new Vector2();
			for (int i = 0; i < count; i++)
			{
				avg.X += vertices[i].X;
				avg.Y += vertices[i].Y;
			}
			avg.X /= count;
			avg.Y /= count;
			return avg;
		}


		//-----------------------------------------------------------------------------
		// Get furthest vertex along a certain direction
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		private static int IndexOfFurthestPoint(Vector2[] vertices, int count, Vector2 d)
		{

			float maxProduct = DotProduct(d, vertices[0]);
			int index = 0;
			for (int i = 1; i < count; i++)
			{
				float product = DotProduct(d, vertices[i]);
				if (product > maxProduct)
				{
					maxProduct = product;
					index = i;
				}
			}
			return index;
		}


		//-----------------------------------------------------------------------------
		// Minkowski sum support function for GJK for two polygons.
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		private static Vector2 SupportPolyPoly(Vector2[] vertices1, int count1, Vector2[] vertices2, int count2, Vector2 d)
		{

			// get furthest point of first body along an arbitrary direction
			int i = IndexOfFurthestPoint(vertices1, count1, d);

			// get furthest point of second body along the opposite direction
			int j = IndexOfFurthestPoint(vertices2, count2, Negate(d));

			// subtract (Minkowski sum) the two points to see if bodies 'overlap'
			return vertices1[i] - vertices2[j];
		}


		//-----------------------------------------------------------------------------
		// Minkowski sum support function for GJK for a circle and a polygon.
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		private static Vector2 SupportCirclePoly(Vector2 center, float r, Vector2[] vertices2, int count2, Vector2 d)
		{
			// get furthest point of second body along the opposite direction
			int j = IndexOfFurthestPoint(vertices2, count2, Negate(d));

			// subtract (Minkowski sum) the two points to see if bodies 'overlap'
			return center + r * SafeNormalize(d) - vertices2[j];
		}


		//-----------------------------------------------------------------------------
		// The GJK yes/no test
		public static bool CheckPolyPoly(Vector2[] vertices1, int count1, Vector2[] vertices2, int count2)
		{
			int index = 0; // index of current vertex of simplex
			Vector2 a, b, c, d, ao, ab, ac, abperp, acperp;
			Vector2[] simplex = new Vector2[3];

			Vector2 position1 = AveragePoint(vertices1, count1); // not a CoG but
			Vector2 position2 = AveragePoint(vertices2, count2); // it's ok for GJK )

			// initial direction from the center of 1st body to the center of 2nd body
			d = position1 - position2;

			// if initial direction is zero – set it to any arbitrary axis (we choose X)
			if (d.X == 0 && d.Y == 0)
				d.X = 1f;

			// set the first support as initial point of the new simplex
			a = simplex[0] = SupportPolyPoly(vertices1, count1, vertices2, count2, d);

			if (DotProduct(a, d) <= 0)
				return false; // no collision

			d = Negate(a); // The next search direction is always towards the origin, so the next search direction is negate(a)

			var iteration = 0;

			while (true)
			{
				if (iteration > 1000)
				{
					// A failsafe to ensure that we don't end up in an infinite loop.
					break;
				}

				a = simplex[++index] = SupportPolyPoly(vertices1, count1, vertices2, count2, d);

				if (DotProduct(a, d) <= 0)
					return false; // no collision

				ao = Negate(a); // from point A to Origin is just negative A

				// simplex has 2 points (a line segment, not a triangle yet)
				if (index < 2)
				{
					b = simplex[0];
					ab = b - a; // from point A to B
					d = TripleProduct(ab, ao, ab); // normal to AB towards Origin
					if (d.LengthSquared() == 0)
						d = Perpendicular(ab);
					continue; // skip to next iteration
				}

				b = simplex[1];
				c = simplex[0];
				ab = b - a; // from point A to B
				ac = c - a; // from point A to C

				acperp = TripleProduct(ab, ac, ac);

				if (DotProduct(acperp, ao) >= 0)
				{
					d = acperp; // new direction is normal to AC towards Origin
				}
				else
				{

					abperp = TripleProduct(ac, ab, ab);

					if (DotProduct(abperp, ao) < 0)
						return true; // collision

					simplex[0] = simplex[1]; // swap first element (point C)

					d = abperp; // new direction is normal to AB towards Origin
				}

				simplex[1] = simplex[2]; // swap element in the middle (point B)
				--index;
			}

			return false;
		}


		//-----------------------------------------------------------------------------
		// The GJK yes/no test
		public static bool CheckCirclePoly(Vector2 center, float r, Vector2[] vertices2, int count2)
		{
			int index = 0; // index of current vertex of simplex
			Vector2 a, b, c, d, ao, ab, ac, abperp, acperp;
			Vector2[] simplex = new Vector2[3];

			Vector2 position1 = center; // not a CoG but
			Vector2 position2 = AveragePoint(vertices2, count2); // it's ok for GJK )

			// initial direction from the center of 1st body to the center of 2nd body
			d = position1 - position2;

			// if initial direction is zero – set it to any arbitrary axis (we choose X)
			if (d.X == 0 && d.Y == 0)
				d.X = 1f;

			// set the first support as initial point of the new simplex
			a = simplex[0] = SupportCirclePoly(center, r, vertices2, count2, d);

			if (DotProduct(a, d) <= 0)
				return false; // no collision

			d = Negate(a); // The next search direction is always towards the origin, so the next search direction is negate(a)

			var iteration = 0;

			while (true)
			{
				iteration += 1;
				if (iteration > 1000)
				{
					// A failsafe to ensure that we don't end up in an infinite loop.
					break;
				}

				a = simplex[++index] = SupportCirclePoly(center, r, vertices2, count2, d);

				if (DotProduct(a, d) <= 0)
					return false; // no collision

				ao = Negate(a); // from point A to Origin is just negative A

				// simplex has 2 points (a line segment, not a triangle yet)
				if (index < 2)
				{
					b = simplex[0];
					ab = b - a; // from point A to B
					d = TripleProduct(ab, ao, ab); // normal to AB towards Origin
					if (d.LengthSquared() == 0)
						d = Perpendicular(ab);
					continue; // skip to next iteration
				}

				b = simplex[1];
				c = simplex[0];
				ab = b - a; // from point A to B
				ac = c - a; // from point A to C

				acperp = TripleProduct(ab, ac, ac);

				if (DotProduct(acperp, ao) >= 0)
				{
					d = acperp; // new direction is normal to AC towards Origin
				}
				else
				{

					abperp = TripleProduct(ac, ab, ab);

					if (DotProduct(abperp, ao) < 0)
						return true; // collision

					simplex[0] = simplex[1]; // swap first element (point C)

					d = abperp; // new direction is normal to AB towards Origin
				}

				simplex[1] = simplex[2]; // swap element in the middle (point B)
				--index;
			}

			return false;
		}

	}
}
