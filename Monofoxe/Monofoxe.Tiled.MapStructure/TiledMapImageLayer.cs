using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Monofoxe.Tiled.MapStructure
{
	public class TiledMapImageLayer : TiledMapLayer
	{
		public string ImagePath;
		public Texture2D Texture;

		/// <summary>
		/// Tiled will treat this color as transparent.
		/// Ah, blast right from 1998.
		/// </summary>
		public Color TransparentColor;
	}
}
