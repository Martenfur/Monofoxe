using System.Collections.Generic;

namespace Pipefoxe.SpriteGroup
{
	public class SpriteGroupData
	{
		public int TextureSize;
		public int TexturePadding;
		public string RootDir;
		public string GroupName;
		public string ClassTemplatePath;
		public string ClassDir;
		public List<RawSprite> Sprites;
		public List<RawSprite> Textures;

		public SpriteGroupData()
		{
			Sprites = new List<RawSprite>();
			Textures = new List<RawSprite>();
		}

	}
}
