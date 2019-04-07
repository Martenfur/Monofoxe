namespace Monofoxe.Engine.Drawing
{
	public class CustomLinePrimitive : Primitive2D
	{
		protected override GraphicsMode _type => GraphicsMode.LinePrimitives;
		
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
