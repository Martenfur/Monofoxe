using Monofoxe.Engine.Drawing;

namespace Monofoxe.Utils.Tilemaps
{
	public class Tileset
	{
		public readonly Sprite Tiles;
		
		public readonly uint StartingIndex;

		public int Count => Tiles.Frames.Length;

		public Tileset(Sprite tiles, uint startingIndex = 1)
		{
			Tiles = tiles;
			StartingIndex = startingIndex;
		}

		/// <summary>
		/// Returns frame according to tile index,
		/// or null, if index is out of tileset's bounds. 
		/// </summary>
		public Frame GetFrame(uint index)
		{
			if (index < StartingIndex || index >= StartingIndex + Tiles.Frames.Length)
			{
				return null;
			}
			return Tiles.Frames[index - StartingIndex];
		}

	}
}
