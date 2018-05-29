using System.Collections.Generic;
using Microsoft.Xna.Framework.Content.Pipeline.Graphics;

namespace PipelineExt
{
	public class AtlassSprites
	{
		public List<Sprite> Sprites;
		public TextureContent Texture;

		public AtlassSprites() =>
			Sprites = new List<Sprite>();
		
		public void Add(Sprite sprite) =>
			Sprites.Add(sprite);
	}
}

