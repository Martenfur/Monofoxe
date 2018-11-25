using System.Collections.Generic;
using Monofoxe.ECSTest.Components;
using Monofoxe.Engine;
using Monofoxe.Engine.ECS;

namespace Monofoxe.ECSTest.Systems
{
	public class SBirb : BaseSystem
	{
		public override string Tag => "birb";

		public override void Create(Component component) {}

		public override void Destroy(Component component) {}

		public override void Update(List<Component> components) {}
		
		public override void Draw(List<Component> components)
		{
			var birbs = ComponentMgr.GetComponentList<CBirb>(components);

			foreach(var birb in birbs)
			{
				birb.Owner.Depth = -(int)birb.Position.Y;
				DrawMgr.DrawSprite(birb.Spr, birb.Position);
			}
			
		}
	}
}
