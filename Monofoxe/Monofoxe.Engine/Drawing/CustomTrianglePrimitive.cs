using Microsoft.Xna.Framework.Graphics;

namespace Monofoxe.Engine.Drawing
{
	/// <summary>
	/// Triangle primitive, which can accept any array of indices. 
	/// </summary>
	public class CustomTrianglePrimitive : Primitive2D
	{
		protected override PrimitiveType _primitiveType => PrimitiveType.TriangleList;
		
		/// <summary>
		/// Array of primitive indices. They tell primitive what vertices to draw in what order.
		/// </summary>
		public short[] Indices;

		public CustomTrianglePrimitive(short[] indices = null)
		{
			Indices = indices;
		}


		/// <summary>
		/// Sets user-defined list of indices for a list of triangles.
		/// </summary>
		protected override short[] GetIndices() =>
			Indices;
	}
}
