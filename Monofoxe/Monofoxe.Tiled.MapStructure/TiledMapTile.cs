using System;

namespace Monofoxe.Tiled.MapStructure
{
	[Serializable()]	
	public struct TiledMapTile
	{
		public int GID;
		public bool FlipHor;
		public bool FlipVer;
		public bool FlipDiag;

		public bool IsBlank => GID == 0;
		
	}
}
