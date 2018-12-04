using System.Collections.Generic;
using Monofoxe.ECSTest.Components;
using Monofoxe.Engine;
using Monofoxe.Engine.ECS;
using System;

namespace Monofoxe.ECSTest.Systems
{
	public class SBirb : BaseSystem
	{
		public override Type ComponentType => typeof(CBirb);

		public override int Priority => 1000;

		public override void Create(Component component) {}

		public override void Destroy(Component component) {}

		public override void Update(List<Component> components) 
		{
		}
		
		public override void Draw(Component component)
		{
			var birb = (CBirb)component;

			birb.Owner.Depth = -(int)birb.Position.Y;
			DrawMgr.DrawSprite(birb.Spr, birb.Position);
		}
	}
}
