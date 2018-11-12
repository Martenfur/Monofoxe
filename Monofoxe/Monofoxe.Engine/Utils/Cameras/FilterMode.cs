namespace Monofoxe.Engine.Utils.Cameras
{
	/// <summary>
	/// filtering modes for camera.
	/// </summary>
	public enum FilterMode
	{
		/// <summary>
		/// Triggers rendering, if filter DOES contain layer.
		/// </summary>
		Inclusive,
	
		/// <summary>
		/// Triggers rendering, if filter DOES NOT contain layer.
		/// </summary>
		Exclusive,

		/// <summary>
		/// Renders all layers.
		/// </summary>
		None,
	}
}
