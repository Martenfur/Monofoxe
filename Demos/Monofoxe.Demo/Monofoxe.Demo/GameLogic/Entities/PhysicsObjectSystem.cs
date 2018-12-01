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
				physicsObject.Position += physicsObject.Speed;




			}
		}

		public override void Draw(Component component)
		{
			var physicsObject = (PhysicsObjectComponent)component;
			
			DrawMgr.DrawRectangle(
				physicsObject.Position - physicsObject.Size / 2,
				physicsObject.Position + physicsObject.Size / 2,
				true
			);
		}

	}
}
