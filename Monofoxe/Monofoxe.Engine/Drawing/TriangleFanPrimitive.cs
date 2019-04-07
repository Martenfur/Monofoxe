using System.Collections.Generic;

namespace Monofoxe.Engine.Drawing
{
	public class TriangleFanPrimitive : Primitive2D
	{	
		protected override GraphicsMode _type => GraphicsMode.TrianglePrimitives;

		/// <summary>
		/// Sets indexes according to trianglefan pattern.
		/// NOTE: Use counter-clockwise culling.
		/// </summary>
		protected override short[] GetIndices()
		{
			//   1
			//  / \
			// 0 - 2 
			//  \ / 
			//   3 

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
