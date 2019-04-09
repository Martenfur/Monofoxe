using System.Collections.Generic;

namespace Monofoxe.Engine.Drawing
{
	/// <summary>
	/// Drawable mesh primitive. Draws a grid of triangles.
	/// Pattern:
	/// 0 - 1 - 2
	/// | / | / |
	/// 3 - 4 - 5
	/// | / | / |
	/// 6 - 7 - 8
	/// </summary>
	public class MeshPrimitive : Primitive2D
	{
		protected override GraphicsMode _graphicsMode => GraphicsMode.TrianglePrimitives;

		/// <summary>
		/// Mesh width in cells.
		/// </summary>
		public int Width;

		/// <summary>
		/// Mesh height in cells.
		/// </summary>
		public int Height;


		public MeshPrimitive(int width = 1, int height = 1)
		{
			Width = width;
			Height = height;
		}

		/// <summary>
		/// Sets indexes according to mesh pattern.
		/// NOTE: Make sure there's enough vertices for width and height of the mesh.
		/// NOTE: Use counter-clockwise culling.
		/// </summary>
		protected override short[] GetIndices()
		{
			// 0 - 1 - 2
			// | / | / |
			// 3 - 4 - 5
			// | / | / |
			// 6 - 7 - 8

			var indices = new List<short>();
			
			var offset = 0; // Basically, equals w * y.

			for(var y = 0; y < Height - 1; y += 1)
			{
				for(var x = 0; x < Width - 1; x += 1)
				{
					indices.Add((short)(x + offset));
					indices.Add((short)(x + 1 + offset));
					indices.Add((short)(x + Width + offset));

					indices.Add((short)(x + Width + offset));
					indices.Add((short)(x + 1 + offset));
					indices.Add((short)(x + Width + 1 + offset));
				}
				offset += Width;
			}
			
			return indices.ToArray();
		}

		

	}
}
