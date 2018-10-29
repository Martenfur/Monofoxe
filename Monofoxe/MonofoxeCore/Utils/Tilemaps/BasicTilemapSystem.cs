using System;
using Microsoft.Xna.Framework;
using Monofoxe.Engine.Drawing;
using Monofoxe.Engine.ECS;
using Monofoxe.Engine;
using System.Collections.Generic;


namespace Monofoxe.Utils.Tilemaps
{
	/// <summary>
	/// System for basic tilemap. Based on Monofoxe.ECS.
	/// Draws tilemaps in camera's bounds.
	/// </summary>
	public class BasicTilemapSystem : BaseSystem
	{
		public override string Tag => "basicTilemap";

		public override void Draw(List<Component> tilemaps)
		{
			foreach(BasicTilemapComponent tilemap in tilemaps)
			{
				var offsetCameraPos = DrawMgr.CurrentCamera.Pos 
					- tilemap.Offset 
					- DrawMgr.CurrentCamera.Offset / DrawMgr.CurrentCamera.Zoom;

				var scaledCameraSize = DrawMgr.CurrentCamera.Size / DrawMgr.CurrentCamera.Zoom;
				Console.WriteLine("www:" + tilemap.TileWidth);
				var startX = (int)(offsetCameraPos.X / tilemap.TileWidth) - tilemap.Padding;
				var startY = (int)(offsetCameraPos.Y / tilemap.TileHeight) - tilemap.Padding;
				
				var endX = startX + (int)scaledCameraSize.X / tilemap.TileWidth + tilemap.Padding + 2; // One for mama, one for papa.
				var endY = startY + (int)scaledCameraSize.Y / tilemap.TileHeight + tilemap.Padding + 2;
				
				// It's faster to determine bounds for whole region.

				// Bounding.
				if (startX < 0)
				{
					startX = 0;
				}
				if (startY < 0)
				{
					startY = 0;
				}
				if (endX >= tilemap.Width)
				{
					endX = tilemap.Width - 1;
				}
				if (endY >= tilemap.Height)
				{
					endY = tilemap.Height - 1;
				}
				// Bounding.

				for(var y = startY; y < endY; y += 1)
				{
					for(var x = startX; x < endX; x += 1)
					{
						// It's fine to use unsafe get, since we know for sure, we are in bounds.
						var tile = tilemap.GetTileUnsafe(x, y);
						
						if (!tile.IsBlank)
						{
							
							var tileFrame = tile.GetFrame();

							if (x == 0 && y == 0)
							{
								//Console.WriteLine(tileFrame.W + " " + tileFrame.H + " " + tileFrame.TexturePosition);
								var f = new Frame(tileFrame.Texture, tileFrame.Texture.Bounds, Vector2.Zero, tileFrame.Texture.Width, tileFrame.Texture.Height);
								DrawMgr.DrawFrame(f, -Vector2.One * 320, Vector2.Zero);
							}

							if (tileFrame != null)
							{
								var scale = Vector2.One;
								var offset = Vector2.Zero;
	
								if (tile.FlipHor)
								{
									offset.X = -tileFrame.W;
									scale.X = -1;
								}
								if (tile.FlipVer)
								{
									offset.Y = -tileFrame.H;
									scale.Y = -1;
								}
								// TODO: Add tile rotation.
								DrawMgr.DrawFrame(
									tileFrame,
									tilemap.Offset + new Vector2(tilemap.TileWidth * x, tilemap.TileHeight * y),
									// We need to subtract image height, because Tiled does so. : - )
									offset - Vector2.UnitY * tilemap.TileHeight + tileFrame.ParentSprite.Origin, 
									scale,
									0,
									Color.White
								);
							}
						}
					}
				}
				
			}
		}

	}
}
