using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;

namespace Monofoxe.Tiled.MapStructure
{
	public class TiledMapLayer
	{
		public string Name;
		public Dictionary<string, string> Properties;
		public bool Visible;
		public float Opacity;
		public Vector2 Offset;
	}
}
