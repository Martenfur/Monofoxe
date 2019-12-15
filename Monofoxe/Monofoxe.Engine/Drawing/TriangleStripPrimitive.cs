using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;

namespace Monofoxe.Engine.Drawing
{
	/// <summary>
	/// Drawable triangle strip primitive. Draws a strip of triangles.
	/// Pattern:
	/// 0 - 2 - 4
	///  \ / \ /
	///   1 - 3
	/// </summary>
	public class TriangleStripPrimitive : Primitive2D
	{
		
		protected override PrimitiveType _primitiveType => PrimitiveType.TriangleList;

		public TriangleStripPrimitive(int capacity) : base(capacity)
		{
		}

		/// <summary>
		/// Sets indices according to trianglestrip pattern.
		/// NOTE: Use counter-clockwise culling.
		/// </summary>
		protected override short[] GetIndices()
		{
			// 0 - 2 - 4
			//  \ / \ /
			//   1 - 3
			
			var indices = new List<short>();
			
			var flip = true;
			for(var i = 0; i < Vertices.Length - 2; i += 1)
			{
				indices.Add((short)i);
				if (flip) // Taking in account counter-clockwise culling.
				{
					indices.Add((short)(i + 2));
					indices.Add((short)(i + 1));
				}
				else
				{
					indices.Add((short)(i + 1));
					indices.Add((short)(i + 2));
				}
				flip = !flip;
			}

			return indices.ToArray();
		}

		

	}
}
