using Microsoft.Xna.Framework;

namespace Monofoxe.Tiled.MapStructure.Objects
{
	public class TiledPolygonObject : TiledObject
	{
		public bool Closed;
		public Vector2[] Points;

		public TiledPolygonObject() {}
		public TiledPolygonObject(TiledObject obj) : base(obj) {}
	}
}
