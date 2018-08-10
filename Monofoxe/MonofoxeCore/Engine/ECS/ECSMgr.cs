using System;
using System.Collections.Generic;
using System.Text;
using System.Collections;

namespace Monofoxe.Engine.ECS
{
	public static class ECSMgr
	{
		public static List<ISystem> Systems = new List<ISystem>();	
		static Dictionary<string, List<IComponent>> _components = new Dictionary<string, List<IComponent>>();

		internal static void Update()
		{
			// TODO: Expand fot other events.
			foreach(ISystem system in Systems)
			{
				if (_components.ContainsKey(system.Tag))
				{
					var activeComponents = new List<IComponent>();
					
					// Throwing away inactive components.
					foreach(IComponent component in _components[system.Tag])
					{
						if (component.Owner.Active)
						{
							activeComponents.Add(component);
						}
					}
					
					system.Update(activeComponents);
				}
			}
		}

		internal static void AddComponent(IComponent component)
		{
			if (_components.ContainsKey(component.Tag))
			{
				_components[component.Tag].Add(component);
			}
			else
			{
				var list = new List<IComponent>();
				list.Add(component);
				_components.Add(component.Tag, list);
			}
		}
		
	}
}
