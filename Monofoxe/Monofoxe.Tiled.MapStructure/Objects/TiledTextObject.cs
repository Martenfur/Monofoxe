using Microsoft.Xna.Framework;

namespace Monofoxe.Tiled.MapStructure.Objects
{
	public class TiledTextObject : TiledObject
	{
		public string Text;
		public Color Color;
		public bool WordWrap;
		public TiledTextAlign TextAlign;

		public string Font;
		public int FontSize = 12;

		public bool Underlined;
		public bool StrikedOut;
	}
}