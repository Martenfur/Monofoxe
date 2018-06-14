using System;
using System.Collections.Generic;
using System.Linq;
using System.Drawing;
using System.IO;
using Newtonsoft.Json.Linq;

namespace Pipefoxe.SpriteGroup
{
	static class TexturePacker
	{
		public static void PackTextures(List<RawSprite> textures, int textureSize, int padding, string groupName)
		{
			Console.WriteLine("Packing textures...");
			List<RawSprite> sprites = Pack(textures, textureSize, padding);
			Console.WriteLine("Done! " + sprites.Count + " atlases packed.");
			
			var atlasIndex = 0;
			var atlasBmp = new Bitmap(textureSize, textureSize);
			var graphics = Graphics.FromImage(atlasBmp);
			graphics.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.NearestNeighbor;
			
			while(sprites.Count > 0)
			{
				var spritesCopy = new List<RawSprite>(sprites);

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
								sprites.Remove(sprite);
							}

							int y = i / sprite.FramesH;
							int x = i % sprite.FramesH;

							graphics.DrawImage(
								sprite.RawTexture,
								frame.TexturePos,
								new Rectangle(
									x * frame.TexturePos.Width,
									y * frame.TexturePos.Height,
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
							 /*
							Rectangle[] srcRects = 
					{
						new Rectangle(
							texture.TexturePosition.X, 
							texture.TexturePosition.Y, 
							1,
							texture.TexturePosition.Height 
						),
						new Rectangle(
							texture.TexturePosition.X + texture.TexturePosition.Width - 1, 
							texture.TexturePosition.Y, 
							1,
							texture.TexturePosition.Height 
						),
						new Rectangle(
							texture.TexturePosition.X, 
							texture.TexturePosition.Y, 
							texture.TexturePosition.Width, 
							1
						),
						new Rectangle(
							texture.TexturePosition.X, 
							texture.TexturePosition.Y + texture.TexturePosition.Height - 1,
							texture.TexturePosition.Width,
							1
						),
					};
							Rectangle[] destRects = 
					{
						new Rectangle(
							texture.Position.X - 1, 
							texture.Position.Y, 
							1, 
							texture.TexturePosition.Height
						),
						new Rectangle(
							texture.Position.X + texture.TexturePosition.Width, 
							texture.Position.Y,
							1, 
							texture.TexturePosition.Height
						),
						new Rectangle(
							texture.Position.X, 
							texture.Position.Y - 1, 
							texture.TexturePosition.Width,
							1
						),
						new Rectangle(
							texture.Position.X, 
							texture.Position.Y + texture.TexturePosition.Height,
							texture.TexturePosition.Width,
							1
						),
					};

							for(var side = 0; side < 4; side += 1) // 4 sides, 4 lines.
							{
								// DrawImage draws 2 pixels of source texture instead of 1 if scaled, for some reason.
								// So we have to draw a ton of lines without scaling.
								for(var i = 0; i < padding; i += 1) 
								{
									var destRect = destRects[side];

									int iAdd = i * ((side % 2) * 2 - 1);
							
									if (side < 2)
									{
										destRect.X += iAdd;
									}
									else
									{
										destRect.Y += iAdd;
									}

									graphics.DrawImage(
										texture.Texture, 
										destRect,
										srcRects[side],
										GraphicsUnit.Pixel
									);
								}
							}
							*/
							#endregion Padding.
						}
					}
				}
				
				
				var outName = Environment.CurrentDirectory + "/" + groupName + "_" + atlasIndex;
				atlasBmp.Save(outName + ".png");

				graphics.Clear(Color.Transparent);
				atlasIndex += 1;
			}
			
			graphics.Dispose();
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
