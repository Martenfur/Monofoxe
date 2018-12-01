using System;
using System.Collections.Generic;
using Monofoxe.Engine.ECS;
using Microsoft.Xna.Framework;

namespace Monofoxe.Demo.GameLogic.Entities
{
	public class PhysicsObjectComponent : Component
	{
		public override string Tag => "physicsObject";
		
		
		public Vector2 Size;
		public Vector2 Speed;
		
		public override object Clone()
		{
			var physicsObjectComponent = new PhysicsObjectComponent();
			physicsObjectComponent.Size = Size;
			physicsObjectComponent.Speed = Speed;

			return physicsObjectComponent;
		}
	}
}
