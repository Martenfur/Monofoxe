using System.Collections.Generic;

namespace Monofoxe.Engine.Drawing
{
	public class LineStripPrimitive : Primitive2D
	{
		
		protected override GraphicsMode _type => GraphicsMode.LinePrimitives;

		public bool Looped = false;

		/// <summary>
		/// Sets indexes according to line strip pattern.
		/// </summary>
		/// <param name="loop">Tells is first and last vertix will have a line between them.</param>
		protected override short[] GetIndices()
		{
			// 0 - 1 - 2 - 3
			
			var indices = new List<short>();

			for(var i = 0; i < Vertices.Count - 1; i += 1)
			{
				indices.Add((short)i);
				indices.Add((short)(i + 1));
			}
			if (Looped)
			{
				indices.Add((short)(Vertices.Count - 1));
				indices.Add(0);
			}

			return indices.ToArray();
		}

		

	}
}
