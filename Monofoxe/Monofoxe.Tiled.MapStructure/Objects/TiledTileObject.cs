
namespace Monofoxe.Tiled.MapStructure.Objects
{
	public class TiledTileObject : TiledObject
	{
		public int GID;

		public bool FlipHor;
		public bool FlipVer;

		public TiledMapTileset Tileset;

		public TiledTileObject() {}
		public TiledTileObject(TiledObject obj) : base(obj) {}
	}
}
