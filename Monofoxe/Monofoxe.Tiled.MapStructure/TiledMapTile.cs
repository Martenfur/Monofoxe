using Microsoft.Xna.Framework.Graphics;

namespace Monofoxe.Tiled.MapStructure
{
	public struct TiledMapTile
	{
		public int GID;
		public int TextureID;
		public bool FlipHor;
		public bool FlipVer;
		public bool IsBlank;

		public Texture2D Texture => Tileset.Textures[TextureID];
		
		public TiledMapTileset Tileset;
	}
}
