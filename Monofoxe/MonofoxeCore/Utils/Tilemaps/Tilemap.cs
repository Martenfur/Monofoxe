using System;
using Microsoft.Xna.Framework;
using Monofoxe.Engine.Drawing;

namespace Monofoxe.Utils.Tilemaps
{
	public class Tilemap : ITilemap<BasicTile>
	{
		protected BasicTile[,] _tileGrid;

		public Vector2 Offset {get; set;} = Vector2.Zero;

		public uint TileWidth {get; protected set;}
		public uint TileHeight {get; protected set;}

		public uint Width {get; protected set;}
		public uint Height {get; protected set;}

		
		public BasicTile GetTile(int x, int y) => 
			_tileGrid[x, y]; 

		public void SetTile(int x, int y, BasicTile tile) => 
			_tileGrid[x, y] = tile;

		/// <summary>
		/// Used, if there was an error.
		/// </summary>
		public Frame DefaultTile;

		public Tilemap(uint width, uint height, uint tileWidth, uint tileHeight)
		{
			Width = width;
			Height = height;
			TileWidth = tileWidth;
			TileHeight = tileHeight;
			_tileGrid = new BasicTile[Width, Height];
		}

		public void Draw()
		{
			throw new NotImplementedException();
		}

		public void Update()
		{
			throw new NotImplementedException();
		}
	}
}
