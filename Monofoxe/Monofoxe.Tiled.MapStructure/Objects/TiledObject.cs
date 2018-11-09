using System.Collections.Generic;
using Microsoft.Xna.Framework;

namespace Monofoxe.Tiled.MapStructure.Objects
{
	/// <summary>
	/// Base tiled object.
	/// </summary>
	public class TiledObject
	{
		
		public string Name;
		public string Type;
		public int ID;
		public Vector2 Position;
		public Vector2 Size;
		public float Rotation;

		public bool Visible;

		public Dictionary<string, string> Properties;

		public TiledObject(){}
		public TiledObject(TiledObject obj)
		{
			// Just copying everything.
			Name = obj.Name;
			Type = obj.Type;
			ID = obj.ID;
			Position = obj.Position;
			Size = obj.Size;
			Rotation = obj.Rotation;
			Visible = obj.Visible;
			Properties = new Dictionary<string, string>(obj.Properties);
		}

	}
}
