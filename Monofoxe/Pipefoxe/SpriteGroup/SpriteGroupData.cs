using System.Collections.Generic;

namespace Pipefoxe.SpriteGroup
{
	public class SpriteGroupData
	{
		/// <summary>
		/// Size of a single texture atlas. Should be power of 2.
		/// </summary>
		public int AtlasSize;

		/// <summary>
		/// Space between textures on atlas. Will be filled with border pixels.
		/// </summary>
		public int TexturePadding;

		/// <summary>
		/// Full path to source sprites.
		/// </summary>
		public string RootDir;

		/// <summary>
		/// Name of sprite group. Equals to file name.
		/// </summary>
		public string GroupName;

		public List<RawSprite> Sprites = new List<RawSprite>();
		public List<RawSprite> SingleTextures = new List<RawSprite>();
	}
}
