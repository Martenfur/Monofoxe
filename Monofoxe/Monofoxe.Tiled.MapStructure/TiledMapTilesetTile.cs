using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Monofoxe.Tiled.MapStructure
{
	[Serializable()]
	public class TiledMapTilesetTile
	{
		public int GID;
		public int TextureID;

		public Texture2D Texture => Tileset.Textures[TextureID];
		
		public TiledMapTileset Tileset;
		
		public Rectangle TexturePosition;

		public Dictionary<string, string> Properties;
	}
}