using System;
using System.Collections.Generic;
using Monofoxe.Engine.ECS;
using Monofoxe.ECSTest.Components;

namespace Monofoxe.ECSTest.Systems
{
	public class SCollision : ISystem, ISystemFixedUpdateEvents
	{
		public string Tag => "Collision";

		public void Create(Component component) {}

		public void Destroy(Component component) {}

		public void Update(List<Component> components) {}


		public void FixedUpdate(List<Component> components) 
		{
			
			foreach(CCollision collider in components)
			{
				ComponentSystemMgr.GetComponentList("");
			}
		}

		public void FixedUpdateBegin(List<Component> components) {}
		public void FixedUpdateEnd(List<Component> components) {}

		public void Draw(List<Component> components) {}
	}
}
