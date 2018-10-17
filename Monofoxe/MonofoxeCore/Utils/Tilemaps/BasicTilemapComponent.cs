using System;
using Microsoft.Xna.Framework;
using Monofoxe.Engine.Drawing;
using Monofoxe.Engine.ECS;

namespace Monofoxe.Utils.Tilemaps
{
	public class BasicTilemapComponent : Component, ITilemap<BasicTile>
	{
		public override string Tag => "basicTilemap";

		protected BasicTile[,] _tileGrid;

		public Vector2 Offset {get; set;} = Vector2.Zero;

		public int TileWidth {get; protected set;}
		public int TileHeight {get; protected set;}

		public int Width {get; protected set;}
		public int Height {get; protected set;}

		
		public BasicTile GetTile(int x, int y) => 
			_tileGrid[x, y]; 

		public void SetTile(int x, int y, BasicTile tile) => 
			_tileGrid[x, y] = tile;

		/// <summary>
		/// Used, if there was an error.
		/// </summary>
		public Frame DefaultTile;

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
