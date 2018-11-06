using System.Collections.Generic;
using Microsoft.Xna.Framework;

namespace Monofoxe.Tiled.MapStructure.Objects
{
	/// <summary>
	/// Base tiled object.
	/// 
	/// NOTE: Also represents rectangle and ellipse objects.
	/// </summary>
	public class TiledObject
	{
		public string Name;
		public int ID;
		public Vector2 Position;
		public Vector2 Size;
		public float Rotation;

		public bool Visible;

		public Dictionary<string, string> Properties;
	}
}
