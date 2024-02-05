using Microsoft.Xna.Framework;
using Monofoxe.Engine.Collisions.Algorithms;
using Monofoxe.Engine.Collisions.Colliders;
using System.Collections.Generic;

namespace Monofoxe.Engine.Collisions
{
	public static class ColliderFactory
	{
		/// <summary>
		/// Creates a free-form polygon with clockwise winding. The polygon can be convex or concave and has no limit on the amount of vertices.
		/// </summary>
		public static Collider CreatePolygon(List<Vector2> vertices)
		{
			var collider = ColliderPool.GetCollider();

			var polys = BayazitDecomposer.ConvexPartition(vertices);

			for (var i = 0; i < polys.Count; i += 1)
			{
				var poly = ShapePool.GetPolygon();

				for (var k = 0; k < polys[i].Count; k += 1)
				{
					poly.Add(polys[i][k]);
				}

				collider.AddShape(poly);
			}

			return collider;
		}


		/// <summary>
		/// Creates a perfect circle.
		/// </summary>
		public static CircleCollider CreateCircle(float r)
		{
			var collider = ColliderPool.GetCircleCollider();

			var circle = ShapePool.GetCircle();
			circle.Radius = r;
			collider.AddShape(circle);

			return collider;
		}


		/// <summary>
		/// Creates a rectangle with its center inthe middle, consisting of four vertices that form the following shape:
		/// 0---1
		/// | \ |
		/// 3---2
		/// </summary>
		public static RectangleCollider CreateRectangle(Vector2 size)
		{
			var collider = ColliderPool.GetRectangleCollider();
			var poly = ShapePool.GetPolygon();

			var halfSize = size / 2;

			// 0---1
			// | \ |
			// 3---2
			poly.Add(new Vector2(-halfSize.X, -halfSize.Y));
			poly.Add(new Vector2( halfSize.X, -halfSize.Y));
			poly.Add(new Vector2( halfSize.X,  halfSize.Y));
			poly.Add(new Vector2(-halfSize.X,  halfSize.Y));

			collider.AddShape(poly);

			return collider;
		}


		/// <summary>
		/// Creates a simple line that consists of two vertices that form the following shape with its center at vertex 0:
		/// 0---1
		/// </summary>
		public static LineCollider CreateLine(float length)
		{
			var collider = ColliderPool.GetLineCollider();
			var poly = ShapePool.GetPolygon();

			poly.Add(new Vector2(0, 0));
			poly.Add(new Vector2(length, 0));

			collider.AddShape(poly);

			return collider;
		}
	}
}
