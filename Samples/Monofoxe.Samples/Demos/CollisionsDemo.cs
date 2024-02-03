using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Monofoxe.Engine.Collisions;
using Monofoxe.Engine;
using Monofoxe.Engine.Cameras;
using Monofoxe.Engine.Drawing;
using Monofoxe.Engine.EC;
using Monofoxe.Engine.Resources;
using Monofoxe.Engine.SceneSystem;
using Monofoxe.Engine.Utils;
using Monofoxe.Engine.Collisions.Shapes;
using System;
using System.Collections.Generic;

namespace Monofoxe.Samples.Demos
{
	public class CollisionsDemo : Entity
	{
		private Collider _circleCollider1;
		private Collider _circleCollider2;

		private Circle _circle;
		private Polygon _squarePolygon;
		private bool _circleAndSquarePolygonCollided;
		private bool _collidersCollided;
		private Collider _polyCollider;
		private Collider _rectCollider;

		public CollisionsDemo(Layer layer) : base(layer)
		{
			Settings.WorldScale = 16;

			_circle = ShapePool.GetCircle();
			// All collider measuremenets must be converted to meters, which is basically 1 / WorldScale.
			// This is done for performance reasons. Collision detection works best with smaller shapes.
			_circle.Radius = 3.ToMeters();

			_squarePolygon = ShapePool.GetPolygon();
			// Note that we don't add vertices directly to the RelativeVertices array. 
			// It is possible to do that, however, you also must increment Count by 1.
			_squarePolygon.Add(new Vector2(-32, -32).ToMeters());
			_squarePolygon.Add(new Vector2(32, -32).ToMeters() * 3);
			_squarePolygon.Add(new Vector2(32, 32).ToMeters());
			_squarePolygon.Add(new Vector2(-32, 32).ToMeters());

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
		}

		

		public override void Update()
		{
			base.Update();

			_rectCollider.Position = new Vector2(500, 400).ToMeters();
			_rectCollider.Rotation = (float)GameMgr.ElapsedTimeTotal * 32;
			_rectCollider.UpdateTransform();

			_squarePolygon.Position = new Vector2(200, 100).ToMeters();
			_squarePolygon.Rotation = (float)GameMgr.ElapsedTimeTotal * 32;
			_squarePolygon.UpdateTransform(); // Generating the correctly translated vertices.
			
			_circleCollider1.Origin = new Vector2(32, 0).ToMeters();
			_circleCollider2.Origin = new Vector2(0, -64).ToMeters();
			_circleCollider1.Rotation = (float)GameMgr.ElapsedTimeTotal * 32;
			
			_circle.Position = new Vector2(
				100 + 100 * MathF.Sin((float)GameMgr.ElapsedTimeTotal * 2), 
				100
			).ToMeters();

			_circle.Position = GameController.MainCamera.GetRelativeMousePosition().ToMeters();

			_circleAndSquarePolygonCollided = ShapeCollisionChecker.CheckCollision(_circle, _squarePolygon);

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

			GraphicsMgr.CurrentColor = Color.White;
			if (_circleAndSquarePolygonCollided)
			{
				GraphicsMgr.CurrentColor = Color.Red;
			}

			DrawCircle(_circle);
			DrawPolygon(_squarePolygon);

			DrawAABB(_circle);
			DrawAABB(_squarePolygon);


			GraphicsMgr.CurrentColor = Color.White;
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
			ShapePool.Return(_squarePolygon);
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

			for (var i = 0; i < poly.Count; i += 1)
			{
				CircleShape.Draw(poly.Vertices[i].ToPixels(), i * 2, false);
			}
		}

		private void DrawAABB(IShape shape)
		{
			GraphicsMgr.CurrentColor = Color.Gray;
			var aabb = shape.GetBoundingBox();
			RectangleShape.Draw(aabb.BottomRight.ToPixels(), aabb.TopLeft.ToPixels(), true);
			GraphicsMgr.CurrentColor = Color.Red;
			CircleShape.Draw(aabb.BottomRight.ToPixels(), 3, true);

			GraphicsMgr.CurrentColor = Color.Green;
			CircleShape.Draw(aabb.TopLeft.ToPixels(), 3, true);
		}

	}
}
