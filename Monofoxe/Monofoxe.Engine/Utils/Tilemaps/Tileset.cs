using Monofoxe.Engine.Drawing;

namespace Monofoxe.Engine.Utils.Tilemaps
{
	public class Tileset
	{
		public readonly Sprite Tiles;
		
		public readonly int StartingIndex;

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

		public Tileset(Sprite tiles, int startingIndex = 1)
		{
			Tiles = tiles;
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
