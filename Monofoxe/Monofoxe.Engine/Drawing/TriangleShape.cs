using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Monofoxe.Engine.Drawing
{
	/// <summary>
	/// Drawable triangle shape. Can be drawn by using static methods or be instantiated.
	/// </summary>
	public static class TriangleShape
	{
		private static VertexPositionColorTexture[] _triangleVertices = new VertexPositionColorTexture[4];

		private static short[] _filledTriangleIndices = { 0, 1, 2 };
		private static short[] _outlineTriangleIndices = { 0, 1, 1, 2, 2, 0 };
		

		/// <summary>
		/// Draws a triangle.
		/// </summary>
		public static void Draw(Vector2 p1, Vector2 p2, Vector2 p3, ShapeFill fill, float zDepth = 0) =>
			Draw(p1, p2, p3, fill, GraphicsMgr.CurrentColor, GraphicsMgr.CurrentColor, GraphicsMgr.CurrentColor, zDepth);


		/// <summary>
		/// Draws a triangle with specified colors.
		/// </summary>
		public static void Draw(Vector2 p1, Vector2 p2, Vector2 p3, ShapeFill fill, Color c1, Color c2, Color c3, float zDepth = 0)
		{
			_triangleVertices[0].Position = new Vector3(p1.X, p1.Y, zDepth);
			_triangleVertices[0].Color = c1;
			_triangleVertices[1].Position = new Vector3(p2.X, p2.Y, zDepth);
			_triangleVertices[1].Color = c2;
			_triangleVertices[2].Position = new Vector3(p3.X, p3.Y, zDepth);
			_triangleVertices[2].Color = c3;
			
			GraphicsMgr.VertexBatch.Texture = null;
			if (fill == ShapeFill.Outline)
			{
				GraphicsMgr.VertexBatch.AddPrimitive(PrimitiveType.LineList, _triangleVertices, _outlineTriangleIndices);
			}
			else
			{
				GraphicsMgr.VertexBatch.AddPrimitive(PrimitiveType.TriangleList, _triangleVertices, _filledTriangleIndices);
			}
		}
	}
}
