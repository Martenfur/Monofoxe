namespace Monofoxe.Engine
{
	/// <summary>
	/// Every time we want to draw primitive of a new type
	/// or switch texture, we need to empty vertex buffer
	/// and switch pipeline mode.
	/// </summary>
	public enum PipelineMode
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
		OutlinePrimitives,
	}
}
