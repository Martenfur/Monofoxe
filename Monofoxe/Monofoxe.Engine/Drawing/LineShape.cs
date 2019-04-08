using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Monofoxe.Engine.Drawing
{
	public class LineShape : IDrawable
	{
		public Vector2 Position {get; set;}

		/// <summary>
		/// First triangle point. 
		/// NOTE: all line points treat position as an origin point;
		/// </summary>
		public Vector2 Point1;

		/// <summary>
		/// Second triangle point. 
		/// NOTE: all line points treat position as an origin point;
		/// </summary>
		public Vector2 Point2;
		
		public Color Color;

		public LineShape(Vector2 position, Vector2 point1, Vector2 point2)
		{
			Position = position;
			Point1 = point1;
			Point2 = point2;
			Color = DrawMgr.CurrentColor;
		}

		public void Draw() =>
			Draw(Point1 + Position, Point2 + Position, Color, Color);	
		

		private static readonly short[] _lineIndices = new short[]{0, 1, 3, 1, 2, 3};

		
		/// <summary>
		/// Draws a line.
		/// </summary>
		public static void Draw(Vector2 p1, Vector2 p2) =>
			Draw(p1.X, p1.Y, p2.X, p2.Y, DrawMgr.CurrentColor, DrawMgr.CurrentColor);

		/// <summary>
		/// Draws a line with specified colors.
		/// </summary>
		public static void Draw(Vector2 p1, Vector2 p2, Color c1, Color c2) =>
			Draw(p1.X, p1.Y, p2.X, p2.Y, c1, c2);

		/// <summary>
		/// Draws a line.
		/// </summary>
		public static void Draw(float x1, float y1, float x2, float y2) =>
			Draw(x1, y1, x2, y2, DrawMgr.CurrentColor, DrawMgr.CurrentColor);
		
		/// <summary>
		/// Draws a line with specified colors.
		/// </summary>
		public static void Draw(float x1, float y1, float x2, float y2, Color c1, Color c2)
		{
			var vertices = new List<VertexPositionColorTexture>
			{
				new VertexPositionColorTexture(new Vector3(x1, y1, 0), c1, Vector2.Zero),
				new VertexPositionColorTexture(new Vector3(x2, y2, 0), c2, Vector2.Zero)
			};
			

			DrawMgr.AddVertices(GraphicsMode.LinePrimitives, null, vertices, new short[]{0, 1});
		}
		
		
	}
}
