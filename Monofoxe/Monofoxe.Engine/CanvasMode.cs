namespace Monofoxe.Engine
{
	/// <summary>
	/// Canvas drawing modes for WindowMgr.
	/// </summary>
	public enum CanvasMode
	{
		/// <summary>
		/// Keeps aspect ration between screen and canvas, 
		/// resulting in black bars.
		/// </summary>
		KeepAspectRatio,

		/// <summary>
		/// Scales canvas to fit them into screen.
		/// </summary>
		Fill,

		/// <summary>
		/// Canvas stays as is, without any transforming.
		/// </summary>
		None,
	}
}