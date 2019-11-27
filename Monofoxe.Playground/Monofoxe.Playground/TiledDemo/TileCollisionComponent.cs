using Monofoxe.Engine.ECS;
using Monofoxe.Engine.SceneSystem;
using Monofoxe.Engine.Utils.Tilemaps;
using Monofoxe.Playground.ECSDemo;
using Monofoxe.Playground.TiledDemo.ExtendedMapBuilder;

namespace Monofoxe.Playground.TiledDemo
{
	/// <summary>
	/// A component used for actors to enable tile collisions.
	/// </summary>
	public class TileCollisionComponent : Component
	{

		public override void FixedUpdate()
		{
			throw new System.NotImplementedException();
		}

		public override void Update()
		{
			// The most basic and crappy collision system imaginable.
			// The point of it is to just show how to get tile data,
			// and not how to actually implement collisions.


			if (Owner.Scene.TryGetLayer("Walls", out Layer layer))
			{
				var position = Owner.GetComponent<PositionComponent>();

				var tilemaps = layer.GetEntityList<BasicTilemap>();

				foreach (BasicTilemap tilemap in tilemaps)
				{
					// Getting the tile player is currently standing on.
					var tile = tilemap.GetTile(
						(int)(position.Position.X / tilemap.TileWidth),
						(int)(position.Position.Y / tilemap.TileHeight)
					);

					if (tile != null)
					{
						var tilesetTile = tile.Value.GetTilesetTile();

						if (
							tilesetTile != null
							&& tilesetTile is SolidTilesetTile
							&& ((SolidTilesetTile)tilesetTile).Solid
						) // If this tile is solid - perform "collision."
						{
							position.Position = position.PreviousPosition;
						}

					}
				}
			}


		}


		public override void Draw()
		{
			throw new System.NotImplementedException();
		}

		public override void Destroy()
		{
			throw new System.NotImplementedException();
		}

	}


}
