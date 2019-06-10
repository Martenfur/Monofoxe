namespace Monofoxe.Engine.Drawing
{
	/// <summary>
	/// Every time we want to draw primitive of different type
	/// or switch texture, we need to empty vertex buffer
	/// and switch graphics mode.
	/// </summary>
	public enum GraphicsMode
	{
		/// <summary>
		/// No mode set.
		/// </summary>
		None,

		/// <summary>
		/// Sprite batch.
		/// </summary>
		Sprites,

		/// <summary>
		/// Text.
		/// </summary>
		SpritesNonPremultiplied,

		/// <summary>
		/// Triangle list.
		/// </summary>
		TrianglePrimitives,

		/// <summary>
		/// Line list.
		/// </summary>
		LinePrimitives,
	}
}
