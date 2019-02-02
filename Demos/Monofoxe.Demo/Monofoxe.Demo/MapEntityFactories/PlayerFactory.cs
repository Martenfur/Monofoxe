using Microsoft.Xna.Framework;
using Monofoxe.Demo.GameLogic.Entities;
using Monofoxe.Engine.ECS;
using Monofoxe.Engine.SceneSystem;
using Monofoxe.Tiled;
using Monofoxe.Tiled.MapStructure.Objects;

namespace Monofoxe.Demo.MapEntityFactories
{
	public class PlayerFactory : ITiledEntityFactory
	{
		public string Tag => "player";

		public Entity Make(TiledObject obj, Layer layer)
		{
			var point = (TiledPointObject)obj;

			var entity = EntityMgr.CreateEntityFromTemplate(layer, "Player");
			entity.GetComponent<PositionComponent>().Position = point.Position;
			
			return entity;
		}
	}
}
