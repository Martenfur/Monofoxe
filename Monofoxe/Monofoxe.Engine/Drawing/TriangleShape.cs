using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Monofoxe.Engine.Drawing
{
	/// <summary>
	/// Drawable triangle shape. Can be drawn by using static methods or be instantiated.
	/// </summary>
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

		/// <summary>
		/// If false, circle will be filled with solid color. If true, only outline will be drawn.
		/// </summary>
		public bool IsOutline = false;

		public Color Color = Color.White;


		public void Draw() =>
			Draw(Point1 + Position, Point2 + Position, Point3 + Position, IsOutline, Color, Color, Color);
		

		private static readonly short[][] _triangleIndices = 
		{
			new short[]{0, 1, 2}, // Filled.
			new short[]{0, 1, 1, 2, 2, 0} // Outline.
		};
		

		/// <summary>
		/// Draws a triangle.
		/// </summary>
		public static void Draw(Vector2 p1, Vector2 p2, Vector2 p3, bool isOutline) =>
			Draw(p1, p2, p3, isOutline, GraphicsMgr.CurrentColor, GraphicsMgr.CurrentColor, GraphicsMgr.CurrentColor);
		
		/// <summary>
		/// Draws a triangle with specified colors.
		/// </summary>
		public static void Draw(Vector2 p1, Vector2 p2, Vector2 p3, bool isOutline, Color c1, Color c2, Color c3)
		{
			PrimitiveType mode;
			short[] indices;
			if (isOutline)
			{
				mode = PrimitiveType.LineList;
				indices = _triangleIndices[1];
			}
			else
			{
				mode = PrimitiveType.TriangleList;
				indices = _triangleIndices[0];
			}
		
			var vertices = new VertexPositionColorTexture[]
			{
				new VertexPositionColorTexture(p1.ToVector3(), c1, Vector2.Zero),
				new VertexPositionColorTexture(p2.ToVector3(), c2, Vector2.Zero),
				new VertexPositionColorTexture(p3.ToVector3(), c3, Vector2.Zero)
			};

			GraphicsMgr.VertexBatch.Texture = null;
			GraphicsMgr.VertexBatch.DrawPrimitive(mode, vertices, indices);
		}
		
	}
}
