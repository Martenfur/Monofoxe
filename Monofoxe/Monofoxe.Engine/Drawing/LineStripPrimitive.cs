using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;

namespace Monofoxe.Engine.Drawing
{
	/// <summary>
	/// Drawable line strip primitive. Draws a bunch of connected lines. Can be looped.
	/// Pattern: 0 - 1 - 2 - 3
	/// </summary>
	public class LineStripPrimitive : Primitive2D
	{
		
		protected override PrimitiveType _primitiveType => PrimitiveType.LineList;

		/// <summary>
		/// If true, a line between first and last vertex will be drawn.
		/// </summary>
		public bool Looped = false;

		/// <summary>
		/// Sets indices according to line strip pattern.
		/// </summary>
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
