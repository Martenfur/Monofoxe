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

		static Dictionary<string, List<IComponent>> _depthSortedComponents = new Dictionary<string, List<IComponent>>();
		
		internal static void SortComponentsByDepth()
		{
			_depthSortedComponents.Clear();
			foreach(KeyValuePair<string, List<IComponent>> list in _components)
			{
				_depthSortedComponents.Add(list.Key, list.Value.OrderByDescending(o => o.Owner.Depth).ToList());
			}
		}

		#region Events.

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
				}
			}
			_newComponents.Clear();
		}



		internal static void FixedUpdateBegin()
		{
			foreach(ISystem system in Systems)
			{
				if (system is ISystemFixedUpdateEvents && _components.ContainsKey(system.Tag))
				{
					((ISystemFixedUpdateEvents)system).FixedUpdateBegin(FilterInactiveComponnets(_components[system.Tag]));
				}
			}
		}

		internal static void FixedUpdate()
		{
			foreach(ISystem system in Systems)
			{
				if (system is ISystemFixedUpdateEvents && _components.ContainsKey(system.Tag))
				{
					((ISystemFixedUpdateEvents)system).FixedUpdate(FilterInactiveComponnets(_components[system.Tag]));
				}
			}
		}

		internal static void FixedUpdateEnd()
		{
			foreach(ISystem system in Systems)
			{
				if (system is ISystemFixedUpdateEvents && _components.ContainsKey(system.Tag))
				{
					((ISystemFixedUpdateEvents)system).FixedUpdateEnd(FilterInactiveComponnets(_components[system.Tag]));
				}
			}
		}




		internal static void UpdateBegin()
		{
			foreach(ISystem system in Systems)
			{
				if (system is ISystemExtEvents &&  _components.ContainsKey(system.Tag))
				{
					((ISystemExtEvents)system).UpdateBegin(FilterInactiveComponnets(_components[system.Tag]));
				}
			}
		}

		internal static void Update()
		{
			foreach(ISystem system in Systems)
			{
				if (_components.ContainsKey(system.Tag))
				{
					system.Update(FilterInactiveComponnets(_components[system.Tag]));
				}
			}
		}

		internal static void UpdateEnd()
		{
			foreach(ISystem system in Systems)
			{
				if (system is ISystemExtEvents &&  _components.ContainsKey(system.Tag))
				{
					((ISystemExtEvents)system).UpdateEnd(FilterInactiveComponnets(_components[system.Tag]));
				}
			}
		}

		

		internal static void DrawBegin()
		{
			foreach(ISystem system in Systems)
			{
				if (system is ISystemExtEvents && _depthSortedComponents.ContainsKey(system.Tag))
				{
					((ISystemExtEvents)system).DrawBegin(FilterInactiveComponnets(_depthSortedComponents[system.Tag]));
				}
			}
		}

		internal static void Draw()
		{
			foreach(ISystem system in Systems)
			{
				if (_depthSortedComponents.ContainsKey(system.Tag))
				{
					system.Draw(FilterInactiveComponnets(_depthSortedComponents[system.Tag]));
				}
			}
		}

		internal static void DrawEnd()
		{
			foreach(ISystem system in Systems)
			{
				if (system is ISystemExtEvents && _depthSortedComponents.ContainsKey(system.Tag))
				{
					((ISystemExtEvents)system).DrawEnd(FilterInactiveComponnets(_depthSortedComponents[system.Tag]));
				}
			}
		}

		internal static void DrawGUI()
		{
			foreach(ISystem system in Systems)
			{
				if (system is ISystemDrawGUIEvents && _depthSortedComponents.ContainsKey(system.Tag))
				{
					((ISystemDrawGUIEvents)system).DrawGUI(FilterInactiveComponnets(_depthSortedComponents[system.Tag]));
				}
			}
		}


		#endregion Events.



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
			// Removing from lists.
			if (_newComponents.ContainsKey(component.Tag))
			{
				_newComponents[component.Tag].Remove(component);
			}
			if (_components.ContainsKey(component.Tag))
			{
				_components[component.Tag].Remove(component);
			}

			// Performing Destroy event.
			foreach(ISystem system in Systems)
			{
				if (component.Tag == system.Tag)
				{
					system.Destroy(component);
				}
			}
		}
		


		/// <summary>
		/// Invokes Create event for corresponding system.
		/// Create events are usually executed at the beginning of each step,
		/// so components stay uninitialized for a bit right after creation.
		/// In most cases it's fine, but you may need to init your component
		/// right here and right now.
		/// </summary>
		public static void InitComponent(IComponent component)
		{
			// If component is even there.
			if (_newComponents.ContainsKey(component.Tag) && _newComponents[component.Tag].Contains(component))
			{
				foreach(ISystem system in Systems)
				{
					if (component.Tag == system.Tag)
					{
						system.Create(component);
					}
				}
				_newComponents.Remove(component.Tag);
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
