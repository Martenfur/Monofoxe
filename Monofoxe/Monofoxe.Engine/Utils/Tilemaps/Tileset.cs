using Monofoxe.Engine.Drawing;
using Microsoft.Xna.Framework;

namespace Monofoxe.Engine.Utils.Tilemaps
{
	public class Tileset
	{
		public readonly ITilesetTile[] Tiles;
		
		public readonly int StartingIndex;

		public Vector2 Offset;

		public int Count
		{
			get
			{
				if (Tiles != null)
				{
					return Tiles.Length;
				}
				return 0;
			}
		}

		public Tileset(ITilesetTile[] tiles, Vector2 offset, int startingIndex = 1)
		{
			Tiles = (ITilesetTile[])tiles.Clone();
			Offset = offset;
			StartingIndex = startingIndex;
		}

		/// <summary>
		/// Returns tileset tile according to tile index,
		/// or null, if index is out of tileset's bounds. 
		/// </summary>
		public ITilesetTile GetTilesetTile(int index)
		{
			if (Tiles == null || index < StartingIndex || index >= StartingIndex + Tiles.Length)
			{
				return null;
			}
			return Tiles[index - StartingIndex];
		}

	}
}
