using Monofoxe.Engine.EC;
using Monofoxe.Engine.SceneSystem;
using Monofoxe.Playground.ECDemo;
using Monofoxe.Tiled;
using Monofoxe.Tiled.MapStructure.Objects;


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
			
			// Note that tile.Tileset will be null. Template tileset is not needed in the game itself,
			// so it is ignored by the map loader. Set __ignoreTileset property to False in tileset 
			// properties in Tiled to make map loader load that tileset.
			// You can also add __ignoreTilesetTexture to the properties instead and set it to True
			// to not load only tileset texture.

			var entity = new Player(layer, tile.Position);

			// Adding a collider componebt
			entity.AddComponent(new TileCollisionComponent());

			return entity;
		}
	}
}
