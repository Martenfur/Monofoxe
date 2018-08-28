using System;
using System.Collections.Generic;
using System.Linq;
using System.Collections;

namespace Monofoxe.Engine.ECS
{
	public static class ComponentSystemMgr
	{
		/// <summary>
		/// List of systems.
		/// </summary>
		public static List<ISystem> Systems = new List<ISystem>();

		/// <summary>
		/// Component dictionary.
		/// </summary>
		static Dictionary<string, List<Component>> _components = new Dictionary<string, List<Component>>();
		
		/// <summary>
		/// Newly created components. Used for Create event.
		/// </summary>
		static Dictionary<string, List<Component>> _newComponents = new Dictionary<string, List<Component>>();

		static Dictionary<string, List<Component>> _depthSortedComponents = new Dictionary<string, List<Component>>();


		class ComponentCollection
		{
			Dictionary<string, List<Component>> _newComponents, _components;
			
			public ComponentCollection()
			{
				_newComponents = new Dictionary<string, List<Component>>();
				_components = new Dictionary<string, List<Component>>();
			}
			

			internal void AddComponent(Component component)
			{
				if (_newComponents.ContainsKey(component.Tag))
				{
					_newComponents[component.Tag].Add(component);
				}
				else
				{
					var list = new List<Component>();
					list.Add(component);
					_newComponents.Add(component.Tag, list);
				}
			}



		}


		public static void Implementing()
		{
			var items = 
			AppDomain.CurrentDomain.GetAssemblies().SelectMany(
				x => x.GetTypes()
			).Where(
				mytype => typeof(ISystem).IsAssignableFrom(mytype) 
				&& mytype.GetInterfaces().Contains(typeof(ISystem))
			); 
			foreach(var item in items) 
			{
				Console.WriteLine(item.FullName); 
			}
		}

		#region Events.

		/*
		 * For event explanation, see Entity. 
		 */

		internal static void Create()
		{
			foreach(ISystem system in Systems)
			{
				if (_newComponents.ContainsKey(system.Tag))
				{
					foreach(Component component in _newComponents[system.Tag])
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



		internal static void AddComponent(Component component)
		{
			// TODO: Add automatic system management.
			if (_newComponents.ContainsKey(component.Tag))
			{
				_newComponents[component.Tag].Add(component);
			}
			else
			{
				var list = new List<Component>();
				list.Add(component);
				_newComponents.Add(component.Tag, list);
			}
		}
		

		internal static void RemoveComponent(Component component)
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
		

		internal static void SortComponentsByDepth()
		{
			_depthSortedComponents.Clear();
			foreach(KeyValuePair<string, List<Component>> list in _components)
			{
				_depthSortedComponents.Add(list.Key, list.Value.OrderByDescending(o => o.Owner.Depth).ToList());
			}
		}



		/// <summary>
		/// Filters out inactive components.
		/// Component is inactive, if its owner is inactive.
		/// </summary>
		static List<Component> FilterInactiveComponnets(List<Component> components)
		{
			var activeComponents = new List<Component>();
					
			foreach(Component component in components)
			{
				if (component.Owner.Active)
				{
					activeComponents.Add(component);
				}
			}
			return activeComponents;	
		}



		/// <summary>
		/// Invokes Create event for corresponding system.
		/// Create events are usually executed at the beginning of each step,
		/// so components stay uninitialized for a bit right after creation.
		/// In most cases it's fine, but you may need to init your component
		/// right here and right now.
		/// </summary>
		public static void InitComponent(Component component)
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
		/// Returns list of components with given tag.
		/// </summary>
		public static List<Component> GetComponentList(string tag, List<Component> components)
		{
			var list = new List<Component>();

			foreach(Component component in components)
			{
				list.Add(component.Owner[tag]);
			}

			return list;
		}


		/// <summary>
		/// Returns list of components with given tag.
		/// </summary>
		public static List<T> GetComponentList<T>(List<Component> components) where T : Component
		{
			var list = new List<T>();

			foreach(Component component in components)
			{
				list.Add(component.Owner.GetComponent<T>());
			}
			return list;
		}

	}
}
