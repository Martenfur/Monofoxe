using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Monofoxe.Engine.ECS;
using Microsoft.Xna.Framework;
using Monofoxe.Engine;

namespace Monofoxe.Demo.GameLogic.Entities
{
	public class PhysicsObjectSystem : BaseSystem
	{
		public override string Tag => "physicsObject";

		public override void Update(List<Component> components)
		{
			foreach(PhysicsObjectComponent physicsObject in components)
			{
				var positionComponent = physicsObject.Owner.GetComponent<PositionComponent>();
				positionComponent.Position = Input.MousePos;//physicsObject.Speed;




			}
		}

		public override void Draw(Component component)
		{
			var physicsObject = (PhysicsObjectComponent)component;
			var positionComponent = physicsObject.Owner.GetComponent<PositionComponent>();

			DrawMgr.DrawRectangle(
				positionComponent.Position - physicsObject.Size / 2,
				positionComponent.Position + physicsObject.Size / 2,
				true
			);

			DrawMgr.DrawCircle(
				positionComponent.PreviousPosition,
				8,
				true
			);

		}

	}
}
