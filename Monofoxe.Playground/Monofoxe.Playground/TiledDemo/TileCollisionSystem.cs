using Monofoxe.Engine.ECS;
using Monofoxe.Engine.SceneSystem;
using Monofoxe.Engine.Utils.Tilemaps;
using Monofoxe.Playground.ECSDemo;
using Monofoxe.Playground.TiledDemo.ExtendedMapBuilder;
using System;
using System.Collections.Generic;

namespace Monofoxe.Playground.TiledDemo
{
	public class TileCollisionSystem : BaseSystem
	{
		public override Type ComponentType => typeof(TileCollisionComponent);

		public override int Priority => 1;

		public override void Update(List<Component> components)
		{
			// The most basic and crappy collision system imaginable.
			// The point of it is to just show how to get tile data,
			// and not how to actually implement collisions.

			foreach(TileCollisionComponent collision in components)
			{
				if (collision.Owner.Scene.TryGetLayer("Walls", out Layer layer))
				{
					var position = collision.Owner.GetComponent<PositionComponent>();

					var tilemaps = layer.GetComponentList<BasicTilemapComponent>();
					foreach(BasicTilemapComponent tilemap in tilemaps)
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
		}
		

	}
}
