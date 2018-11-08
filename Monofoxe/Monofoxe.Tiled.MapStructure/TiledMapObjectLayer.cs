using Monofoxe.Tiled.MapStructure.Objects;
using Microsoft.Xna.Framework;

namespace Monofoxe.Tiled.MapStructure
{
	public class TiledMapObjectLayer : TiledMapLayer
	{
		public TiledObject[] Objects;
		public TiledMapObjectDrawingOrder DrawingOrder;
		public Color Color;
	}
}
