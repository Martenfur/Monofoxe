using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content.Pipeline;

namespace Monofoxe.Pipeline.SpriteGroup
{
	[ContentProcessor(DisplayName = "Sprite Group Processor - Monofoxe")]
	public class SpriteGroupProcessor : ContentProcessor<SpriteGroupData, (List<RawSprite>, List<Bmp>)>
	{
		public override (List<RawSprite>, List<Bmp>) Process(SpriteGroupData groupData, ContentProcessorContext context)
		{
			// Packing sprites into texture atlases.
			var atlasResult = TexturePacker.PackTextures(groupData.Sprites, groupData.AtlasSize, groupData.TexturePadding, groupData.GroupName);
			List<RawSprite> sprites = atlasResult.spriteInfo;
			List<Bmp> atlases = atlasResult.atlases;
			// Packing sprites into texture atlases.
			
			var singleTextureResult = ProcessSingleTextures(atlases.Count, groupData.SingleTextures);
			// Now atlas sprites and singles got same format and we can merge them into one texture\sprite list.
			sprites = sprites.Concat(singleTextureResult.spriteInfo).ToList();
			atlases = atlases.Concat(singleTextureResult.textures).ToList();

			return (sprites, atlases);
		}



		/// <summary>
		/// Single textures can also be spritesheets. 
		/// Function generates frame arrays for input sprites according to sheet data
		/// and splits frames into single textures.
		/// </summary>
		/// <param name="startingIndex">We already got atlases, so we need to index new textures with that in mind.</param>
		/// <param name="sprites"></param>
		/// <returns></returns>
		private (List<RawSprite> spriteInfo, List<Bmp> textures) 
			ProcessSingleTextures(int startingIndex, List<RawSprite> sprites)
		{
			var textures = new List<Bmp>();
			var spriteInfo = new List<RawSprite>();

			var textureIndex = startingIndex;

			// Input sprites got no frames.
			foreach(var sprite in sprites)
			{
				int frameW = sprite.RawTexture.Width / sprite.FramesH;
				int frameH = sprite.RawTexture.Height / sprite.FramesV;

				for(var frameId = 0; frameId < sprite.FramesH * sprite.FramesV; frameId += 1)
				{
					var frame = new Frame();
					frame.TextureIndex = textureIndex;
					frame.TexturePos = new Rectangle(0, 0, frameW, frameH);

					// Calculating texture coordinates out of frame index.
					int x = (frameId % sprite.FramesH) * frame.TexturePos.Width;
					int y = (frameId / sprite.FramesH) * frame.TexturePos.Height;
					
					var texture = new Bmp(frameW, frameH);
					
					// These are single texture sprites. This means that each frame has to be separate texture. 
					texture.Draw(
						sprite.RawTexture,
						0, 0,
						new Rectangle(x, y, frameW, frameH)
					);


					textureIndex += 1;
					sprite.Frames.Add(frame);
					textures.Add(texture);
				}
				spriteInfo.Add(sprite);
			}

			return (spriteInfo, textures);
		}
	}
}
