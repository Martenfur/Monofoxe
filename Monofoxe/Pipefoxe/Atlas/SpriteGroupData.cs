using System.Collections.Generic;

namespace Pipefoxe.Atlas
{
	class SpriteGroupData
	{
		public int TextureSize;
		public int TexturePadding;
		public string RootDir;
		public string GroupName;
		public string ClassTemplatePath;
		public List<RawSprite> Sprites;
		public List<RawSprite> Textures;

		public SpriteGroupData()
		{
			Sprites = new List<RawSprite>();
			Textures = new List<RawSprite>();
		}

	}
}
