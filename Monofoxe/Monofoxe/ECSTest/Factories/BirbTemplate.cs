using Monofoxe.ECSTest.Components;
using Monofoxe.Engine.ECS;
using Monofoxe.Engine.SceneSystem;

namespace Monofoxe.ECSTest.Factories
{
	public class BirbTemplate : IEntityTemplate
	{
		public string Tag => "birb";

		public Entity Make(Layer layer)
		{
			var entity = new Entity(layer, Tag);

			entity.AddComponent(new CBirb());

			return entity;
		}
	}
}
