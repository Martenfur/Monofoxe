using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Monofoxe.Engine.Drawing
{
	public class RectangleShape : IDrawable
	{
		/// <summary>
		/// Center point of a rectangle.
		/// </summary>
		public Vector2 Position {get; set;}

		public Vector2 Size = Vector2.One;

		/// <summary>
		/// If false, circle will be filled with solid color. If true, only outline will be drawn.
		/// </summary>
		public bool IsOutline = false;

		public Color Color = Color.White;

		/// <summary>
		/// Draws a rectangle using instance properties.
		/// </summary>
		public void Draw()
		{
			Draw(
				Position - Size / 2, Position - Size / 2, 
				IsOutline, 
				Color, Color, Color, Color
			);
		}
		
		private static readonly short[][] _rectangleIndices = 
		{
			new short[]{0, 1, 3, 1, 2, 3}, // Filled.
			new short[]{0, 1, 1, 2, 2, 3, 3, 0} // Outline.
		};

		
		/// <summary>
		/// Draws a rectangle using top left and bottom right point.
		/// </summary>
		public static void Draw(Vector2 p1, Vector2 p2, bool isOutline) =>
			Draw(p1, p2, isOutline, GraphicsMgr.CurrentColor, GraphicsMgr.CurrentColor, GraphicsMgr.CurrentColor, GraphicsMgr.CurrentColor);

		/// <summary>
		/// Draws a rectangle using top left and bottom right point with specified colors for each corner.
		/// </summary>
		public static void Draw(
			Vector2 p1, 
			Vector2 p2, 
			bool isOutline, 
			Color c1, 
			Color c2, 
			Color c3, 
			Color c4
		)
		{
			GraphicsMode mode;
			short[] indices;
			if (isOutline)
			{
				mode = GraphicsMode.LinePrimitives;
				indices = _rectangleIndices[1];
			}
			else
			{
				mode = GraphicsMode.TrianglePrimitives;
				indices = _rectangleIndices[0];
			}
			var x1 = p1.X;
			var y1 = p1.Y;
			var x2 = p2.X;
			var y2 = p2.Y;

			var vertices = new List<VertexPositionColorTexture>
			{
				new VertexPositionColorTexture(new Vector3(x1, y1, 0), c1, Vector2.Zero),
				new VertexPositionColorTexture(new Vector3(x2, y1, 0), c2, Vector2.Zero),
				new VertexPositionColorTexture(new Vector3(x2, y2, 0), c3, Vector2.Zero),
				new VertexPositionColorTexture(new Vector3(x1, y2, 0), c4, Vector2.Zero)
			};

			GraphicsMgr.AddVertices(mode, null, vertices, indices);
		}


		/// <summary>
		/// Draws a rectangle using center point and size.
		/// </summary>
		public static void DrawBySize(Vector2 p, Vector2 size, bool isOutline) =>
			Draw(p - size / 2, p + size / 2f, isOutline, GraphicsMgr.CurrentColor, GraphicsMgr.CurrentColor, GraphicsMgr.CurrentColor, GraphicsMgr.CurrentColor);

		/// <summary>
		/// Draws a rectangle using center point and size with specified colors for each corner.
		/// </summary>
		public static void DrawBySize(Vector2 p, Vector2 size, bool isOutline, Color c1, Color c2, Color c3, Color c4) =>
			Draw(p - size / 2f, p + size / 2f, isOutline, c1, c2, c3, c4);

		

	}
}
