using System.Collections.Generic;
using Microsoft.Xna.Framework;

namespace Monofoxe.Tiled.MapStructure
{
	/// <summary>
	/// Data structure for Tiled map.
	/// </summary>
	public class TiledMap
	{
		public Color? BackgroundColor;

		public int Width;
		public int Height;

		public int TileWidth;
		public int TileHeight;

		public TiledMapTileset[] Tilesets;

		public TiledMapTileLayer[] TileLayers;

		public Dictionary<string, string> Properties;
	}
}
