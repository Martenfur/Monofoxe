using Monofoxe.Engine;
using Monofoxe.Engine.ECS;
using Monofoxe.Engine.SceneSystem;
using Monofoxe.Tiled;
using Monofoxe.Tiled.MapStructure.Objects;
using Monofoxe.ECSTest.Components;

namespace Monofoxe.Test.MapFactories
{
	public class BarrelMapFactory : ITiledEntityFactory
	{
		public string Tag => "barrel";

		public Entity Make(TiledObject obj, Layer layer)
		{
			var barrel = EntityMgr.CreateEntity(layer, "ball");
			barrel.GetComponent<CMovement>().Position = obj.Position;
			barrel.GetComponent<CCollision>().MaskR = obj.Size.X / 2f;
			
			return barrel;
		}
	}
}
