using System;
using Microsoft.Xna.Framework;

namespace Monofoxe.Engine.Utils.Tilemaps
{
	public interface ITilemap<T> where T : struct, ITile
	{
		Vector2 Offset {get; set;}
		int TileWidth {get;}
		int TileHeight {get;}

		int Width {get;}
		int Height {get;}

		T? GetTile(int x, int y);
		void SetTile(int x, int y, T tile);

		/// <summary>
		/// Tells, if given coodrinates are in tilemap's bounds.
		/// </summary>
		bool InBounds(int x, int y);
		
	}
}
