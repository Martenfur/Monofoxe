using System;
using System.Collections.Generic;
using System.Text;

namespace Monofoxe.Tiled.MapStructure
{
	public class TiledMapTileLayer : TiledMapLayer
	{
		public int Width;
		public int Height;
		public int TileWidth;
		public int TileHeight;
		public TiledMapTile[,] Tiles;
	}
}
