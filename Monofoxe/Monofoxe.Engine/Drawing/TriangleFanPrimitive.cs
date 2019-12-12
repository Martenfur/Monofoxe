using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;

namespace Monofoxe.Engine.Drawing
{
	/// <summary>
	/// Drawable triangle fan primitive. 
	/// Draws a bunch of triangles, all of which begin at the first vertex.
	/// Pattern:
	/// 1 - 2
	/// | / |
	/// 0 - 3 
	/// | \ |
	/// 5 - 4 
	/// </summary>
	public class TriangleFanPrimitive : Primitive2D
	{	
		protected override PrimitiveType _primitiveType => PrimitiveType.TriangleList;

		/// <summary>
		/// Sets indexes according to trianglefan pattern.
		/// NOTE: Use counter-clockwise culling.
		/// </summary>
		protected override short[] GetIndices()
		{
			// 1 - 2
			// | / |
			// 0 - 3 
			// | \ |
			// 5 - 4

			var indices = new List<short>();
			
			for(var i = 1; i < Vertices.Count - 1; i += 1)
			{
				indices.Add(0);
				indices.Add((short)i);
				indices.Add((short)(i + 1));
			}

			return indices.ToArray();
		}


		

	}
}
