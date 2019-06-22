using System;
using System.Collections.Generic;
using Monofoxe.Engine.ECS;
using Monofoxe.Engine.Utils;
using Microsoft.Xna.Framework;
using Monofoxe.Engine.Drawing;

namespace Monofoxe.Playground.ECSDemo
{
	public class ActorSystem : BaseSystem
	{
		public override Type ComponentType => typeof(ActorComponent);

		public override int Priority => 1;

		public override void Create(Component component)
		{

		}


		public override void Update(List<Component> components)
		{
			// During update system picks up all the components on the scene at once.
			foreach(ActorComponent actor in components)
			{
				if (actor.Move)
				{
					// Retrieving the position component from entity.
					var position = actor.Owner.GetComponent<PositionComponent>();

					position.Position += TimeKeeper.GlobalTime(actor.Speed) * GameMath.DirectionToVector2(actor.Direction);
				}
			}
		}

		public override void Draw(Component component)
		{
			var actor = (ActorComponent)component;
			var position = actor.Owner.GetComponent<PositionComponent>();
		
			GraphicsMgr.CurrentColor = Color.White;

			actor.Sprite.Draw(position.Position, actor.Sprite.Origin);
		}

	}
}
