using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Pipefoxe.SpriteGroup
{
	/// <summary>
	/// Packs set of spritesheets into texture atlasses.
	/// </summary>
	static class TexturePacker
	{
		public static (List<RawSprite> spriteInfo, List<Bmp> atlases) 
			PackTextures(List<RawSprite> textures, int textureSize, int padding, string groupName)
		{
			List<RawSprite> sprites = Pack(textures, textureSize, padding);
			List<Bmp> atlases = AssembleAtlases(sprites, textureSize, padding);
			
			return (sprites, atlases);
		}



		/// <summary>
		/// Calculates sprite positions for texture atlases.
		/// Basic algorhitm:
		/// In the beginning we have 1x1 grid and list of sprites sorted by height.
		/// We take biggest sprite and try to fit it into each free cell of a grid.
		/// If sprite fits, we place it into this cell and split grid at the right
		/// and bottom bound of placed sprite.
		/// </summary>
		private static List<RawSprite> Pack(List<RawSprite> textures, int atlasSize, int padding)
		{
			// Checking for textures larger than atlas size. 
			foreach(var texture in textures)
			{
				if (texture.RawTexture.Width / texture.FramesH > atlasSize || texture.RawTexture.Height / texture.FramesV > atlasSize)
				{
					throw new Exception("Cannot pack " + texture.Name + "! It's too big for " + atlasSize + "x" + atlasSize + " atlas!");
				}
			}
			// Checking for textures larger than atlas size. 

			// Sorting textures by height. 
			List<RawSprite> sortedSprites = textures.OrderByDescending(o => o.RawTexture.Height / o.FramesV).ToList();

			var textureIndex = 0;
			
			var packedSprites = new List<RawSprite>();
			while(sortedSprites.Count > 0)
			{
				var packedFrames = new List<Frame>();
	
				var grid = new TextureGrid(atlasSize);
				
				// We need to copy list, because sortedTextures has to be changed during enumeration.
				var sortedTexturesCopy = new List<RawSprite>(sortedSprites);

				foreach(var sprite in sortedTexturesCopy)
				{
					var spriteFrameSize = new Rectangle(
						0, 
						0, 
						sprite.RawTexture.Width / sprite.FramesH, 
						sprite.RawTexture.Height / sprite.FramesV
					);
					
					// Sprite consists out of multiple frames. Each should be placed separately.
					for(var frameId = 0; frameId < sprite.FramesH * sprite.FramesV; frameId += 1)
					{
						var placed = false; // Tells if texture has been placed.

						var frame = new Frame();
						frame.TextureIndex = textureIndex;
						frame.TexturePos = spriteFrameSize;

						for(var y = 0; y < grid.Height; y += 1)
						{
							for(var x = 0; x < grid.Width; x += 1)
							{
								int rightBound = grid.GetCellX(x) + frame.TexturePos.Width + padding;
								int bottomBound = grid.GetCellY(y) + frame.TexturePos.Height + padding;

								frame.TexturePos.X = grid.GetCellX(x) + padding;
								frame.TexturePos.Y = grid.GetCellY(y) + padding;
								
								if (!grid[y, x]
									&& !CheckOverlap(frame, packedFrames, padding)
									&& rightBound <= atlasSize - padding
									&& bottomBound <= atlasSize - padding
								)
								{
									packedFrames.Add(frame);
									sprite.Frames.Add(frame);

									// This means that we've packed all frames in sprite.
									if (sprite.Frames.Count == sprite.FramesH * sprite.FramesV) 
									{
										packedSprites.Add(sprite);
										sortedSprites.Remove(sprite);
									}

									grid.SplitH(rightBound + padding);
									grid.SplitV(bottomBound + padding);

									// In case frame takes more than one cell.
									for(var fillY = y; fillY < grid.Height; fillY += 1)
									{
										if (grid.GetCellY(fillY) >= bottomBound + padding)
										{
											break;
										}
										for(var fillX = x; fillX < grid.Width; fillX += 1)
										{
											if (grid.GetCellX(fillX) >= rightBound + padding)
											{
												break;
											}
											grid[fillY, fillX] = true;
										}
									}
									// In case frame takes more than one cell.
						
									placed = true;
									break;
								}
							}
							if (placed)
							{
								break;
							}
						}
						
						if (!placed) // No need to check rest of the frames if we cannot fit current one.
						{
							break;
						}
					}
				}
				
				textureIndex += 1;
				
			}
			

			return packedSprites;
		}



		/// <summary>
		/// Generates atlas textures out of sprite info.
		/// </summary>
		private static List<Bmp> AssembleAtlases(List<RawSprite> sprites, int textureSize, int padding)
		{
			var atlases = new List<Bmp>();
			var atlasIndex = 0;

			// List of unrendered sprites. When all sprite's frames are rendered, it will be removed from list.
			var spritesEnum = new List<RawSprite>(sprites); 

			while(spritesEnum.Count > 0)
			{
				var atlasBmp = new Bmp(textureSize, textureSize);

				var spritesCopy = new List<RawSprite>(spritesEnum);

				foreach(var sprite in spritesCopy)
				{
					for(var i = sprite.RenderedFrames; i < sprite.FramesV * sprite.FramesH; i += 1)			
					{
						var frame = sprite.Frames[i];

						if (frame.TextureIndex == atlasIndex) // Each frame may be on different atlas.
						{
							sprite.RenderedFrames += 1;

							if (sprite.RenderedFrames == sprite.FramesH * sprite.FramesV)
							{
								spritesEnum.Remove(sprite);
							}

							var frameX = (i % sprite.FramesH) * frame.TexturePos.Width;
							var frameY = (i / sprite.FramesH) * frame.TexturePos.Height;
							
							atlasBmp.Draw(
								sprite.RawTexture,
								frame.TexturePos.X,
								frame.TexturePos.Y,
								new Rectangle(
									frameX,
									frameY,
									frame.TexturePos.Width, 
									frame.TexturePos.Height
								),
								padding
							);
						}
					}
				}
				
				atlases.Add(atlasBmp);
				atlasIndex += 1;
			}
			return atlases;
		}



		/// <summary>
		/// Checks if one texture overlaps with the rest.
		/// </summary>
		private static bool CheckOverlap(Frame frame, List<Frame> frames, int padding)
		{
			var paddingOffset = new Point(padding, padding);
			foreach(var secondFrame in frames)
			{
				if (
					RectangleInRectangle(
						frame.TexturePos.Location - paddingOffset, 
						frame.TexturePos.Location + frame.TexturePos.Size + paddingOffset, 
						secondFrame.TexturePos.Location - paddingOffset, 
						secondFrame.TexturePos.Location + secondFrame.TexturePos.Size + paddingOffset
					)
				)
				{
					return true;
				}
			}
			return false;
		}



		/// <summary>
		/// Checks if two rectangles intersect.
		/// </summary>
		private static bool RectangleInRectangle(Point rect1Pt1, Point rect1Pt2, Point rect2Pt1, Point rect2Pt2) =>
			rect1Pt1.X < rect2Pt2.X && rect1Pt2.X > rect2Pt1.X && rect1Pt1.Y < rect2Pt2.Y && rect1Pt2.Y > rect2Pt1.Y;
		
		
		
		private static bool IsPow2(int x) =>
			(x & (x - 1)) == 0;

	}
}
