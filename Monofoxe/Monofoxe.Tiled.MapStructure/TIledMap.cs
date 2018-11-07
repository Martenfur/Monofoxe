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
		public TiledMapObjectLayer[] ObjectLayers;


		public Dictionary<string, string> Properties;

		// TODO: Add infinite map support.



		public TiledMapTilesetTile? GetTilesetTile(int gid)
		{
			var tileset = GetTileset(gid);
			if (tileset != null)
			{
				return tileset.Tiles[gid - tileset.FirstGID];
			}
			return null;
		}

		public TiledMapTileset GetTileset(int gid)
		{
			foreach(var tileset in Tilesets)
			{
				if (gid >= tileset.FirstGID && gid < tileset.FirstGID + tileset.Tiles.Length)
				{
					return tileset;
				}
			}
			return null;
		}

	}
}
