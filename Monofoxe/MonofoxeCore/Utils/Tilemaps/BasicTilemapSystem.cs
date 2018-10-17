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
			foreach(BasicTilemapComponent tilemap in tilemaps)
			{
				var offsetCameraPos = DrawMgr.CurrentCamera.Pos - DrawMgr.CurrentCamera.Offset / DrawMgr.CurrentCamera.Zoom;
				var scaledCameraSize = DrawMgr.CurrentCamera.Size / DrawMgr.CurrentCamera.Zoom;

				var startX = (int)(offsetCameraPos.X / tilemap.TileWidth);
				var startY = (int)(offsetCameraPos.Y / tilemap.TileHeight);
				
				var endX = startX + (int)scaledCameraSize.X / tilemap.TileWidth + 2;
				var endY = startY + (int)scaledCameraSize.Y / tilemap.TileHeight + 2;

				if (startX < 0)
				{
					startX = 0;
				}
				if (startY < 0)
				{
					startY = 0;
				}

				for(var y = startY; y < endY; y += 1)
				{
					for(var x = startX; x < endX; x += 1)
					{
						var tile = tilemap.GetTile(x, y);

						if (!tile.IsBlank)
						{
							DrawMgr.DrawFrame(
								tile.GetFrame(), 
								tilemap.Offset + new Vector2(tilemap.TileWidth * x, tilemap.TileHeight * y), 
								Vector2.Zero
							);
						}
					}
				}
			}
		}

	}
}
