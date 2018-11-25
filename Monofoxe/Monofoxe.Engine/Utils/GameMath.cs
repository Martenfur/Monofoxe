using System;
using Microsoft.Xna.Framework;

namespace Monofoxe.Engine.Utils
{
	/// <summary>
	/// Contains useful math stuff. 
	/// </summary>
	public static class GameMath
	{
		#region Angles and stuff.

		/// <summary>
		/// Calculates distance between two points.
		/// </summary>
		public static float Distance(Vector2 p1, Vector2 p2) =>
			(p2 - p1).Length();
		
		public static float Distance(float x1, float y1, float x2, float y2) =>
			(float)Math.Sqrt((x2 - x1) * (x2 - x1) + (y2 - y1) * (y2 - y1));

		public static float Distance(float x, float y) =>
			(float)Math.Sqrt(x * x + y * y);
		
		
		
		/// <summary>
		/// Calculates direction between two points in degrees.
		/// </summary>
		public static double Direction(Vector2 p1, Vector2 p2) =>
			DirectionRad(p2.X - p1.X, p2.Y - p1.Y) * 360 / (Math.PI * 2);
		
		public static double Direction(float x1, float y1, float x2, float y2) =>
			DirectionRad(x2 - x1, y2 - y1) * 360 / (Math.PI * 2);
		
		public static double Direction(Vector2 p) =>
			DirectionRad(p.X, p.Y) * 360 / (Math.PI * 2);
		
		public static double Direction(float x, float y) =>
			DirectionRad(x, y) * 360 / (Math.PI * 2);
		

		
		/// <summary>
		/// Calculates direction between two points in radians.
		/// </summary>
		public static double DirectionRad(Vector2 p1, Vector2 p2) =>
			DirectionRad(p2.X - p1.X, p2.Y - p1.X);

		public static double DirectionRad(float x1, float y1, float x2, float y2) =>
			DirectionRad(x2 - x1, y2 - y1);
		
		public static double DirectionRad(Vector2 p) =>
			DirectionRad(p.X, p.Y);

		public static double DirectionRad(float x, float y) =>
			(Math.Atan2(y, -x) + Math.PI) % (Math.PI * 2);
		
			

		/// <summary>
		/// Calculates difference between two angles from -180 to 180;
		/// </summary>
		public static double AngleDiff(double ang1, double ang2)
		{
			double diff = ang1 - ang2;
			
			if (diff > 180)
			{
				return diff - 360;
			}
			if (diff < -180)
			{
				return diff + 360;
			}

			return diff;
		}
		
		/// <summary>
		/// Calculates difference between two angles in radians from -pi to pi;
		/// </summary>
		public static double AngleDiffRad(double ang1, double ang2)
		{
			double diff = ang1 - ang2;
			
			if (diff > Math.PI)
			{
				return diff - Math.PI * 2;
			}
			if (diff < -Math.PI)
			{
				return diff + Math.PI * 2;
			}

			return diff;
		}

		#endregion Angles and stuff.


		#region Intersestions.

		/// <summary>
		/// Checks if a point lies within a rectangle.
		/// </summary>
		public static bool PointInRectangle(Vector2 point, Vector2 rectPoint1, Vector2 rectPoint2) =>	
			point.X >= rectPoint1.X && point.X <= rectPoint2.X && point.Y >= rectPoint1.Y && point.Y <= rectPoint2.Y;
		
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
			rect1Pt1.X < rect2Pt2.X && rect1Pt2.X > rect2Pt1.X && rect1Pt1.Y < rect2Pt2.Y && rect1Pt2.Y > rect2Pt1.Y;


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
	}
}
