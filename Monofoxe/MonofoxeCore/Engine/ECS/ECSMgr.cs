using System;
using System.Collections.Generic;
using System.Linq;
using System.Collections;

namespace Monofoxe.Engine.ECS
{
	public static class ECSMgr
	{
		public static List<ISystem> Systems = new List<ISystem>();	
		static Dictionary<string, List<IComponent>> _components = new Dictionary<string, List<IComponent>>();
		
		/// <summary>
		/// Used for Create event.
		/// </summary>
		static Dictionary<string, List<IComponent>> _newComponents = new Dictionary<string, List<IComponent>>();

		
		internal static void Create()
		{
			foreach(ISystem system in Systems)
			{
				if (_newComponents.ContainsKey(system.Tag))
				{
					foreach(IComponent component in _newComponents[system.Tag])
					{
						system.Create(component);
					}

					if (_components.ContainsKey(system.Tag))
					{
						_components[system.Tag].AddRange(_newComponents[system.Tag]);
					}
					else
					{
						_components.Add(system.Tag, _newComponents[system.Tag]);
					}

					_newComponents.Remove(system.Tag);
				}
			}
		}



		internal static void Update()
		{
			// TODO: Expand for other events.
			foreach(ISystem system in Systems)
			{
				if (_components.ContainsKey(system.Tag))
				{
					system.Update(FilterInactiveComponnets(_components[system.Tag]));
				}
			}
		}

		internal static void Draw()
		{
			// TODO: Expand for other events.
			foreach(ISystem system in Systems)
			{
				if (_components.ContainsKey(system.Tag))
				{
					var depthSortedComponents = _components[system.Tag].OrderByDescending(o => o.Owner.Depth).ToList();
					
					system.Draw(FilterInactiveComponnets(depthSortedComponents));
				}
			}
		}


		internal static void AddComponent(IComponent component)
		{
			if (_newComponents.ContainsKey(component.Tag))
			{
				_newComponents[component.Tag].Add(component);
			}
			else
			{
				var list = new List<IComponent>();
				list.Add(component);
				_newComponents.Add(component.Tag, list);
			}
		}
		

		internal static void RemoveComponent(IComponent component)
		{
			if (_newComponents.ContainsKey(component.Tag))
			{
				_newComponents[component.Tag].Remove(component);
			}
			if (_components.ContainsKey(component.Tag))
			{
				_components[component.Tag].Remove(component);
			}
		}
		

		/// <summary>
		/// Filters out inactive components.
		/// </summary>
		static List<IComponent> FilterInactiveComponnets(List<IComponent> components)
		{
			var activeComponents = new List<IComponent>();
					
			foreach(IComponent component in components)
			{
				if (component.Owner.Active)
				{
					activeComponents.Add(component);
				}
			}
			return activeComponents;	
		}

	}
}
