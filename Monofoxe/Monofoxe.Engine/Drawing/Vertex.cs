using Microsoft.Xna.Framework;

namespace Monofoxe.Engine.Drawing
{
	public struct Vertex
	{
		public Vector2 Position;
		public Color Color;
		public Vector2 TexturePosition;

		public Vertex(Vector2 position)
		{
			Position = position;
			Color = DrawMgr.CurrentColor;
			TexturePosition = Vector2.Zero;
		}

		public Vertex(Vector2 position, Color color)
		{
			Position = position;
			Color = color;
			TexturePosition = Vector2.Zero;
		}

		public Vertex(Vector2 position, Color color, Vector2 texturePosition)
		{
			Position = position;
			Color = color;
			TexturePosition = texturePosition;
		}

	}
}
