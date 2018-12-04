using System;
using System.Collections.Generic;
using Monofoxe.Engine.ECS;
using Microsoft.Xna.Framework;

namespace Monofoxe.Demo.GameLogic.Entities
{
	public class SolidObjectComponent : Component
	{
		public override string Tag => "solidObject";

		public Vector2 Size;
		
		public override object Clone()
		{
			var positionComponent = new SolidObjectComponent();
			positionComponent.Size = Size;
			
			return positionComponent;
		}
	}
}
