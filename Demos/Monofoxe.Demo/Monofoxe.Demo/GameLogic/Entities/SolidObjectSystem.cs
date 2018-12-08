using System;
using Monofoxe.Engine;
using Monofoxe.Engine.ECS;


namespace Monofoxe.Demo.GameLogic.Entities
{
	public class SolidObjectSystem : BaseSystem
	{
		public override Type ComponentType => typeof(SolidObjectComponent);

		public override void Draw(Component component)
		{
			var solid = (SolidObjectComponent)component;
			var position = solid.Owner.GetComponent<PositionComponent>();

			DrawMgr.DrawRectangle(
				position.Position - solid.Size / 2,
				position.Position + solid.Size / 2,
				true
			);

			DrawMgr.DrawCircle(
				position.PreviousPosition,
				8,
				true
			);

		}


	}
}
