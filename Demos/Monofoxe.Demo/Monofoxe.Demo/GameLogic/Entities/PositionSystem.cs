using System;
using System.Collections.Generic;
using Monofoxe.Engine.ECS;

namespace Monofoxe.Demo.GameLogic.Entities
{
	public class PositionSystem : BaseSystem
	{
		public override Type ComponentType => typeof(PositionComponent);

		public override int Priority => 100000; // Previous position should update before everything.

		public override void Create(Component component)
		{
			var position = (PositionComponent)component;

			position.PreviousPosition = position.Position;
		}
		

		public override void Update(List<Component> components)
		{
			foreach(PositionComponent position in components)
			{
				position.PreviousPosition = position.Position;
			}
		}
	}
}
