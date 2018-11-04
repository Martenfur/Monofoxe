using Monofoxe.Engine.Drawing;

namespace Monofoxe.Utils.Tilemaps
{
	public struct BasicTile : ITile
	{
		public int Index {get; private set;}
		public bool IsBlank => Tileset == null || Index == (Tileset.StartingIndex - 1);

		public bool FlipHor {get; set;}
		public bool FlipVer {get; set;}
		public bool FlipDiag {get; set;}

		public Tileset Tileset;

		public BasicTile(int index, Tileset tileset, bool flipHor = false, bool flipVer = false, bool flipDiag = false)
		{
			Index = index;
			Tileset = tileset;
			FlipHor = flipHor;
			FlipVer = flipVer;
			FlipDiag = flipDiag;
		}

		public Frame GetFrame() =>
			Tileset.GetFrame(Index);
		
	}
}
