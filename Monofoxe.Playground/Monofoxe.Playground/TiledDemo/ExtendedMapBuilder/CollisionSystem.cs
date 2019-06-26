using System;
using System.Collections.Generic;
using Monofoxe.Engine.ECS;
using Monofoxe.Engine.Utils;
using Monofoxe.Engine.Utils.Tilemaps;
using Monofoxe.Engine.SceneSystem;
using Microsoft.Xna.Framework;
using Monofoxe.Engine.Drawing;
using Monofoxe.Playground.ECSDemo;

namespace Monofoxe.Playground.TiledDemo.ExtendedMapBuilder
{
	public class CollisionSystem : BaseSystem
	{
		public override Type ComponentType => typeof(CollisionComponent);

		public override int Priority => 1;

		public override void Create(Component component)
		{

		}


		public override void Update(List<Component> components)
		{
			// The most basic and crappy collision system imaginable.
			// The point of it is to just show how to get tile data,
			// and not how to actually implement collisions.

			foreach(CollisionComponent collision in components)
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
							var ttile = (SolidTilesetTile)tile.Value.GetTilesetTile();
							if (ttile != null && ttile.Solid) // If this tile is solid - perform "collision."
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
