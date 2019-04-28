using Monofoxe.ECSTest.Components;
using Monofoxe.Engine.ECS;
using Monofoxe.Engine.SceneSystem;

namespace Monofoxe.ECSTest.Factories
{
	public class BallTemplate : IEntityTemplate
	{
		public string Tag => "ball";

		public Entity Make(Layer layer)
		{
			var entity = new Entity(layer, Tag);

			entity.AddComponent(
				new CCollision
				{
					MaskR = 8
				}
			);
			entity.AddComponent(
				new CMovement
				{
					Spr = Resources.Sprites.Special.AutismCat
				}
			);

			return entity;
		}
	}
}
