using Microsoft.Xna.Framework;

namespace Monofoxe.Engine.Drawing
{
	/// <summary>
	/// Anything that can be drawn.
	/// </summary>
	public abstract class Drawable
	{
		/// <summary>
		/// Drawable object's global position.
		/// </summary>
		public Vector2 Position;

		/// <summary>
		/// Draws the object.
		/// </summary>
		public abstract void Draw();
	}
}
