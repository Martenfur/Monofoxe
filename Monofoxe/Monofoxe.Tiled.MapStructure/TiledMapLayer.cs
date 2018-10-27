using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;

namespace Monofoxe.Tiled.MapStructure
{
	[Serializable()]
	public abstract class TiledMapLayer
	{
		public string Name;
		public int ID;
		public Dictionary<string, string> Properties;
		public bool Visible;
		public float Opacity;
		public Vector2 Offset;
	}
}
