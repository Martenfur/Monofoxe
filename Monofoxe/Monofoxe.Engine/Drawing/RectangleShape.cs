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
				Position.X - Size.X / 2, Position.Y - Size.Y / 2, 
				Position.X + Size.X / 2, Position.Y + Size.Y / 2, 
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
			Draw(p1.X, p1.Y, p2.X, p2.Y, isOutline, GraphicsMgr.CurrentColor, GraphicsMgr.CurrentColor, GraphicsMgr.CurrentColor, GraphicsMgr.CurrentColor);

		/// <summary>
		/// Draws a rectangle using top left and bottom right point with specified colors for each corner.
		/// </summary>
		public static void Draw(Vector2 p1, Vector2 p2, bool isOutline, Color c1, Color c2, Color c3, Color c4) =>
			Draw(p1.X, p1.Y, p2.X, p2.Y, isOutline, c1, c2, c3, c4);

		/// <summary>
		/// Draws a rectangle using top left and bottom right point.
		/// </summary>
		public static void Draw(float x1, float y1, float x2, float y2, bool isOutline) =>
			Draw(x1, y1, x2, y2, isOutline, GraphicsMgr.CurrentColor, GraphicsMgr.CurrentColor, GraphicsMgr.CurrentColor, GraphicsMgr.CurrentColor);
		
		/// <summary>
		/// Draws a rectangle using top left and bottom right point with specified colors for each corner.
		/// </summary>
		public static void Draw(float x1, float y1, float x2, float y2, bool isOutline, Color c1, Color c2, Color c3, Color c4)
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
			Draw(p.X - size.X / 2, p.Y - size.Y / 2, p.X + size.X / 2, p.Y + size.Y / 2, isOutline, GraphicsMgr.CurrentColor, GraphicsMgr.CurrentColor, GraphicsMgr.CurrentColor, GraphicsMgr.CurrentColor);

		/// <summary>
		/// Draws a rectangle using center point and size with specified colors for each corner.
		/// </summary>
		public static void DrawBySize(Vector2 p, Vector2 size, bool isOutline, Color c1, Color c2, Color c3, Color c4) =>
			Draw(p.X - size.X / 2, p.Y - size.Y / 2, p.X + size.X / 2, p.Y + size.Y / 2, isOutline, c1, c2, c3, c4);

		/// <summary>
		/// Draws a rectangle using center point and size.
		/// </summary>
		public static void DrawBySize(float x, float y, float w, float h, bool isOutline) =>
			Draw(x - w / 2, y - h / 2, x + w / 2, y + h / 2, isOutline, GraphicsMgr.CurrentColor, GraphicsMgr.CurrentColor, GraphicsMgr.CurrentColor, GraphicsMgr.CurrentColor);
		
		/// <summary>
		/// Draws a rectangle using center point and size with specified colors for each corner.
		/// </summary>
		public static void DrawBySize(float x, float y, float w, float h, bool isOutline, Color c1, Color c2, Color c3, Color c4) =>
				Draw(x - w / 2, y - h / 2, x + w / 2, y + h / 2, isOutline, c1, c2, c3, c4);
		
			
		

	}
}
