using Microsoft.Xna.Framework;

namespace Monofoxe.Engine.Drawing
{
	public struct Vertex
	{
		public Vector3 Position;
		public Color Color;
		public Vector2 TexturePosition;

		public Vertex(Vector2 position) 
			: this(position.ToVector3()) 
		{ }
		public Vertex(Vector2 position, Color color) 
			: this(position.ToVector3(), color) 
		{ }
		public Vertex(Vector2 position, Color color, Vector2 texturePosition) 
			: this(position.ToVector3(), color, texturePosition) 
		{ }



		public Vertex(Vector3 position)
		{
			Position = position;
			Color = GraphicsMgr.CurrentColor;
			TexturePosition = Vector2.Zero;
		}

		public Vertex(Vector3 position, Color color)
		{
			Position = position;
			Color = color;
			TexturePosition = Vector2.Zero;
		}

		public Vertex(Vector3 position, Color color, Vector2 texturePosition)
		{
			Position = position;
			Color = color;
			TexturePosition = texturePosition;
		}

	}
}
