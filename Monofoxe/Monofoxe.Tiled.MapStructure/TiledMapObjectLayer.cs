using Monofoxe.Tiled.MapStructure.Objects;
using Microsoft.Xna.Framework;

namespace Monofoxe.Tiled.MapStructure
{
	public class TiledMapObjectLayer : TiledMapLayer
	{
		public TiledObject[] Object;
		public TiledMapObjectDrawingOrder DrawingOrder;
		public Color Color;
	}
}
