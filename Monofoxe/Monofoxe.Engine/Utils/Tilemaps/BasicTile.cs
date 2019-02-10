
namespace Monofoxe.Engine.Utils.Tilemaps
{
	public struct BasicTile : ITile
	{
		public int Index {get; set;}
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

		public ITilesetTile GetTilesetTile()
		{
			if (Tileset == null)
			{
				return null;
			}
			return Tileset.GetTilesetTile(Index);
		}
	}
}
