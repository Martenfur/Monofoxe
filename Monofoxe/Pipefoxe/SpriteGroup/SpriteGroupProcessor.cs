using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework.Content.Pipeline;
using System.Drawing;
using System.IO;

namespace Pipefoxe.SpriteGroup
{
	[ContentProcessor(DisplayName = "Sprite Group Processor - Monofoxe")]
	public class SpriteGroupProcessor : ContentProcessor<SpriteGroupData, (List<RawSprite>, List<Bitmap>)>
	{
		public override (List<RawSprite>, List<Bitmap>) Process(SpriteGroupData groupData, ContentProcessorContext context)
		{
			// Packing sprites into texture atlases.
			var atlasResult = TexturePacker.PackTextures(groupData.Sprites, groupData.TextureSize, groupData.TexturePadding, groupData.GroupName);
			List<RawSprite> sprites = atlasResult.spriteInfo;
			List<Bitmap> atlases = atlasResult.atlases;
			// Packing sprites into texture atlases.
			
			var singleTextureResult = ProcessSingleTextures(atlases.Count, groupData.Textures);
			sprites = sprites.Concat(singleTextureResult.spriteInfo).ToList();
			atlases = atlases.Concat(singleTextureResult.textures).ToList();
			
			// Debug.
			var index = 0;
			foreach(Bitmap atlas in atlases)
			{
				atlas.Save(Environment.CurrentDirectory + '/' + groupData.GroupName + "_debug/" + groupData.GroupName + '_' + index + ".png");
				index += 1;
			}
			// Debug.

			ClassGenerator.Generate(
				groupData.RootDir + '/' + groupData.ClassTemplatePath, 
				Environment.CurrentDirectory + groupData.ClassDir, 
				sprites,
				groupData.GroupName
			);

			return (sprites, atlases);
		}


		private (List<RawSprite> spriteInfo, List<Bitmap> textures) 
		ProcessSingleTextures(int startingIndex, List<RawSprite> sprites)
		{
			var textures = new List<Bitmap>();
			var spriteInfo = new List<RawSprite>();

			var textureIndex = startingIndex;
			foreach(RawSprite sprite in sprites)
			{
				int frameW = sprite.RawTexture.Width / sprite.FramesH;
				int frameH = sprite.RawTexture.Height / sprite.FramesV;

				for(var frameId = 0; frameId < sprite.FramesH * sprite.FramesV; frameId += 1)
				{
					var frame = new Frame();
					frame.TextureIndex = textureIndex;
					frame.TexturePos = new Rectangle(0, 0, frameW, frameH);

					int x = (frameId % sprite.FramesH) * frame.TexturePos.Width;
					int y = (frameId / sprite.FramesH) * frame.TexturePos.Height;
					
					var texture = new Bitmap(frameW, frameH);
					var graphics = Graphics.FromImage(texture);
					graphics.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.NearestNeighbor;
					
					graphics.DrawImage(
						sprite.RawTexture,
						frame.TexturePos,
						new Rectangle(x, y, frameW, frameH),
						GraphicsUnit.Pixel
					);

					graphics.Dispose();

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
