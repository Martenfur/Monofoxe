using Microsoft.Xna.Framework;
using Monofoxe.Engine;
using Monofoxe.Engine.Collisions;
using Monofoxe.Engine.Collisions.Shapes;
using Monofoxe.Engine.Drawing;
using Monofoxe.Engine.EC;
using Monofoxe.Engine.SceneSystem;
using Monofoxe.Engine.Utils;
using System;
using System.Collections.Generic;

namespace Monofoxe.Samples.Demos
{
	public class CollisionsDemo : Entity
	{
		private Circle _circle;
		private Polygon _polygon;
		private bool _circleAndPolygonCollided;


		private Collider _circleCollider1;
		private Collider _circleCollider2;
		private Collider _polyCollider;
		private Collider _rectCollider;

		private bool _collidersCollided;

		public CollisionsDemo(Layer layer) : base(layer)
		{
			CollisionSettings.WorldScale = 64;

			// Setting up shapes. -----------------------------------------

			// Shapes are the most basic unit you can use for collision detection.
			// However, they have more restrictions than colliders, so in most cases, it is recommended to just use those instead.
			// Currently, there are only two types of shapes - circle and polygon.

			_circle = ShapePool.GetCircle(); // Instead of creating an instance directly, we take it from the shape pool. This prevents GC overhead if you require creating and destroying many shapes.

			// All collision system measuremenets must be converted to meters, which is basically 1 / WorldScale.
			// This is done for performance reasons. Collision detection works best with smaller shapes.
			_circle.Radius = 32.ToMeters();

			_polygon = ShapePool.GetPolygon();
			// Note that we don't add vertices directly to the RelativeVertices array. 
			// It is possible to do that, however, you also must increment Count by 1.
			// There are some restrictions to the polygon shape:
			// - The vrtices should form a convex shape.
			// - The polygon vertices should have clockwise winding.
			// - Single polygon cannot have more than CollisionSettings.MaxPolygonVertices (which is 8 by default).
			_polygon.Add(new Vector2(-32, -32).ToMeters());
			_polygon.Add(new Vector2(32, -32).ToMeters());
			_polygon.Add(new Vector2(32, 32).ToMeters());
			_polygon.Add(new Vector2(-32, 32).ToMeters());

			_polygon.Position = new Vector2(200, 100).ToMeters();
			// Setting up shapes. -----------------------------------------



			// Setting up colliders. -----------------------------------------

			// Colliders are essentially collections of shapes that act a single shape.
			// Because of that, they can be convex and have an unlimited number of vertices by internally splitting themselves into smaller convex parts.

			// Colliders are created using ColliderFactory. It already has a bunch of predefined shapes.
			_circleCollider1 = ColliderFactory.CreateCircle(32.ToMeters());
			_circleCollider2 = ColliderFactory.CreateCircle(64.ToMeters());


			var verts = new List<Vector2>();
			for(var i = 0; i < 8; i += 1)
			{
				verts.Add(new Angle(360 / 8f * i).ToVector2() * (80 + MathF.Cos(i * 0.4f) * 6).ToMeters());
			}

			_polyCollider = ColliderFactory.CreatePolygon(verts);
			_rectCollider = ColliderFactory.CreateRectangle(new Vector2(32, 32).ToMeters());
			_rectCollider.Origin = new Vector2(-16, -16).ToMeters();
			// Setting up colliders. -----------------------------------------
		}

		

		public override void Update()
		{
			base.Update();

			// Updating colliders. -----------------------------------------
			_polygon.Rotation = (float)GameMgr.ElapsedTimeTotal * 32;
			_polygon.UpdateTransform(); // For polygon shapes, it is required to apply new transform after every change.
			
			_circle.Position = new Vector2(
				100 + 50 * MathF.Sin((float)GameMgr.ElapsedTimeTotal * 2), 
				100
			).ToMeters();
			
			_circleAndPolygonCollided = CollisionChecker.CheckCollision(_circle, _polygon);
			// Updating colliders. -----------------------------------------



			_rectCollider.Position = new Vector2(500, 400).ToMeters();
			_rectCollider.Rotation = (float)GameMgr.ElapsedTimeTotal * 32;
			_rectCollider.UpdateTransform();

			_circleCollider1.Origin = new Vector2(32, 0).ToMeters();
			_circleCollider2.Origin = new Vector2(0, -64).ToMeters();
			_circleCollider1.Rotation = (float)GameMgr.ElapsedTimeTotal * 32;
			
			_circleCollider1.Position = new Vector2(
				100 + 100 * MathF.Sin((float)GameMgr.ElapsedTimeTotal * 2),
				250
			).ToMeters();
			_circleCollider1.UpdateTransform();
			_circleCollider2.Position = new Vector2(
				100 - 100 * MathF.Sin((float)GameMgr.ElapsedTimeTotal * 2),
				250
			).ToMeters();
			_circleCollider2.UpdateTransform();

			_polyCollider.Position = new Vector2(500, 300).ToMeters();
			_polyCollider.Rotation = (float)GameMgr.ElapsedTimeTotal * 32;
			_polyCollider.UpdateTransform();

			_collidersCollided = CollisionChecker.CheckCollision(_circleCollider1, _circleCollider2);
		}


		public override void Draw()
		{
			base.Draw();

			GraphicsMgr.CurrentColor = Color.CornflowerBlue;
			if (_circleAndPolygonCollided)
			{
				GraphicsMgr.CurrentColor = Color.Red;
			}

			DrawCircle(_circle);
			DrawPolygon(_polygon);

			DrawAABB(_circle);
			DrawAABB(_polygon);


			GraphicsMgr.CurrentColor = Color.DeepPink;
			if (_collidersCollided)
			{
				GraphicsMgr.CurrentColor = Color.Red;
			}
			DrawCollider(_circleCollider1);
			DrawCollider(_circleCollider2);


			//GraphicsMgr.CurrentColor = Color.White;
			DrawCollider(_polyCollider);

			DrawCollider(_rectCollider);
		}

	
		public override void Destroy()
		{
			base.Destroy();

			// Returning used shapes back to the pool.
			ShapePool.Return(_circle);
			ShapePool.Return(_polygon);
		}


		private void DrawCollider(Collider collider)
		{
			for(var i = 0; i < collider.ShapesCount; i += 1)
			{
				var shape = collider.GetShape(i);
				if (shape.Type == ShapeType.Circle)
				{
					DrawCircle((Circle)shape);
				}
				if (shape.Type == ShapeType.Polygon)
				{
					DrawPolygon((Polygon)shape);
				}
			}
			CircleShape.Draw(collider.Position.ToPixels(), 4, true);
		}


		private void DrawCircle(Circle circle)
		{
			CircleShape.Draw(circle.Position.ToPixels(), circle.Radius.ToPixels(), true);
		}

		private void DrawPolygon(Polygon poly)
		{
			for (var i = 0; i < poly.Count - 1; i += 1)
			{
				LineShape.Draw(poly.Vertices[i].ToPixels(), poly.Vertices[i + 1].ToPixels());
			}
			LineShape.Draw(poly.Vertices[0].ToPixels(), poly.Vertices[poly.Count - 1].ToPixels());
		}

		private void DrawAABB(IShape shape)
		{
			GraphicsMgr.CurrentColor = Color.Gray;
			var aabb = shape.GetBoundingBox();
			RectangleShape.Draw(aabb.BottomRight.ToPixels(), aabb.TopLeft.ToPixels(), true);
		}

	}
}
