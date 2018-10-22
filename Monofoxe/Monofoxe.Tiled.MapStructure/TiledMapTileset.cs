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
		public int FirstGID;
		public int TileWidth;
		public int TileHeight;
		public int Spacing;
		public int Margin;
		public int TileCount;
		public int Columns;

		public Vector2 Offset;

		public TiledMapTile[] Tiles;
		
		//TODO: Add animated tiles.

		public Dictionary<string, string> Properties;
		
	}
}