using Microsoft.Xna.Framework;
using Monofoxe.Engine.Collisions.Algorithms;
using Monofoxe.Engine.Utils;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Monofoxe.Engine.Collisions
{
	public static class ColliderFactory
	{

		public static Collider CreateCircle(float r)
		{
			var collider = ColliderPool.GetCollider();

			var circle = ShapePool.GetCircle();
			circle.Radius = r;
			collider.AddShape(Vector2.Zero, circle);

			return collider;
		}


		public static Collider CreateRectangle(Vector2 size)
		{
			var collider = ColliderPool.GetCollider();
			var poly = ShapePool.GetPolygon();

			var halfSize = size / 2;

			// 0---1
			// | \ |
			// 3---2
			poly.Add(new Vector2(-halfSize.X, -halfSize.Y));
			poly.Add(new Vector2( halfSize.X, -halfSize.Y));
			poly.Add(new Vector2( halfSize.X,  halfSize.Y));
			poly.Add(new Vector2(-halfSize.X,  halfSize.Y));

			collider.AddShape(Vector2.Zero, poly);

			var isit = GameMath.IsClockWise(poly.RelativeVertices.ToList());
		
			return collider;
		}


		public static Collider CreateLine(float length)
		{
			throw new NotImplementedException();

		}


		public static Collider CreateRing(float arc, float r, float thickness)
		{
			throw new NotImplementedException();
		}


		public static Collider CreateSector(float arc, float r)
		{
			throw new NotImplementedException();
		}


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

				collider.AddShape(Vector2.Zero, poly);
			}

			return collider;
		}
	}
}
