using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Monofoxe.Engine.Drawing
{
	/// <summary>
	/// Drawable line shape.
	/// NOTE: The line has no width. 
	/// </summary>
	public static class LineShape
	{
		private static VertexPositionColorTexture[] _lineVertices = new VertexPositionColorTexture[2];
		private static short[] _lineIndices = { 0, 1 };
		
		/// <summary>
		/// Draws a line.
		/// </summary>
		public static void Draw(Vector2 p1, Vector2 p2) =>
			Draw(p1, p2, GraphicsMgr.CurrentColor, GraphicsMgr.CurrentColor);

		/// <summary>
		/// Draws a line with specified colors.
		/// </summary>
		public static void Draw(Vector2 p1, Vector2 p2, Color c1, Color c2, float zDepth = 0)
		{
			_lineVertices[0].Position = new Vector3(p1, zDepth);
			_lineVertices[0].Color = c1;
			_lineVertices[1].Position = new Vector3(p2, zDepth);
			_lineVertices[1].Color = c2;
			
			GraphicsMgr.VertexBatch.Texture = null;
			GraphicsMgr.VertexBatch.AddPrimitive(PrimitiveType.LineList, _lineVertices, _lineIndices);
		}
	}
}
