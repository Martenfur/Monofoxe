using System;
using System.Collections.Generic;
using Monofoxe.Engine.ECS;
using Microsoft.Xna.Framework;
using Monofoxe.Engine;

namespace Monofoxe
{
	class TestSystem : ISystem, ISystemDrawGUIEvents
	{
		public string Tag => "test";

		
		public void Create(Component component)
		{
			((TestComponent)component).Position += Vector2.One * 32;
		}



		public void Update(List<Component> components)
		{
			foreach(TestComponent component in components)
			{
				component.Position += Vector2.UnitX;
			}
		}


		public void Draw(List<Component> components)
		{
			
		}

		public void Destroy(Component component) {}


		public void DrawGUI(List<Component> components)
		{
			foreach(TestComponent component in components)
			{
				DrawMgr.DrawCircle(component.Position, 32, false);
			}
		}

	}
}
