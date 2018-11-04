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

		public RenderOrder RenderOrder;
		public Orientation Orientation;

		public StaggerAxis StaggerAxis = StaggerAxis.None;
		public StaggerIndex StaggerIndex = StaggerIndex.None;

		public int HexSideLength;

		public TiledMapTileset[] Tilesets;

		public TiledMapTileLayer[] TileLayers;

		public Dictionary<string, string> Properties;

		// TODO: Add infinite map support.
	}
}
