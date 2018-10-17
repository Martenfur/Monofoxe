﻿using System;
using Microsoft.Xna.Framework;

namespace Monofoxe.Utils.Tilemaps
{
	public interface ITilemap<T> where T : struct, ITile
	{
		Vector2 Offset {get; set;}
		int TileWidth {get;}
		int TileHeight {get;}

		int Width {get;}
		int Height {get;}

		T GetTile(int x, int y);
		void SetTile(int x, int y, T tile);
		
	}
}