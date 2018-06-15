using System;
using System.Collections.Generic;
using System.Linq;
using System.Drawing;
using System.IO;

namespace Pipefoxe.SpriteGroup
{
	static class TexturePacker
	{
		public static (List<RawSprite> spriteInfo, List<Bitmap> atlases) 
		PackTextures(List<RawSprite> textures, int textureSize, int padding, string groupName)
		{
			List<RawSprite> sprites = Pack(textures, textureSize, padding);
			List<Bitmap> atlases = AssembleAtlases(sprites, textureSize, padding);
			
			return (sprites, atlases);
		}

		private static List<RawSprite> Pack(List<RawSprite> textures, int textureSize, int padding)
		{
			// Checking for textures larger than atlas size. 
			foreach(RawSprite texture in textures)
			{
				if (texture.RawTexture.Width / texture.FramesH > textureSize || texture.RawTexture.Height / texture.FramesV > textureSize)
				{
					throw(new Exception("Cannot pack " + texture.Name + "! It's too big for " + textureSize + "x" + textureSize + " atlas!"));
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
	
				var grid = new TextureGrid(textureSize);
				
				// We need to copy list, because sortedTextures has to be changed during enumeration.
				var sortedTexturesCopy = new List<RawSprite>(sortedSprites);

				foreach(RawSprite sprite in sortedTexturesCopy)
				{
					var spriteFrameSize = new Rectangle(
						0, 
						0, 
						sprite.RawTexture.Width / sprite.FramesH, 
						sprite.RawTexture.Height / sprite.FramesV
					);
					
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
								&& rightBound <= textureSize - padding
								&& bottomBound <= textureSize - padding)
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
									grid[y, x] = true;
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

		private static List<Bitmap> AssembleAtlases(List<RawSprite> sprites, int textureSize, int padding)
		{
			var atlases = new List<Bitmap>();
			var atlasIndex = 0;
			
			var spritesEnum = new List<RawSprite>(sprites);

			while(spritesEnum.Count > 0)
			{
				var atlasBmp = new Bitmap(textureSize, textureSize);
				var graphics = Graphics.FromImage(atlasBmp);
				graphics.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.NearestNeighbor;
			
				var spritesCopy = new List<RawSprite>(spritesEnum);

				foreach(RawSprite sprite in spritesCopy)
				{
					for(var i = sprite.RenderedFrames; i < sprite.FramesV * sprite.FramesH; i += 1)			
					{
						var frame = sprite.Frames[i];

						if (frame.TextureIndex == atlasIndex)
						{
							sprite.RenderedFrames += 1;

							if (sprite.RenderedFrames == sprite.FramesH * sprite.FramesV)
							{
								spritesEnum.Remove(sprite);
							}

							int x = (i % sprite.FramesH) * frame.TexturePos.Width;
							int y = (i / sprite.FramesH) * frame.TexturePos.Height;
							
							graphics.DrawImage(
								sprite.RawTexture,
								frame.TexturePos,
								new Rectangle(
									x,
									y,
									frame.TexturePos.Width, 
									frame.TexturePos.Height
								),  
								GraphicsUnit.Pixel
							);

							#region Padding.
							/*
							 * Drawing padding lines around the texture turned out to be harder than I thought.
							 * Let's just pretend it's all pretty and readable.
							 */
							
							Rectangle[] srcRects =
							{
								new Rectangle(x, y, 1, frame.TexturePos.Height),
								new Rectangle(x + frame.TexturePos.Width - 1, y, 1, frame.TexturePos.Height),
								new Rectangle(x, y, frame.TexturePos.Width, 1),
								new Rectangle(x, y + frame.TexturePos.Height - 1, frame.TexturePos.Width, 1),
							};
							Rectangle[] destRects =
							{
								new Rectangle(frame.TexturePos.X - 1, frame.TexturePos.Y, 1, frame.TexturePos.Height),
								new Rectangle(frame.TexturePos.X + frame.TexturePos.Width, frame.TexturePos.Y, 1, frame.TexturePos.Height),
								new Rectangle(frame.TexturePos.X, frame.TexturePos.Y - 1, frame.TexturePos.Width, 1),
								new Rectangle(frame.TexturePos.X, frame.TexturePos.Y + frame.TexturePos.Height, frame.TexturePos.Width, 1),
							};

							for (var side = 0; side < 4; side += 1) // 4 sides, 4 lines.
							{
								// DrawImage draws 2 pixels of source texture instead of 1 if scaled, for some reason.
								// So we have to draw a ton of lines without scaling.
								for (var l = 0; l < padding; l += 1)
								{
									var destRect = destRects[side];

									int lAdd = l * ((side % 2) * 2 - 1);

									if (side < 2)
									{
										destRect.X += lAdd;
									}
									else
									{
										destRect.Y += lAdd;
									}

									graphics.DrawImage(
										sprite.RawTexture,
										destRect,
										srcRects[side],
										GraphicsUnit.Pixel
									);
								}
							}

							#endregion Padding.
						}
					}
				}
				
				atlases.Add(atlasBmp);
				graphics.Dispose();
				atlasIndex += 1;
			}
			return atlases;
		}



		/// <summary>
		/// Checks if one texture overlaps with the rest.
		/// </summary>
		private static bool CheckOverlap(Frame frame, List<Frame> textures, int padding)
		{
			var paddingOffset = new Size(padding, padding);
			foreach(Frame tex in textures)
			{
				if (
					RectangleInRectangle(
						frame.TexturePos.Location - paddingOffset, 
						frame.TexturePos.Location + frame.TexturePos.Size + paddingOffset, 
						tex.TexturePos.Location - paddingOffset, 
						tex.TexturePos.Location + tex.TexturePos.Size + paddingOffset
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
		public static bool RectangleInRectangle(Point rect1Pt1, Point rect1Pt2, Point rect2Pt1, Point rect2Pt2) =>
			rect1Pt1.X < rect2Pt2.X && rect1Pt2.X > rect2Pt1.X && rect1Pt1.Y < rect2Pt2.Y && rect1Pt2.Y > rect2Pt1.Y;

		private static bool IsPow2(int x) =>
			(x & (x - 1)) == 0;

	}
}
