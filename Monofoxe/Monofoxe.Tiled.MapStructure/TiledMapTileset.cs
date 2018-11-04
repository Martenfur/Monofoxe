using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

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
		
		/// <summary>
		/// Spacing between tiles in tileset.
		/// Doesn't work with image collection tilesets.
		/// </summary>
		public int Spacing;

		/// <summary>
		/// Padding of the first tile row\column.
		/// </summary>
		public int Margin;
		
		public int TileCount;
		
		/// <summary>
		/// Amount of columns in tileset.
		/// Will be 0 in image collection tileset.
		/// </summary>
		public int Columns;

		/* 
		 * More convenient way of getting tileset sizes.
		 * Tile count and columns are left just for compatibility
		 * with... whatever.
		 */
		public int Width
		{
			get 
			{
				if (Columns == 0) // This means image collection tileset, which is essentially a list of images.
				{
					return TileCount;
				}
				return Columns;
			}
		}
		public int Height
		{
			get 
			{
				if (Columns == 0) // This means image collection tileset, which is essentially a list of images.
				{
					return 1;
				}
				return TileCount / Columns;
			}
		}


		public Vector2 Offset;

		public TiledMapTilesetTile[] Tiles;
		
		public Color? BackgroundColor;

		//TODO: Add animated tiles.

		public Dictionary<string, string> Properties;
		
	}
}