using Microsoft.Xna.Framework;

namespace PipelineExt
{
	public class Frame
	{
		public readonly Rectangle TexturePos;
		public readonly Point Origin;
		public readonly Point Size;
		public readonly string Name;

		public Frame(string name, Point size, Point origin, Rectangle texturePos)
		{
			Name = name;
			Size = size;
			Origin = origin;
			TexturePos = texturePos;
		}
	}
}
