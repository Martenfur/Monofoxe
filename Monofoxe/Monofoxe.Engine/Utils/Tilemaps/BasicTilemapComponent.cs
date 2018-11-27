using System;
using Microsoft.Xna.Framework;
using Monofoxe.Engine.Drawing;
using Monofoxe.Engine.ECS;

namespace Monofoxe.Engine.Utils.Tilemaps
{
	/// <summary>
	/// Basic tilemap class. Provides basic functionality,
	/// supports camera zooming.
	/// </summary>
	public class BasicTilemapComponent : Component, ITilemap<BasicTile>
	{
		public override string Tag => "basicTilemap";

		protected BasicTile[,] _tileGrid;

		public Vector2 Offset {get; set;} = Vector2.Zero;

		public int TileWidth {get; protected set;}
		public int TileHeight {get; protected set;}

		public int Width {get; protected set;}
		public int Height {get; protected set;}

		/// <summary>
		/// Tells how many tile rows and columns will be drawn outside of camera's bounds.
		/// May be useful for tiles larger than the grid. 
		/// </summary>
		public int Padding = 0;

		
		public BasicTile? GetTile(int x, int y)
		{
			if (InBounds(x, y))
			{
				return _tileGrid[x, y]; 
			}
			return null;
		}

		public void SetTile(int x, int y, BasicTile tile)
		{
			if (InBounds(x, y))
			{
				_tileGrid[x, y] = tile;
			}
		}

		/// <summary>
		/// Returns tile without out-of-bounds check.
		/// WARNING: This method will throw an exception, if coordinates are out of bounds.
		/// </summary>
		public BasicTile GetTileUnsafe(int x, int y) =>
			_tileGrid[x, y];
		
		/// <summary>
		/// Sets tile without out-of-bounds check.
		/// WARNING: This method will throw an exception, if coordinates are out of bounds.
		/// </summary>
		public BasicTile SetTileUnsafe(int x, int y, BasicTile tile) =>
			_tileGrid[x, y] = tile;
		
		
		public bool InBounds(int x, int y) =>
			x >= 0 && y >= 0 && x < Width && y < Height;
		

		public BasicTilemapComponent(int width, int height, int tileWidth, int tileHeight)
		{
			Width = width;
			Height = height;
			TileWidth = tileWidth;
			TileHeight = tileHeight;
			_tileGrid = new BasicTile[Width, Height];
		}


		public override object Clone() =>
			throw new NotImplementedException();

	}
}
