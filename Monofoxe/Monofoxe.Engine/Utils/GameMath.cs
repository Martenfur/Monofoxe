using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;

namespace Monofoxe.Engine.Utils
{
	/// <summary>
	/// Contains useful math stuff. 
	/// </summary>
	public static class GameMath
	{
		#region Math.

		public static double Lerp(double a, double b, double value) =>
			a + (b - a) * value;


		#endregion Math.


		#region Distance.

		/// <summary>
		/// Calculates distance between two points.
		/// </summary>
		public static float Distance(Vector2 p1, Vector2 p2) =>
			(p2 - p1).Length();
		
		#endregion Distance.



		#region Intersestions.

		/// <summary>
		/// Checks if a point lies within a rectangle.
		/// </summary>
		public static bool PointInRectangle(Vector2 point, Vector2 rectPoint1, Vector2 rectPoint2) =>	
			point.X >= rectPoint1.X && point.X <= rectPoint2.X && point.Y >= rectPoint1.Y && point.Y <= rectPoint2.Y;
		
		/// <summary>
		/// Checks if a point lies within a rectangle.
		/// </summary>
		public static bool PointInRectangleBySize(Vector2 point, Vector2 rectCenter, Vector2 rectSize)
		{
			var rectHalfSize = rectSize / 2f;
			var pt1 = rectCenter - rectHalfSize;
			var pt2 = rectCenter + rectHalfSize;
			return point.X >= pt1.X && point.X <= pt2.X && point.Y >= pt1.Y && point.Y <= pt2.Y;
		}
		

		/// <summary>
		/// Checks if a point lies within a triangle.
		/// </summary>
		public static bool PointInTriangle(Vector2 point, Vector2 triPoint1, Vector2 triPoint2, Vector2 triPoint3)
		{
			Vector2 p = point - triPoint1;
			Vector2 v2 = triPoint2 - triPoint1;
			Vector2 v3 = triPoint3 - triPoint1;
			
			float w1 = (triPoint1.X * v3.Y - point.X * v3.Y + p.Y * v3.X) / (v2.Y * v3.X - v2.X * v3.Y); 
			float w2 = (point.Y - triPoint1.Y - w1 * v2.Y) / v3.Y;
			
			return (w1 >= 0 && w2 >= 0 && ((w1 + w2) <= 1));
		}

		/// <summary>
		/// Checks if two rectangles intersect.
		/// </summary>
		public static bool RectangleInRectangle(Vector2 rect1Pt1, Vector2 rect1Pt2, Vector2 rect2Pt1, Vector2 rect2Pt2) =>
			rect1Pt1.X <= rect2Pt2.X && rect1Pt2.X >= rect2Pt1.X && rect1Pt1.Y <= rect2Pt2.Y && rect1Pt2.Y >= rect2Pt1.Y;
		
		/// <summary>
		/// Checks if two rectangles intersect.
		/// </summary>
		public static bool RectangleInRectangleBySize(Vector2 rect1Center, Vector2 rect1Size, Vector2 rect2Center, Vector2 rect2Size)
		{
			var delta = rect2Center - rect1Center;
			var size = (rect2Size + rect1Size) / 2f; 

			return Math.Abs(delta.X) <= size.X && Math.Abs(delta.Y) <= size.Y; 
		}

		/// <summary>
		/// Calculates on which side point is from a line. 
		/// 1 - left
		/// -1 - right
		/// 0 - on the line
		/// </summary>
		public static int PointSide(Vector2 point, Vector2 linePt1, Vector2 linePt2)
		{
			var v = new Vector2(linePt2.Y - linePt1.Y, linePt1.X - linePt2.X);

			return Math.Sign(Vector2.Dot(point - linePt1, v));
		}
		
		/// <summary>
		/// Checks if two lines cross. Returns 1 if lines cross, 0 if not and 2 if lines overlap.
		/// </summary>
		public static int LinesCross(Vector2 line1Pt1, Vector2 line1Pt2, Vector2 line2Pt1, Vector2 line2Pt2)
		{
			var line1 = new Vector2(line1Pt2.Y - line1Pt1.Y, line1Pt1.X - line1Pt2.X);
			var line2 = new Vector2(line2Pt2.Y - line2Pt1.Y, line2Pt1.X - line2Pt2.X);
			
			var side1 = Math.Sign(Vector2.Dot(line2Pt1 - line1Pt1, line1));
			var side2 = Math.Sign(Vector2.Dot(line2Pt2 - line1Pt1, line1));
			var side3 = Math.Sign(Vector2.Dot(line1Pt1 - line2Pt1, line2));
			var side4 = Math.Sign(Vector2.Dot(line1Pt2 - line2Pt1, line2));

			if (side1 != side2 && side3 != side4)
			{
				return 1;
			}

			if (side1 == 0 && side2 == 0)
			{
				return 2;
			}
						
			return 0;
		}

		
		/// <summary>
		/// Checks if two linew cross. Returns 1 if lines cross, 0 if not and 2 if lines overlap.
		/// Also calculates intersection point.
		/// </summary>
		public static int LinesCross(Vector2 line1Pt1, Vector2 line1Pt2, Vector2 line2Pt1, Vector2 line2Pt2, ref Vector2 collisionPt)
		{
			int result = LinesCross(line1Pt1, line1Pt2, line2Pt1, line2Pt2);
			if (result == 1)
			{
				Vector2 e1 = line1Pt2 - line1Pt1;
				e1.Normalize();
				Vector2 e2 = line2Pt2 - line2Pt1;
				e2.Normalize();

				// A bit of unpleasant math.
				// The base idea is:
				// e1 * l1 + v1 = e2 * l2 + v2
				// So we need to calculate l1 or l2 and plop it into vector function.

				// Formula will probably break with parallel lines.
				// (result == 1) prevents it, but just keep this in mind.
				float l = (e1.X * (line1Pt1.Y - line2Pt1.Y) + e1.Y * (line2Pt1.X - line1Pt1.X)) / (e1.X * e2.Y - e2.X * e1.Y);

				collisionPt = line2Pt1 + e2 * l;
			}
			return result;
		}

		#endregion Intersections.



		/// <summary>
		/// Indicates if the vertices are in clockwise order.
		/// Warning: If the area of the polygon is 0, it is unable to determine the winding.
		/// </summary>
		public static bool IsClockWise(List<Vector2> vertices)
		{
			//The simplest polygon which can exist in the Euclidean plane has 3 sides.
			if (vertices.Count < 3)
				return false;

			return (GetSignedArea(vertices) > 0.0f);
		}


		/// <summary>
		/// Gets the signed area.
		/// If the area is less than 0, it indicates that the polygon is couter clockwise winded.
		/// </summary>
		/// <returns>The signed area</returns>
		public static float GetSignedArea(List<Vector2> vertices)
		{
			//The simplest polygon which can exist in the Euclidean plane has 3 sides.
			if (vertices.Count < 3)
				return 0;

			int i;
			float area = 0;

			for (i = 0; i < vertices.Count; i++)
			{
				int j = (i + 1) % vertices.Count;

				Vector2 vi = vertices[i];
				Vector2 vj = vertices[j];

				area += vi.X * vj.Y;
				area -= vi.Y * vj.X;
			}
			area /= 2.0f;
			return area;
		}


		/// <summary>
		/// Gets the area.
		/// </summary>
		/// <returns></returns>
		public static float GetArea(List<Vector2> vertices)
		{
			float area = GetSignedArea(vertices);
			return (area < 0 ? -area : area);
		}



		#region Bezier curve stuff.
		// A look up table for factorials. Capped to 16.
		private static float[] Factorial = new float[]
		{
			1.0f,
			1.0f,
			2.0f,
			6.0f,
			24.0f,
			120.0f,
			720.0f,
			5040.0f,
			40320.0f,
			362880.0f,
			3628800.0f,
			39916800.0f,
			479001600.0f,
			6227020800.0f,
			87178291200.0f,
			1307674368000.0f,
			20922789888000.0f,
		};

		private static float Binomial(int n, int i)
		{
			float ni;
			float a1 = Factorial[n];
			float a2 = Factorial[i];
			float a3 = Factorial[n - i];

			ni = a1 / (a2 * a3);

			return ni;
		}

		private static float Bernstein(int n, int i, float value)
		{
			float value_i = MathF.Pow(value, i);
			float value_n_minus_i = MathF.Pow((1 - value), (n - i));

			float basis = Binomial(n, i) * value_i * value_n_minus_i;

			return basis;
		}
		#endregion


		/// <summary>
		/// Calculates bezier curve.
		/// </summary>
		/// <param name="value">Should be in 0..1 range.</param>
		public static Vector2 BezierCurve(Vector2[] controlPoints, float value)
		{
			int N = controlPoints.Length - 1;

			if (N > 16)
			{
				throw new Exception("The maximum control points allowed is 16.");
			}


			if (value <= 0)
			{
				return controlPoints[0];
			}
			if (value >= 1)
			{
				return controlPoints[controlPoints.Length - 1];
			}


			Vector2 pointOnCurve = new Vector2();

			for (int i = 0; i < controlPoints.Length; ++i)
			{
				Vector2 bn = Bernstein(N, i, value) * controlPoints[i];

				pointOnCurve += bn;
			}

			return pointOnCurve;
		}


		/// <summary>
		/// Returns an array of points spaced with passed interval.
		/// </summary>
		public static Vector2[] BezierCurvePoints(Vector2[] controlPoints, float interval = 0.01f)
		{
			int N = controlPoints.Length - 1;

			if (N > 16)
			{
				throw new Exception("The maximum control points allowed is 16.");
			}


			List<Vector2> points = new List<Vector2>();

			for (float t = 0.0f; t <= 1.0f + interval - 0.0001f; t += interval)
			{
				Vector2 p = new Vector2();

				for (int i = 0; i < controlPoints.Length; ++i)
				{
					Vector2 bn = Bernstein(N, i, t) * controlPoints[i];

					p += bn;
				}

				points.Add(p);
			}

			return points.ToArray();
		}


		/// <summary>
		/// Returns a projection aligned to the Y axis.
		/// </summary>
		public static Vector2 VerticalAxisProjection(Vector2 p1, Vector2 p2, Vector2 pp)
		{
			if (p1.X == p2.X)
			{
				return pp;
			}

			var d = p2 - p1;
			var e = d.SafeNormalize();

			var l = (pp.X - p2.X) / e.X;

			return p2 + e * l;
		}
	}
}
