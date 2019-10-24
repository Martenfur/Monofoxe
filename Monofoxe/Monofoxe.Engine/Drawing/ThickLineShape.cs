using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Monofoxe.Engine.Utils;

namespace Monofoxe.Engine.Drawing
{
	/// <summary>
	/// Drawable thick line shape. Can be drawn by using static methods or be instantiated.
	/// </summary>
	public class ThickLineShape : IDrawable
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
			Draw(pt1, pt2, thickness, GraphicsMgr.CurrentColor, GraphicsMgr.CurrentColor);

		/// <summary>
		/// Draws a line with specified width and colors.
		/// </summary>
		public static void Draw(Vector2 pt1, Vector2 pt2, float thickness, Color c1, Color c2)
		{
			var normal2 = (pt2 - pt1).Rotate90(); // TODO: TEST!!!

			normal2 = normal2.GetSafeNormalize(); // The result is a unit vector rotated by 90 degrees.
			normal2 *= thickness / 2;

			var normal = normal2.ToVector3();

			var vertices = new List<VertexPositionColorTexture>
			{
				new VertexPositionColorTexture(pt1.ToVector3() - normal, c1, Vector2.Zero),
				new VertexPositionColorTexture(pt1.ToVector3() + normal, c1, Vector2.Zero),
				new VertexPositionColorTexture(pt2.ToVector3() + normal, c2, Vector2.Zero),
				new VertexPositionColorTexture(pt2.ToVector3() - normal, c2, Vector2.Zero)
			};

			// Thick line is in fact just a rotated rectangle.
			GraphicsMgr.AddVertices(GraphicsMode.TrianglePrimitives, null, vertices, _thickLineIndices); 
		}
		
	}
}
