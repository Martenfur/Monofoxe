namespace Monofoxe.Engine.Drawing
{
	public class CustomTrianglePrimitive : Primitive2D
	{
		protected override GraphicsMode _type => GraphicsMode.TrianglePrimitives;
		
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
