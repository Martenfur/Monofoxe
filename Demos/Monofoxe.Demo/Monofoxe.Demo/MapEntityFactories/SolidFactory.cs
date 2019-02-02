using Monofoxe.Demo.GameLogic.Entities;
using Monofoxe.Engine.ECS;
using Monofoxe.Engine.SceneSystem;
using Monofoxe.Tiled;
using Monofoxe.Tiled.MapStructure.Objects;

namespace Monofoxe.Demo.MapEntityFactories
{
	public class SolidFactory : ITiledEntityFactory
	{
		public string Tag => "solid";

		public Entity Make(TiledObject obj, Layer layer)
		{
			System.Console.WriteLine("Making solid!");
			var rectangle = (TiledRectangleObject)obj;

			var entity = EntityMgr.CreateEntityFromTemplate(layer, "SolidBoi");
			entity.GetComponent<PositionComponent>().Position = rectangle.Position + rectangle.Size / 2f;
			entity.GetComponent<SolidComponent>().Size = rectangle.Size;

			return entity;
		}
	}
}
