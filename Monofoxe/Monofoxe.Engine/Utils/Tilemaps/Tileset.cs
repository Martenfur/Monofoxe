using Monofoxe.Engine.Drawing;
using Microsoft.Xna.Framework;

namespace Monofoxe.Engine.Utils.Tilemaps
{
	public class Tileset
	{
		public readonly Sprite Tiles;
		
		public readonly int StartingIndex;

		public Vector2 Offset;

		public int Count
		{
			get
			{
				if (Tiles != null)
				{
					return Tiles.Frames.Length;
				}
				return 0;
			}
		}

		public Tileset(Sprite tiles, Vector2 offset, int startingIndex = 1)
		{
			Tiles = tiles;
			Offset = offset;
			StartingIndex = startingIndex;
		}

		/// <summary>
		/// Returns frame according to tile index,
		/// or null, if index is out of tileset's bounds. 
		/// </summary>
		public Frame GetFrame(int index)
		{
			if (Tiles == null || index < StartingIndex || index >= StartingIndex + Tiles.Frames.Length)
			{
				return null;
			}
			return Tiles.Frames[index - StartingIndex];
		}

	}
}
