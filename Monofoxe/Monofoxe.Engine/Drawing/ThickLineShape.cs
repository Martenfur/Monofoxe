using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Monofoxe.Engine.Drawing
{
	/// <summary>
	/// Drawable thick line shape. Can be drawn by using static methods or be instantiated.
	/// </summary>
	public class ThickLineShape : IDrawable
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

		/// <summary>
		/// Line thickness.
		/// </summary>
		public float Thickness = 1;
		
		public Color Color = Color.White;

		public void Draw() =>
			Draw(Point1 + Position, Point2 + Position, Thickness, Color, Color);	
		
		
		
		private static readonly short[] _thickLineIndices = new short[]{0, 1, 3, 1, 2, 3};
		
		/// <summary>
		/// Draws a line with specified width.
		/// </summary>
		public static void Draw(Vector2 pt1, Vector2 pt2, float thickness) =>
			Draw(pt1.X, pt1.Y, pt2.X, pt2.Y, thickness, GraphicsMgr.CurrentColor, GraphicsMgr.CurrentColor);

		/// <summary>
		/// Draws a line with specified width and colors.
		/// </summary>
		public static void Draw(Vector2 pt1, Vector2 pt2, float thickness, Color c1, Color c2) =>
			Draw(pt1.X, pt1.Y, pt2.X, pt2.Y, thickness, c1, c2);

		/// <summary>
		/// Draws a line with specified width.
		/// </summary>
		public static void Draw(float x1, float y1, float x2, float y2, float thickness) =>
			Draw(x1, y1, x2, y2, thickness, GraphicsMgr.CurrentColor, GraphicsMgr.CurrentColor);
	

		/// <summary>
		/// Draws a line with specified width and colors.
		/// </summary>
		public static void Draw(float x1, float y1, float x2, float y2, float thickness, Color c1, Color c2)
		{
			var normal = new Vector3(y2 - y1, x1 - x2, 0);
			normal.Normalize(); // The result is a unit vector rotated by 90 degrees.
			normal *= thickness / 2;

			var vertices = new List<VertexPositionColorTexture>
			{
				new VertexPositionColorTexture(new Vector3(x1, y1, 0) + normal, c1, Vector2.Zero),
				new VertexPositionColorTexture(new Vector3(x1, y1, 0) - normal, c1, Vector2.Zero),
				new VertexPositionColorTexture(new Vector3(x2, y2, 0) - normal, c2, Vector2.Zero),
				new VertexPositionColorTexture(new Vector3(x2, y2, 0) + normal, c2, Vector2.Zero)
			};

			// Thick line is in fact just a rotated rectangle.
			GraphicsMgr.AddVertices(GraphicsMode.TrianglePrimitives, null, vertices, _thickLineIndices); 
		}
		
	}
}
