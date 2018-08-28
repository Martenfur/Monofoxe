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
		static Dictionary<string, ISystem> _activeSystems = new Dictionary<string, ISystem>();

		static Dictionary<string, ISystem> _systemPool = new Dictionary<string, ISystem>();


		/// <summary>
		/// Component dictionary.
		/// </summary>
		static Dictionary<string, List<Component>> _components = new Dictionary<string, List<Component>>();
		
		/// <summary>
		/// Newly created components. Used for Create event.
		/// </summary>
		static List<Component> _newComponents = new List<Component>();

		static Dictionary<string, List<Component>> _depthSortedComponents = new Dictionary<string, List<Component>>();

		/// <summary>
		/// Creates an instance of each ISystem implementing class.
		/// </summary>
		public static void LoadSystemPool()
		{
			/* 
			 * ZOMG TEH DRAMA :o
			 * This ancient horror gets list of all classes, which implement
			 * ISystem interface.
			 */
			var systemTypes = AppDomain.CurrentDomain.GetAssemblies().SelectMany(x => x.GetTypes());
			systemTypes = systemTypes.Where(
				mytype => typeof(ISystem).IsAssignableFrom(mytype) 
				&& mytype.GetInterfaces().Contains(typeof(ISystem))
			);

			foreach(Type systemType in systemTypes)
			{
				var newSystem = (ISystem)Activator.CreateInstance(systemType);
				_systemPool.Add(newSystem.Tag, newSystem);
			}

		}

		#region Events.

		/*
		 * For event explanation, see Entity. 
		 */

		internal static void Create()
		{
			// Managing new components.
			if (_newComponents.Count > 0)
			{
				foreach(var component in _newComponents)
				{
					if (!_activeSystems.ContainsKey(component.Tag))
					{
						if (_systemPool.ContainsKey(component.Tag))
						{
							var newSystem = _systemPool[component.Tag];
							_activeSystems.Add(component.Tag, newSystem);
							newSystem.Create(component);
						}
					}
					else
					{
						_activeSystems[component.Tag].Create(component);
					}

					if (_components.ContainsKey(component.Tag))
					{
						_components[component.Tag].Add(component);
					}
					else
					{
						var list = new List<Component>(new Component[]{component});
						_components.Add(component.Tag, list);
					}
				}
				_newComponents.Clear();
			}

			// Disabling systems without components.
			foreach(var componentListPair in _components.ToList())
			{
				if (componentListPair.Value.Count == 0)
				{
					_components.Remove(componentListPair.Key);
				}
			}
		}



		internal static void FixedUpdateBegin()
		{
			foreach(var systemPair in _activeSystems)
			{
				var system = systemPair.Value;
				if (system is ISystemFixedUpdateEvents && _components.ContainsKey(system.Tag))
				{
					((ISystemFixedUpdateEvents)system).FixedUpdateBegin(FilterInactiveComponnets(_components[system.Tag]));
				}
			}
		}

		internal static void FixedUpdate()
		{
			foreach(var systemPair in _activeSystems)
			{
				var system = systemPair.Value;
				if (system is ISystemFixedUpdateEvents && _components.ContainsKey(system.Tag))
				{
					((ISystemFixedUpdateEvents)system).FixedUpdate(FilterInactiveComponnets(_components[system.Tag]));
				}
			}
		}

		internal static void FixedUpdateEnd()
		{
			foreach(var systemPair in _activeSystems)
			{
				var system = systemPair.Value;
				if (system is ISystemFixedUpdateEvents && _components.ContainsKey(system.Tag))
				{
					((ISystemFixedUpdateEvents)system).FixedUpdateEnd(FilterInactiveComponnets(_components[system.Tag]));
				}
			}
		}




		internal static void UpdateBegin()
		{
			foreach(var systemPair in _activeSystems)
			{
				var system = systemPair.Value;
				if (system is ISystemExtEvents &&  _components.ContainsKey(system.Tag))
				{
					((ISystemExtEvents)system).UpdateBegin(FilterInactiveComponnets(_components[system.Tag]));
				}
			}
		}

		internal static void Update()
		{
			foreach(var systemPair in _activeSystems)
			{
				var system = systemPair.Value;
				if (_components.ContainsKey(system.Tag))
				{
					system.Update(FilterInactiveComponnets(_components[system.Tag]));
				}
			}
		}

		internal static void UpdateEnd()
		{
			foreach(var systemPair in _activeSystems)
			{
				var system = systemPair.Value;
				if (system is ISystemExtEvents &&  _components.ContainsKey(system.Tag))
				{
					((ISystemExtEvents)system).UpdateEnd(FilterInactiveComponnets(_components[system.Tag]));
				}
			}
		}

		

		internal static void DrawBegin()
		{
			foreach(var systemPair in _activeSystems)
			{
				var system = systemPair.Value;
				if (system is ISystemExtEvents && _depthSortedComponents.ContainsKey(system.Tag))
				{
					((ISystemExtEvents)system).DrawBegin(FilterInactiveComponnets(_depthSortedComponents[system.Tag]));
				}
			}
		}

		internal static void Draw()
		{
			foreach(var systemPair in _activeSystems)
			{
				var system = systemPair.Value;
				if (_depthSortedComponents.ContainsKey(system.Tag))
				{
					system.Draw(FilterInactiveComponnets(_depthSortedComponents[system.Tag]));
				}
			}
		}

		internal static void DrawEnd()
		{
			foreach(var systemPair in _activeSystems)
			{
				var system = systemPair.Value;
				if (system is ISystemExtEvents && _depthSortedComponents.ContainsKey(system.Tag))
				{
					((ISystemExtEvents)system).DrawEnd(FilterInactiveComponnets(_depthSortedComponents[system.Tag]));
				}
			}
		}

		internal static void DrawGUI()
		{
			foreach(var systemPair in _activeSystems)
			{
				var system = systemPair.Value;
				if (system is ISystemDrawGUIEvents && _depthSortedComponents.ContainsKey(system.Tag))
				{
					((ISystemDrawGUIEvents)system).DrawGUI(FilterInactiveComponnets(_depthSortedComponents[system.Tag]));
				}
			}
		}


		#endregion Events.



		internal static void AddComponent(Component component)
		{
			_newComponents.Add(component);
		}
		

		internal static void RemoveComponent(Component component)
		{
			// Removing from lists.
			_newComponents.Remove(component);
			if (_components.ContainsKey(component.Tag))
			{
				_components[component.Tag].Remove(component);
			}

			// Performing Destroy event.
			if (_activeSystems.ContainsKey(component.Tag))
			{
				_activeSystems[component.Tag].Destroy(component);
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
			if (_newComponents.Contains(component) && _activeSystems.ContainsKey(component.Tag))
			{
				_activeSystems[component.Tag].Create(component);
				_newComponents.Remove(component);
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
