using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;


namespace Monofoxe.Engine.Drawing
{
	public class Frame
	{
		/// <summary>
		/// Texture atlass where frame is stored.
		/// </summary>
		public readonly Texture2D Texture;

		/// <summary>
		/// Position of the frame on the atlass. Note that it may be rotated.
		/// </summary>
		public readonly Rectangle TexturePosition;

		/// <summary>
		/// Width of the frame.
		/// </summary>
		public readonly int W;

		/// <summary>
		/// Height of the frame.
		/// </summary>
		public readonly int H;
		
		public readonly Vector2 Origin;

		public Frame(Texture2D texture, Rectangle texturePosition, Vector2 origin, int w, int h)
		{
			Texture = texture;
			TexturePosition = texturePosition;
			
			W = w;
			H = h;
			Origin = origin;
		}
	}
}
