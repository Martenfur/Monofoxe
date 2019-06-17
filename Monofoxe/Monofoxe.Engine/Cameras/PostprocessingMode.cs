namespace Monofoxe.Engine.Cameras
{
	/// <summary>
	/// Postprocessing modes for camera.
	/// </summary>
	public enum PostprocessingMode
	{
		/// <summary>
		/// No shaders will be applied.
		/// </summary>
		None,

		/// <summary>
		/// Enables applying shaders to the camera surface.
		/// </summary>
		Camera,
		
		/// <summary>
		/// Enables applying shaders to the camera surface 
		/// AND individual layers.
		/// </summary>
		CameraAndLayers,
	}
}
