using System;

namespace Monofoxe.Tiled.MapStructure
{
	[Serializable()]	
	public class TiledMapTileLayer : TiledMapLayer
	{
		public int Width;
		public int Height;
		public int TileWidth;
		public int TileHeight;
		public TiledMapTile[][] Tiles;
	}
}
