using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;

namespace Monofoxe.Tiled.MapStructure
{
	public struct TiledMapTile
	{
		public int GID;
		public bool FlipHor;
		public bool FlipVer;
		public bool IsBlank;

		public TiledMapTileset Tileset;
	}
}
