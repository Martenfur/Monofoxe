using Microsoft.Xna.Framework;
using Monofoxe.Engine;
using Monofoxe.Engine.Collisions;
using Monofoxe.Engine.Collisions.Colliders;
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


		private CircleCollider _circleCollider1;
		private CircleCollider _circleCollider2;
		private Collider _polyCollider;
		private LineCollider _lineCollider;

		private Collider _polyCollider2;
		private RectangleCollider _rectCollider;

		private bool _circleCollidersCollided;
		private bool _lineAndPolyCollidersCollided;
		private bool _rectAndPolyCollidersCollided;

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
			// - The vertices should form a convex shape.
			// - The polygon vertices should have clockwise winding.
			// - Single polygon cannot have more than CollisionSettings.MaxPolygonVertices (which is 8 by default).
			_polygon.Add(new Vector2(-32, -32).ToMeters());
			_polygon.Add(new Vector2(32, -32).ToMeters());
			_polygon.Add(new Vector2(32, 32).ToMeters());
			_polygon.Add(new Vector2(32, 32).ToMeters());
			_polygon.Add(new Vector2(-32, 32).ToMeters());

			_polygon.Position = new Vector2(200, 100).ToMeters();
			// Setting up shapes. -----------------------------------------



			// Setting up colliders. -----------------------------------------

			// Colliders are essentially collections of shapes that act a single shape.
			// Because of that, they can be convex and have an unlimited number of vertices by internally splitting themselves into smaller convex parts.

			// Colliders are created using ColliderFactory. It already has a bunch of predefined shapes. 
			// Note that all these colliders are taken from ColliderPool and should be returned to it afterwards.
			_circleCollider1 = ColliderFactory.CreateCircle(32.ToMeters());
			_circleCollider2 = ColliderFactory.CreateCircle(32.ToMeters());

			_polyCollider = ColliderPool.GetCollider(); // Getting an empty collider directly form the pool.
			for (var i = 1; i <= 360 / 8; i += 1)
			{
				var triangle = ShapePool.GetPolygon();
				triangle.Add(Vector2.Zero);
				triangle.Add(new Angle(i * 8).ToVector2() * 80.ToMeters());
				triangle.Add(new Angle((i + 1) * 8).ToVector2() * 80.ToMeters());
				_polyCollider.AddShape(triangle);
			}
			_lineCollider = ColliderFactory.CreateLine(50.ToMeters());
			//ColliderFactory.CreatePolygon(verts);



			var verts = new List<Vector2>();

			// Colliders to not have restrictions to how many vertices they can have.
			// They also can be concave - they will be partitioned into simple shapes.
			verts.Add(new Vector2(0, 0).ToMeters());
			verts.Add(new Vector2(32, 0).ToMeters());
			verts.Add(new Vector2(32, 32).ToMeters());
			verts.Add(new Vector2(64, 32).ToMeters());
			verts.Add(new Vector2(64, 64).ToMeters());
			verts.Add(new Vector2(0, 64).ToMeters());


			_polyCollider2 = ColliderFactory.CreatePolygon(verts);

			_rectCollider = ColliderFactory.CreateRectangle(new Vector2(64, 64).ToMeters());
			// Setting up colliders. -----------------------------------------
		}



		public override void Update()
		{
			base.Update();

			// Updating shapes. -----------------------------------------
			_polygon.Rotation = (float)GameMgr.ElapsedTimeTotal * 32;
			_polygon.UpdateTransform(); // For polygon shapes, it is required to apply new transform after every change.

			_circle.Position = new Vector2(
				100 + 50 * MathF.Sin((float)GameMgr.ElapsedTimeTotal * 2),
				100
			).ToMeters();

			_circleAndPolygonCollided = CollisionChecker.CheckCollision(_circle, _polygon);
			// Updating shapes. -----------------------------------------



			// Updating circle colliders. -----------------------------------------			
			_circleCollider1.Position = new Vector2(
				150 + 100 * MathF.Sin((float)GameMgr.ElapsedTimeTotal * 2),
				250
			).ToMeters();
			_circleCollider1.Rotation = (float)GameMgr.ElapsedTimeTotal * 32;
			_circleCollider1.Origin = new Vector2(32, 0).ToMeters();
			_circleCollider1.UpdateTransform();

			_circleCollider2.Position = new Vector2(
				150 - 100 * MathF.Sin((float)GameMgr.ElapsedTimeTotal * 2),
				250
			).ToMeters();
			_circleCollider2.Radius = (32 + MathF.Sin((float)GameMgr.ElapsedTimeTotal * 4) * 8).ToMeters();
			_circleCollider2.UpdateTransform();

			_circleCollidersCollided = CollisionChecker.CheckCollision(_circleCollider1, _circleCollider2);
			// Updating circle colliders. -----------------------------------------



			// Updating colliders. -----------------------------------------
			_polyCollider.Position = new Vector2(550, 150).ToMeters();

			// You can morph vertices directly - however, make sure your geometry allows for transformations you want.
			for (var i = 0; i < _polyCollider.ShapesCount; i += 1)
			{
				var shape = (Polygon)_polyCollider.GetShape(i);
				for (var k = 0; k < shape.Count; k += 1)
				{
					var angle = new Angle(shape.RelativeVertices[k]).RadiansF;
					var e = shape.RelativeVertices[k].SafeNormalize();
					shape.RelativeVertices[k] = e * (80 + (MathF.Sin((float)GameMgr.ElapsedTimeTotal * 2) * 12) * MathF.Sin(angle * 8)).ToMeters();
				}
			}
			_polyCollider.UpdateTransform();

			_lineCollider.Position = new Vector2(370 + 50, 150 - 8).ToMeters();
			_lineCollider.UpdateTransform();
			_lineCollider.Length = 48.ToMeters();

			_lineAndPolyCollidersCollided = CollisionChecker.CheckCollision(_polyCollider, _lineCollider);
			// Updating colliders. -----------------------------------------



			// Updating colliders. -----------------------------------------
			_rectCollider.Position = new Vector2(100, 400).ToMeters();
			_rectCollider.Size = (Vector2.One * 64 + new Vector2(-8, 8) * MathF.Sin((float)GameMgr.ElapsedTimeTotal * 4)).ToMeters();
			_rectCollider.Origin = _rectCollider.Size * 0;
			_rectCollider.UpdateTransform();

			_polyCollider2.Position = new Vector2(200, 400).ToMeters();
			_polyCollider2.Rotation = (float)GameMgr.ElapsedTimeTotal * 32;
			_polyCollider2.UpdateTransform();

			_rectAndPolyCollidersCollided = CollisionChecker.CheckCollision(_polyCollider2, _rectCollider);
			// Updating colliders. -----------------------------------------
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
			if (_circleCollidersCollided)
			{
				GraphicsMgr.CurrentColor = Color.Red;
			}
			DrawCollider(_circleCollider1);
			DrawCollider(_circleCollider2);
			DrawAABB(_circleCollider1);
			DrawAABB(_circleCollider2);


			GraphicsMgr.CurrentColor = Color.Cyan;
			if (_lineAndPolyCollidersCollided)
			{
				GraphicsMgr.CurrentColor = Color.Red;
			}
			DrawCollider(_polyCollider);
			DrawCollider(_lineCollider);
			DrawAABB(_polyCollider);


			GraphicsMgr.CurrentColor = Color.ForestGreen;
			if (_rectAndPolyCollidersCollided)
			{
				GraphicsMgr.CurrentColor = Color.Red;
			}
			DrawCollider(_polyCollider2);
			DrawCollider(_rectCollider);
			DrawAABB(_polyCollider2);
		}


		public override void Destroy()
		{
			base.Destroy();

			// Returning used shapes and colliders back to the pool.
			ShapePool.Return(_circle);
			ShapePool.Return(_polygon);

			ColliderPool.Return(_circleCollider1);
			ColliderPool.Return(_circleCollider2);
			ColliderPool.Return(_polyCollider);
			ColliderPool.Return(_lineCollider);
			ColliderPool.Return(_polyCollider2);
			ColliderPool.Return(_rectCollider);
		}


		private void DrawCollider(Collider collider)
		{
			for (var i = 0; i < collider.ShapesCount; i += 1)
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
			CircleShape.Draw(collider.Position.ToPixels(), 4, ShapeFill.Outline);
		}


		private void DrawCircle(Circle circle)
		{
			CircleShape.Draw(circle.Position.ToPixels(), circle.Radius.ToPixels(), ShapeFill.Outline);
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
			RectangleShape.Draw(aabb.BottomRight.ToPixels(), aabb.TopLeft.ToPixels(), ShapeFill.Outline);
		}

		private void DrawAABB(Collider collider)
		{
			GraphicsMgr.CurrentColor = Color.Gray;
			var aabb = collider.GetBoundingBox();
			RectangleShape.Draw(aabb.BottomRight.ToPixels(), aabb.TopLeft.ToPixels(), ShapeFill.Outline);
		}
	}
}
