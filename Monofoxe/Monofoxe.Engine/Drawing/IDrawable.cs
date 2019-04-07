using Microsoft.Xna.Framework;

namespace Monofoxe.Engine.Drawing
{
	/// <summary>
	/// Anything that can be drawn.
	/// </summary>
	public interface IDrawable
	{
		/// <summary>
		/// Drawable object's global position.
		/// </summary>
		Vector2 Position {get; set;}

		/// <summary>
		/// Draws the object.
		/// </summary>
		void Draw();
	}
}
