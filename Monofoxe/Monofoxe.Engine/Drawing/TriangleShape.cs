using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Monofoxe.Engine.Drawing
{
	public class TriangleShape : IDrawable
	{
		public Vector2 Position {get; set;}

		/// <summary>
		/// First triangle point. 
		/// NOTE: all triangle points treat position as an origin point;
		/// </summary>
		public Vector2 Point1;

		/// <summary>
		/// Second triangle point. 
		/// NOTE: all triangle points treat position as an origin point;
		/// </summary>
		public Vector2 Point2;

		/// <summary>
		/// Third triangle point. 
		/// NOTE: all triangle points treat position as an origin point;
		/// </summary>
		public Vector2 Point3;

		public bool IsOutline;

		public TriangleShape(Vector2 position, Vector2 point1, Vector2 point2, Vector2 point3, bool isOutline = false)
		{
			Position = position;
			Point1 = point1;
			Point2 = point2;
			Point3 = point3;

			IsOutline = isOutline;
		}

		public void Draw() =>
			Draw(Point1 + Position, Point2 + Position, Point3 + Position, IsOutline);
		
		
		
		private static readonly short[][] _triangleIndices = 
		{
			new short[]{0, 1, 2}, // Filled.
			new short[]{0, 1, 1, 2, 2, 0} // Outline.
		};
		

		/// <summary>
		/// Draws a triangle.
		/// </summary>
		public static void Draw(Vector2 p1, Vector2 p2, Vector2 p3, bool isOutline) =>
			Draw(p1.X, p1.Y, p2.X, p2.Y, p3.X, p3.Y, isOutline, DrawMgr.CurrentColor, DrawMgr.CurrentColor, DrawMgr.CurrentColor);
		
		/// <summary>
		/// Draws a triangle with specified colors.
		/// </summary>
		public static void Draw(Vector2 p1, Vector2 p2, Vector2 p3, bool isOutline, Color c1, Color c2, Color c3) =>
			Draw(p1.X, p1.Y, p2.X, p2.Y, p3.X, p3.Y, isOutline, c1, c2, c3);
		
		/// <summary>
		/// Draws a triangle.
		/// </summary>
		public static void Draw(float x1, float y1, float x2, float y2, float x3, float y3, bool isOutline) =>
			Draw(x1, y1, x2, y2, x3, y3, isOutline, DrawMgr.CurrentColor, DrawMgr.CurrentColor, DrawMgr.CurrentColor);
			
		/// <summary>
		/// Draw a triangle with specified colors.
		/// </summary>
		public static void Draw(float x1, float y1, float x2, float y2, float x3, float y3, bool isOutline, Color c1, Color c2, Color c3)
		{
			GraphicsMode mode;
			short[] indices;
			if (isOutline)
			{
				mode = GraphicsMode.OutlinePrimitives;
				indices = _triangleIndices[0];
			}
			else
			{
				mode = GraphicsMode.TrianglePrimitives;
				indices = _triangleIndices[1];
			}
		
			var vertices = new List<VertexPositionColorTexture>
			{
				new VertexPositionColorTexture(new Vector3(x1, y1, 0), c1, Vector2.Zero),
				new VertexPositionColorTexture(new Vector3(x2, y2, 0), c2, Vector2.Zero),
				new VertexPositionColorTexture(new Vector3(x3, y3, 0), c3, Vector2.Zero)
			};

			DrawMgr.AddVertices(mode, null, vertices, indices);
		}
		
	}
}
