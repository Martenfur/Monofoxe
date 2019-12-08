using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Monofoxe.Engine.Drawing;
using Monofoxe.Engine.EC;
using Monofoxe.Engine.SceneSystem;

namespace Monofoxe.Engine.Utils.Tilemaps
{
	/// <summary>
	/// Basic tilemap class. Provides basic functionality,
	/// supports camera zooming.
	/// </summary>
	public class BasicTilemap : Entity, ITilemap<BasicTile>
	{
		protected BasicTile[,] _tileGrid;

		public Vector2 Offset { get; set; } = Vector2.Zero;

		public int TileWidth { get; protected set; }
		public int TileHeight { get; protected set; }

		public int Width { get; protected set; }
		public int Height { get; protected set; }

		/// <summary>
		/// Tells how many tile rows and columns will be drawn outside of camera's bounds.
		/// May be useful for tiles larger than the grid. 
		/// </summary>
		public int Padding = 0;


		public BasicTilemap(Layer layer, int width, int height, int tileWidth, int tileHeight) : base(layer)
		{
			Visible = true;

			Width = width;
			Height = height;
			TileWidth = tileWidth;
			TileHeight = tileHeight;
			_tileGrid = new BasicTile[Width, Height];
		}
		

		public BasicTile? GetTile(int x, int y)
		{
			if (InBounds(x, y))
			{
				return _tileGrid[x, y];
			}
			return null;
		}

		public void SetTile(int x, int y, BasicTile tile)
		{
			if (InBounds(x, y))
			{
				_tileGrid[x, y] = tile;
			}
		}

		/// <summary>
		/// Returns tile without out-of-bounds check.
		/// WARNING: This method will throw an exception, if coordinates are out of bounds.
		/// </summary>
		public BasicTile GetTileUnsafe(int x, int y) =>
			_tileGrid[x, y];

		/// <summary>
		/// Sets tile without out-of-bounds check.
		/// WARNING: This method will throw an exception, if coordinates are out of bounds.
		/// </summary>
		public BasicTile SetTileUnsafe(int x, int y, BasicTile tile) =>
			_tileGrid[x, y] = tile;


		/// <summary>
		/// Tells if given coordinates are in bounds.
		/// </summary>
		public bool InBounds(int x, int y) =>
			x >= 0 && y >= 0 && x < Width && y < Height;
			
		

		public override void Draw()
		{
			var offsetCameraPos = GraphicsMgr.CurrentCamera.Position
				- Offset
				- GraphicsMgr.CurrentCamera.Offset / GraphicsMgr.CurrentCamera.Zoom;

			var scaledCameraSize = GraphicsMgr.CurrentCamera.Size / GraphicsMgr.CurrentCamera.Zoom;
			var startX = (int)(offsetCameraPos.X / TileWidth) - Padding;
			var startY = (int)(offsetCameraPos.Y / TileHeight) - Padding;

			var endX = startX + (int)scaledCameraSize.X / TileWidth + Padding + 2; // One for mama, one for papa.
			var endY = startY + (int)scaledCameraSize.Y / TileHeight + Padding + 2;

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
			if (endX > Width)
			{
				endX = Width;
			}
			if (endY > Height)
			{
				endY = Height;
			}
			// Bounding.

			// Telling whatever is waiting to be drawn to draw itself.
			// If not flushed, drawing raw sprite batch may interfere with primitives.
			GraphicsMgr.VertexBatch.FlushBatch();

			for (var y = startY; y < endY; y += 1)
			{
				for (var x = startX; x < endX; x += 1)
				{
					// It's fine to use unsafe get, since we know for sure, we are in bounds.
					var tile = GetTileUnsafe(x, y);

					if (!tile.IsBlank)
					{
						var tilesetTile = tile.GetTilesetTile();

						if (tilesetTile != null)
						{
							var flip = SpriteFlipFlags.None;
							var offset = Vector2.UnitY * (tilesetTile.Frame.Height - TileHeight);
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
								offset.Y = -TileHeight;
								offset.X = 0;

								flip |= SpriteFlipFlags.FlipHorizontally;
							}

							if (tile.FlipHor)
							{
								// Sprite is rotated by 90 degrees, so X axis becomes Y.
								if (tile.FlipDiag)
								{
									flip ^= SpriteFlipFlags.FlipVertically;
								}
								else
								{
									flip ^= SpriteFlipFlags.FlipHorizontally;
								}
							}

							if (tile.FlipVer)
							{
								if (tile.FlipDiag)
								{
									flip ^= SpriteFlipFlags.FlipHorizontally;
								}
								else
								{
									flip ^= SpriteFlipFlags.FlipVertically;
								}
							}
							// A bunch of Tiled magic.
							/*
							// Mass-drawing srpites with spritebatch is a bit faster.
							GraphicsMgr._batch.Draw(
								tilesetTile.Frame.Texture,
								Offset + new Vector2(TileWidth * x, TileHeight * y) - offset + tile.Tileset.Offset,
								tilesetTile.Frame.TexturePosition,
								Color.White,
								MathHelper.ToRadians(rotation),
								Vector2.Zero,
								Vector2.One,
								flip,
								0
							);*/
							// TODO: Fix.
						}
					}

				}

			}
		}
		
	}
}
