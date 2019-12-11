using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Monofoxe.Tiled.MapStructure.Objects;


namespace Monofoxe.Tiled.MapStructure
{
	public struct TiledMapTilesetTile
	{
		public int GID;
		public int TextureID;

		public Texture2D Texture
		{
			get
			{
				if (Tileset == null)
				{
					return null;
				}
				else
				{
					return Tileset.Textures[TextureID];
				}
			}
		}

		
		public TiledMapTileset Tileset;
		
		public Rectangle TexturePosition;

		public TiledMapObjectDrawingOrder ObjectsDrawingOrder;
		public TiledObject[] Objects;

		public Dictionary<string, string> Properties;
	}
}
