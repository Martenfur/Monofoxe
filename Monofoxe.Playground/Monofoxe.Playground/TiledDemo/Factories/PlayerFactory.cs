using Monofoxe.Engine.ECS;
using Monofoxe.Engine.SceneSystem;
using Monofoxe.Playground.ECSDemo;
using Monofoxe.Tiled;
using Monofoxe.Tiled.MapStructure.Objects;
using Monofoxe.Playground.TiledDemo.ExtendedMapBuilder;


namespace Monofoxe.Playground.TiledDemo.Factories
{
	// Factories are used by map builder to create actual entities out of 
	// Tiled templates.

	public class PlayerFactory : ITiledEntityFactory
	{
		// Tag should be the same as Type property of the template.
		public string Tag => "Player";

		public Entity Make(TiledObject obj, Layer layer, MapBuilder map)
		{
			var tile = (TiledTileObject)obj;
			
			var entity = new Player(layer, tile.Position);

			entity.AddComponent(new CollisionComponent());

			return entity;
		}
	}
}
