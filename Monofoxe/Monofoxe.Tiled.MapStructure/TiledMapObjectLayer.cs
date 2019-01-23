using Microsoft.Xna.Framework;
using Monofoxe.Tiled.MapStructure.Objects;

namespace Monofoxe.Tiled.MapStructure
{
	public class TiledMapObjectLayer : TiledMapLayer
	{
		public TiledObject[] Objects;
		public TiledMapObjectDrawingOrder DrawingOrder;
		public Color Color;
	}
}
