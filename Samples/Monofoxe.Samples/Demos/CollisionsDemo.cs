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

namespace Monofoxe.Samples.Demos
{
	public class CollisionsDemo : Entity
	{
		private Circle _circle;
		private Polygon _squarePolygon;
		private bool _circleAndSquarePolygonCollided;

		public CollisionsDemo(Layer layer) : base(layer)
		{
			Settings.WorldScale = 16;

			_circle = ShapePool.GetCircle();
			// All collider measuremenets must be converted to meters, which is basically 1 / WorldScale.
			// This is done for performance reasons. Collision detection works best with smaller shapes.
			_circle.Radius = 30.ToMeters();

			_squarePolygon = ShapePool.GetPolygon();
			// Note that we don't add vertices directly to the RelativeVertices array. 
			// It is possible to do that, however, you also must increment Count by 1.
			_squarePolygon.Add(new Vector2(-32, -32).ToMeters());
			_squarePolygon.Add(new Vector2(-32, 32).ToMeters());
			_squarePolygon.Add(new Vector2(32, 32).ToMeters());
			_squarePolygon.Add(new Vector2(32, -32).ToMeters() * 3);
		}

		

		public override void Update()
		{
			base.Update();

			_squarePolygon.Position = new Vector2(200, 100).ToMeters();
			_squarePolygon.Rotation = (float)GameMgr.ElapsedTimeTotal * 32;
			_squarePolygon.UpdateTransform(); // Generating the correctly translated vertices.

			_circle.Position = new Vector2(
				100 + 100 * MathF.Sin((float)GameMgr.ElapsedTimeTotal * 2), 
				100
			).ToMeters();

			_circle.Position = GameController.MainCamera.GetRelativeMousePosition().ToMeters();

			_circleAndSquarePolygonCollided = CollisionChecker.CheckCollision(_circle, _squarePolygon);
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
		}

	
		public override void Destroy()
		{
			base.Destroy();

			// Returning used shapes back to the pool.
			ShapePool.Return(_circle);
			ShapePool.Return(_squarePolygon);
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
			GraphicsMgr.CurrentColor = Color.Red;
			CircleShape.Draw(aabb.BottomRight.ToPixels(), 3, true);

			GraphicsMgr.CurrentColor = Color.Green;
			CircleShape.Draw(aabb.TopLeft.ToPixels(), 3, true);
		}

	}
}
