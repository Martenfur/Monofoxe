using System;
using System.Collections.Generic;
using Monofoxe.Engine.ECS;
using Monofoxe.Engine;
using Monofoxe.Engine.Utils.Cameras;

namespace Monofoxe.Demo.GameLogic.Entities
{
	public class PlayerSystem : BaseSystem
	{
		public override Type ComponentType => typeof(PlayerComponent);

		public override int Priority => 1;

		
		public override void Update(List<Component> components)
		{
			foreach(PlayerComponent player in components)
			{
				var physics = player.Owner.GetComponent<PhysicsComponent>();
			
				if (Input.CheckButton(player.Left))
				{
					physics.Speed.X = -player.WalkSpeed;
				}
				if (Input.CheckButton(player.Right))
				{
					physics.Speed.X = player.WalkSpeed;
				}

				if (!Input.CheckButton(player.Left) && !Input.CheckButton(player.Right))
				{
					physics.Speed.X = 0;
				}

				if (Input.CheckButtonPress(player.Jump))
				{
					physics.Speed.Y = -player.JumpSpeed;
				}

				Test.Camera.Pos = player.Owner.GetComponent<PositionComponent>().Position.ToPoint().ToVector2() 
				- Test.Camera.Size / 2;
			
			}
			
		}

		public override void Draw(Component component)
		{
			var physics = component.Owner.GetComponent<PhysicsComponent>();
			var position = component.Owner.GetComponent<PositionComponent>();

			DrawMgr.CurrentColor = physics.Color;

			DrawMgr.DrawRectangle(
				position.Position.ToPoint().ToVector2() - physics.Size / 2,
				position.Position.ToPoint().ToVector2() + physics.Size / 2,
				true
			);
			/*
			DrawMgr.DrawCircle(
				position.PreviousPosition.ToPoint().ToVector2(),
				8,
				true
			);*/
			//DrawMgr.CurrentColor = Color.Black;
		}
	}
}
