using Microsoft.Xna.Framework;

namespace Monofoxe.Tiled.MapStructure.Objects
{
	public class TiledEllipseObject : TiledObject
	{
		public Vector2 Center => Position + Size / 2f;
	}
}
