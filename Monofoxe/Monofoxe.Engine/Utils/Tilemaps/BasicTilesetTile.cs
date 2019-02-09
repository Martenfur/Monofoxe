using Monofoxe.Engine.Drawing;

namespace Monofoxe.Engine.Utils.Tilemaps
{
	public class BasicTilesetTile : ITilesetTile
	{
		public Frame Frame {get; private set;}

		public BasicTilesetTile(Frame frame) =>
			Frame = frame;
	}
}
