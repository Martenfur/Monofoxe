using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Monofoxe.Engine.ECS;
using Microsoft.Xna.Framework;
using Monofoxe.Engine;

namespace Monofoxe
{
	class TestSystem : ISystem, ISystemDrawGUIEvents
	{
		public string Tag => "test";

		
		public void Create(IComponent component)
		{
			((TestComponent)component).Position += Vector2.One * 32;
		}



		public void Update(List<IComponent> components)
		{
			foreach(TestComponent component in components)
			{
				component.Position += Vector2.UnitX;
			}
		}


		public void Draw(List<IComponent> components)
		{
			
		}

		public void Destroy(IComponent component) {}


		public void DrawGUI(List<IComponent> components)
		{
			foreach(TestComponent component in components)
			{
				DrawMgr.DrawCircle(component.Position, 32, false);
			}
		}

	}
}
