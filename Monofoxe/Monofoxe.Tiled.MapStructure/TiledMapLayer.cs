using System.Collections.Generic;
using Microsoft.Xna.Framework;

namespace Monofoxe.Tiled.MapStructure
{
	public class TiledMapLayer
	{
		public string Name;
		public int ID;
		public bool Visible;
		public float Opacity;
		public Vector2 Offset;
		
		public Dictionary<string, string> Properties;
	}
}
