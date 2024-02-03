using Monofoxe.Engine.Collisions.Algorithms;
using Monofoxe.Engine.Collisions.Shapes;
using Monofoxe.Engine.Utils;
using System.Runtime.CompilerServices;

namespace Monofoxe.Engine.Collisions
{
	public static class ShapeCollisionChecker
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool CheckCollision(IShape a, IShape b)
		{
			if (a.Type == ShapeType.Circle && b.Type == ShapeType.Circle)
			{
				// AABB check for this case is kinda redundant.
				var circle1 = (Circle)a;
				var circle2 = (Circle)b;
				return SimpleCollisionDetection.CheckCircleCircle(circle1.Position, circle1.Radius, circle2.Position, circle2.Radius);
			}

			var aabb1 = a.GetBoundingBox();
			var aabb2 = b.GetBoundingBox();
			if (!AABB.TestOverlap(ref aabb2, ref aabb1))
			{
				return false;
			}

			switch (a.Type)
			{
				case ShapeType.Circle:
					switch (b.Type)
					{
						// No circle-circle case because it is handled in the beginning.
						case ShapeType.Polygon:
							var circle = (Circle)a;
							var poly = (Polygon)b;
							return GJK.CheckCirclePoly(circle.Position, circle.Radius, poly.Vertices, poly.Count);
					}
					break;
				case ShapeType.Polygon:
					switch (b.Type)
					{
						case ShapeType.Circle:
							var circle = (Circle)b;
							var poly = (Polygon)a;
							return GJK.CheckCirclePoly(circle.Position, circle.Radius, poly.Vertices, poly.Count);
						case ShapeType.Polygon:
							var poly1 = (Polygon)a;
							var poly2 = (Polygon)b;
							return GJK.CheckPolyPoly(poly1.Vertices, poly1.Count, poly2.Vertices, poly2.Count);
					}
					break;
			}

			return false;
		}
	}
}
