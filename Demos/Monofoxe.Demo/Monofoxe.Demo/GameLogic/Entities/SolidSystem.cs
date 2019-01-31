using System;
using Monofoxe.Engine;
using Monofoxe.Engine.ECS;


namespace Monofoxe.Demo.GameLogic.Entities
{
	public class SolidSystem : BaseSystem
	{
		public override Type ComponentType => typeof(SolidComponent);

		public override void Draw(Component component)
		{
			var solid = (SolidComponent)component;
			var position = solid.Owner.GetComponent<PositionComponent>();

			DrawMgr.DrawRectangle(
				position.Position - solid.Size / 2,
				position.Position + solid.Size / 2,
				false
			);
			

		}


	}
}
