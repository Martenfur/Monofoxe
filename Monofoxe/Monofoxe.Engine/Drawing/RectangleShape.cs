using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Monofoxe.Engine.Drawing
{
	public class RectangleShape : Drawable
	{
		public Vector2 Size = Vector2.One;

		/// <summary>
		/// If false, circle will be filled with solid color. If true, only outline will be drawn.
		/// </summary>
		public bool IsOutline = false;

		public Color Color = Color.White;

		public float ZDepth = 0;

		/// <summary>
		/// Draws a rectangle using instance properties.
		/// </summary>
		public override void Draw()
		{
			Draw(
				Position - Size / 2, Position + Size / 2, 
				IsOutline, 
				Color, Color, Color, Color, 
				ZDepth
			);
		}
		
		
		private static VertexPositionColorTexture[] _rectangleVertices = new VertexPositionColorTexture[4];

		private static short[] _filledRectangleIndices = { 0, 1, 3, 1, 2, 3 }; 
		private static short[] _outlineRectangleIndices = { 0, 1, 1, 2, 2, 3, 3, 0 };


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
			Color c4,
			float zDepth = 0
		)
		{
			_rectangleVertices[0].Position = new Vector3(p1.X, p1.Y, zDepth);
			_rectangleVertices[0].Color = c1;
			_rectangleVertices[1].Position = new Vector3(p2.X, p1.Y, zDepth);
			_rectangleVertices[1].Color = c2;
			_rectangleVertices[2].Position = new Vector3(p2.X, p2.Y, zDepth);
			_rectangleVertices[2].Color = c3;
			_rectangleVertices[3].Position = new Vector3(p1.X, p2.Y, zDepth);
			_rectangleVertices[3].Color = c4;
						
			GraphicsMgr.VertexBatch.Texture = null;
			if (isOutline)
			{
				GraphicsMgr.VertexBatch.AddPrimitive(PrimitiveType.LineList, _rectangleVertices, _outlineRectangleIndices);
			}
			else
			{
				GraphicsMgr.VertexBatch.AddPrimitive(PrimitiveType.TriangleList, _rectangleVertices, _filledRectangleIndices);
			}
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
