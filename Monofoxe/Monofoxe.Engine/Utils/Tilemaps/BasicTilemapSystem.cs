using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Monofoxe.Engine.ECS;


namespace Monofoxe.Engine.Utils.Tilemaps
{
	/// <summary>
	/// System for basic tilemap. Based on Monofoxe.ECS.
	/// Draws tilemaps in camera's bounds.
	/// </summary>
	public class BasicTilemapSystem : BaseSystem
	{
		public override string Tag => "basicTilemap";

		public override void Draw(Component component)
		{
			var tilemap = (BasicTilemapComponent)component;

			var offsetCameraPos = DrawMgr.CurrentCamera.Pos
				- tilemap.Offset
				- DrawMgr.CurrentCamera.Offset / DrawMgr.CurrentCamera.Zoom;

			var scaledCameraSize = DrawMgr.CurrentCamera.Size / DrawMgr.CurrentCamera.Zoom;
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
			DrawMgr.SwitchPipelineMode(PipelineMode.Sprites);

			for(var y = startY; y < endY; y += 1)
			{
				for(var x = startX; x < endX; x += 1)
				{
					// It's fine to use unsafe get, since we know for sure, we are in bounds.
					var tile = tilemap.GetTileUnsafe(x, y);

					if (!tile.IsBlank)
					{
						var tileFrame = tile.GetFrame();

						if (tileFrame != null)
						{
							var scale = SpriteEffects.None;
							var offset = Vector2.Zero;
							var rotation = 0;

							// A bunch of Tiled magic.
							/*
							 * Ok, so here's the deal.
							 * Monogame, understandibly, has no diagonal flip,
							 * so it's implemented by offsetting, rotating, and horizontal flipping.
							 * Also, order actually matters. Diagonal flip should always go first.
							 * 
							 * Yes, this can be implemented with primitives. 
							 * If you've got nothing better to do -- go bananas.
							 * 
							 * I'm really sorry, if you'll need to modify this.
							 */
							if (tile.FlipDiag)
							{
								rotation = -90;
								offset.Y -= tilemap.TileHeight;
								offset.X -= tileFrame.W - tilemap.TileWidth;

								scale |= SpriteEffects.FlipHorizontally;
							}

							if (tile.FlipHor)
							{
								// Sprite is rotated by 90 degrees, so X axis becomes Y.
								if (tile.FlipDiag)
								{
									scale ^= SpriteEffects.FlipVertically;
								}
								else
								{
									scale ^= SpriteEffects.FlipHorizontally;
								}
							}

							if (tile.FlipVer)
							{
								if (!tile.FlipDiag)
								{
									scale ^= SpriteEffects.FlipVertically;
								}
								else
								{
									scale ^= SpriteEffects.FlipHorizontally;
								}
							}
							// A bunch of Tiled magic.

							// Mass-drawing srpites with spritebatch is a bit faster.
							DrawMgr.Batch.Draw(
								tileFrame.Texture,
								tilemap.Offset + new Vector2(tilemap.TileWidth * x, tilemap.TileHeight * y) - offset,
								tileFrame.TexturePosition,
								Color.White,
								MathHelper.ToRadians(rotation),
								-Vector2.UnitY * tilemap.TileHeight + tileFrame.ParentSprite.Origin + tileFrame.Origin,
								Vector2.One,
								scale,
								0
							);
						}
					}

				}

			}
		}

	}
}
