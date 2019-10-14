using System;
using System.Collections.Generic;
using Monofoxe.Engine.ECS;
using Monofoxe.Engine.Utils;
using Microsoft.Xna.Framework;
using Monofoxe.Engine.Drawing;

namespace Monofoxe.Playground.ECSDemo
{
	public class Botystem : BaseSystem
	{
		public override Type ComponentType => typeof(BotComponent);

		public override int Priority => 1;

		public override void Create(Component component)
		{

		}


		public override void Update(List<Component> components)
		{
			foreach(BotComponent bot in components)
			{
				var actor = bot.Owner.GetComponent<ActorComponent>();
				actor.Move = true;
				actor.Direction += TimeKeeper.GlobalTime(bot.TurningSpeed); // ni-ni-ni-ni-ni-ni-ni-ni-ni-ni-ni-ni-ni-ni
			}
		}

		public override void Draw(Component component)
		{
			var actor = (ActorComponent)component;
			var position = actor.Owner.GetComponent<PositionComponent>();

			actor.Sprite.Draw(position.Position);
		}

	}
}
