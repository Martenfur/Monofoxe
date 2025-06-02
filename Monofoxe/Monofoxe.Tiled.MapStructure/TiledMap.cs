using System.Collections.Generic;
using Microsoft.Xna.Framework;

namespace Monofoxe.Tiled.MapStructure
{
	/// <summary>
	/// Data structure for Tiled map.
	/// </summary>
	public class TiledMap
	{
		public string Name;

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

		public TiledMapLayer[] Layers;

		public TiledMapTileLayer[] TileLayers;
		public TiledMapObjectLayer[] ObjectLayers;
		public TiledMapImageLayer[] ImageLayers;


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


		/// <summary>
		/// Returns array of layers of given type (TiledMapTileLayer, TiledMapObjectLayer, TiledMapImageLayer).
		/// </summary>
		public T[] GetLayers<T>() where T : TiledMapLayer
		{
			var layersList = new List<T>();

			foreach(var layer in Layers)
			{
				if (layer is T)
				{
					layersList.Add((T)layer);
				}
			}

			return layersList.ToArray();
		}
	}
}
