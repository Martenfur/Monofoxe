using Microsoft.Xna.Framework;
using Monofoxe.Engine.Drawing;
using Monofoxe.Engine.ECS;
using Monofoxe.Engine.Utils;
using System;
using System.Collections.Generic;

namespace Monofoxe.Playground.ECSDemo
{
	public class ActorSystem : BaseSystem
	{
		public override Type ComponentType => typeof(ActorComponent);

		public override int Priority => 1;

		public override void Create(Component component)
		{

		}

		// Updates at a fixed constant rate, which is defined by
		// GameMgr.FixedUpdateRate. This demo is locked to 60 fps,
		// so FixedUpdate isn't that useful here. But if you want higher refresh rate
		// or dynamic fps (pls don't), it will come in handy.
		public override void FixedUpdate(List<Component> components)
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

					position.PreviousPosition = position.Position;
					position.Position += TimeKeeper.GlobalTime(actor.Speed) * actor.Direction.ToVector2();
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
