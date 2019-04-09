namespace Monofoxe.Engine.Drawing
{
	/// <summary>
	/// Line primitive, which can accept any array of indices. 
	/// </summary>
	public class CustomLinePrimitive : Primitive2D
	{
		protected override GraphicsMode _graphicsMode => GraphicsMode.LinePrimitives;
		
		/// <summary>
		/// Array of primitive indices. They tell primitive what vertices to draw in what order.
		/// </summary>
		public short[] Indices;

		public CustomLinePrimitive(short[] indices = null)
		{
			Indices = indices;
		}

		/// <summary>
		/// Sets user-defined list of indices for a list of lines.
		/// </summary>
		protected override short[] GetIndices() =>
			Indices;
	}
}
