using System;
using Microsoft.Xna.Framework;

namespace Monofoxe.Utils.Tilemaps
{
	public interface ITilemap<T> where T : struct, ITile
	{
		Vector2 Offset {get; set;}
		uint TileWidth {get;}
		uint TileHeight {get;}

		uint Width {get;}
		uint Height {get;}

		
		T GetTile(int x, int y);
		void SetTile(int x, int y, T tile);

		void Update();
		void Draw();
	}
}
