using Microsoft.Xna.Framework;

namespace Monofoxe.Tiled.MapStructure.Objects
{
	public class TiledPointObject : TiledObject
	{
		public new Vector2 Size => Vector2.Zero; // Points cannot have size.

		public TiledPointObject() {}
		public TiledPointObject(TiledObject obj) : base(obj) {}
	}
}
