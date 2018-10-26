using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace Monofoxe.Tiled.MapStructure
{
	public class TiledMapTileset
	{
		public string Name;
		public Texture2D[] Textures;
		public string[] TexturePaths;
		
		public int FirstGID;
		public int TileWidth;
		public int TileHeight;
		public int Spacing;
		public int Margin;
		public int TileCount;
		public int Columns;

		/* 
		 * More convenient way of getting tileset sizes.
		 * Tile count and columns are left just for compatibility
		 * with... whatever.
		 */
		public int Width => Columns;
		public int Height => TileCount / Columns;


		public Vector2 Offset;

		public TiledMapTile[] Tiles;
		
		public Color? BackgroundColor;


		//TODO: Add animated tiles.

		public Dictionary<string, string> Properties;
		
	}
}