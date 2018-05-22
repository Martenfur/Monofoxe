using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;

namespace Monofoxe.Utils
{
	/// <summary>
	/// Contains useful math stuff. 
	/// </summary>
	public static class GameMath
	{
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
		
		
	}
}
