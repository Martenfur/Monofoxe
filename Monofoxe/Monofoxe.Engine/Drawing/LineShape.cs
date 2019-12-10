using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Monofoxe.Engine.Drawing
{
	/// <summary>
	/// Drawable line shape. Can be drawn by using static methods or be instantiated.
	/// NOTE: The line has no width. 
	/// </summary>
	public class LineShape : IDrawable
	{
		public Vector2 Position {get; set;}

		/// <summary>
		/// First line point. 
		/// NOTE: all line points treat position as an origin point;
		/// </summary>
		public Vector2 Point1;

		/// <summary>
		/// Second line point. 
		/// NOTE: all line points treat position as an origin point;
		/// </summary>
		public Vector2 Point2;
		
		public Color Color = Color.White;

		private static VertexPositionColorTexture[] _lineVertices = new VertexPositionColorTexture[2];
		private static short[] _lineIndices = { 0, 1 };


		public void Draw() =>
			Draw(Point1 + Position, Point2 + Position, Color, Color);	
		
		
		/// <summary>
		/// Draws a line.
		/// </summary>
		public static void Draw(Vector2 p1, Vector2 p2) =>
			Draw(p1, p2, GraphicsMgr.CurrentColor, GraphicsMgr.CurrentColor);

		/// <summary>
		/// Draws a line with specified colors.
		/// </summary>
		public static void Draw(Vector2 p1, Vector2 p2, Color c1, Color c2)
		{
			_lineVertices[0].Position = p1.ToVector3();
			_lineVertices[0].Color = c1;
			_lineVertices[1].Position = p2.ToVector3();
			_lineVertices[1].Color = c2;
			
			GraphicsMgr.VertexBatch.Texture = null;
			GraphicsMgr.VertexBatch.DrawPrimitive(PrimitiveType.LineList, _lineVertices, _lineIndices);
		}
		
		
	}
}
