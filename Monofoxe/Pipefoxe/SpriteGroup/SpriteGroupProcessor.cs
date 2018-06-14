using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework.Content.Pipeline;
using System.IO;
using System.Drawing;
namespace Pipefoxe.SpriteGroup
{
	[ContentProcessor(DisplayName = "Sprite Group Processor - Monofoxe")]
	public class SpriteGroupProcessor : ContentProcessor<SpriteGroupData, int>
	{
		public override int Process(SpriteGroupData input, ContentProcessorContext context)
		{
			TexturePacker.PackTextures(input.Sprites, input.TextureSize, input.TexturePadding, input.GroupName);
			File.Create(@"C:\\Users\gn.fur\Desktop\after.txt");
			return 0;
		}
	}
}
