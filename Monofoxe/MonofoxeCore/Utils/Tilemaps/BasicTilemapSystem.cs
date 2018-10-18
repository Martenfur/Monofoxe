using System;
using Microsoft.Xna.Framework;
using Monofoxe.Engine.Drawing;
using Monofoxe.Engine.ECS;
using Monofoxe.Engine;
using System.Collections.Generic;


namespace Monofoxe.Utils.Tilemaps
{
	public class BasicTilemapSystem : BaseSystem
	{
		public override string Tag => "basicTilemap";
		
		public override void Draw(List<Component> tilemaps)
		{
			DrawMgr.CurrentColor = Color.White;
			foreach(BasicTilemapComponent tilemap in tilemaps)
			{
				var offsetCameraPos = DrawMgr.CurrentCamera.Pos - DrawMgr.CurrentCamera.Offset / DrawMgr.CurrentCamera.Zoom;
				var scaledCameraSize = DrawMgr.CurrentCamera.Size / DrawMgr.CurrentCamera.Zoom;

				var startX = (int)(offsetCameraPos.X / tilemap.TileWidth);
				var startY = (int)(offsetCameraPos.Y / tilemap.TileHeight);
				
				var endX = startX + (int)scaledCameraSize.X / tilemap.TileWidth + 2; // One for mama, one for papa.
				var endY = startY + (int)scaledCameraSize.Y / tilemap.TileHeight + 2;
				
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
						
						if (!tile.IsBlank && tile.GetFrame() != null)
						{
							Vector2 scale = Vector2.One;
							Vector2 offset = Vector2.Zero;

							if (tile.FlipHor)
							{
								offset.X = tilemap.TileWidth;
								scale.X = -1;
							}
							if (tile.FlipVer)
							{
								offset.Y = tilemap.TileHeight;
								scale.Y = -1;
							}


							DrawMgr.DrawFrame(
								tile.GetFrame(),
								tilemap.Offset + new Vector2(tilemap.TileWidth * x, tilemap.TileHeight * y),
								offset,
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
