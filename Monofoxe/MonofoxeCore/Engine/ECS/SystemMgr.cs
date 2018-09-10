using System;
using System.Collections.Generic;
using System.Linq;
using Monofoxe.Engine.Drawing;

namespace Monofoxe.Engine.ECS
{
	public static class SystemMgr
	{
		/// <summary>
		/// List of currently active systems.
		/// </summary>
		internal static Dictionary<string, ISystem> _activeSystems = new Dictionary<string, ISystem>();

		/// <summary>
		/// Pool of all game systems.
		/// </summary>
		internal static Dictionary<string, ISystem> _systemPool = new Dictionary<string, ISystem>();

		public static int __dbgSysCount => _activeSystems.Count; // REMOVE
		public static int __dbgSysPoolCount => _systemPool.Count; // REMOVE


		/// <summary>
		/// If true, systems will be enabled and disabled automatically.
		/// </summary>
		public static bool AutoSystemManagement 
		{
			get => _autoSystemManagement;
			set
			{
				_autoSystemManagement = value;
				foreach(var layer in Layer.Layers)
				{
					layer._componentsWereRemoved = true;
				}
			}
		}
		static bool _autoSystemManagement = true;


		#region Events.

		/*
		 * For event explanation, see Entity. 
		 */
		
		internal static void FixedUpdateBegin()
		{
			var components = GetActiveComponents();
			foreach(var systemPair in _activeSystems)
			{
				var system = systemPair.Value;
				if (system is ISystemFixedUpdateEvents && components.ContainsKey(system.Tag))
				{
					((ISystemFixedUpdateEvents)system).FixedUpdateBegin(components[system.Tag]);
				}
			}
		}

		internal static void FixedUpdate()
		{
			var components = GetActiveComponents();
			foreach(var systemPair in _activeSystems)
			{
				var system = systemPair.Value;
				if (system is ISystemFixedUpdateEvents && components.ContainsKey(system.Tag))
				{
					((ISystemFixedUpdateEvents)system).FixedUpdate(components[system.Tag]);
				}
			}
		}

		internal static void FixedUpdateEnd()
		{
			var components = GetActiveComponents();
			foreach(var systemPair in _activeSystems)
			{
				var system = systemPair.Value;
				if (system is ISystemFixedUpdateEvents && components.ContainsKey(system.Tag))
				{
					((ISystemFixedUpdateEvents)system).FixedUpdateEnd(components[system.Tag]);
				}
			}
		}




		internal static void UpdateBegin()
		{
			var components = GetActiveComponents();
			foreach(var systemPair in _activeSystems)
			{
				var system = systemPair.Value;
				if (system is ISystemExtEvents &&  components.ContainsKey(system.Tag))
				{
					((ISystemExtEvents)system).UpdateBegin(components[system.Tag]);
				}
			}
		}

		internal static void Update()
		{
			var components = GetActiveComponents();
			foreach(var systemPair in _activeSystems)
			{
				var system = systemPair.Value;
				if (components.ContainsKey(system.Tag))
				{
					system.Update(components[system.Tag]);
				}
			}
		}

		internal static void UpdateEnd()
		{
			var components = GetActiveComponents();
			foreach(var systemPair in _activeSystems)
			{
				var system = systemPair.Value;
				if (system is ISystemExtEvents &&  components.ContainsKey(system.Tag))
				{
					((ISystemExtEvents)system).UpdateEnd(components[system.Tag]);
				}
			}
		}

		

		internal static void DrawBegin(Dictionary<string, List<Component>> components)
		{
			foreach(var systemPair in _activeSystems)
			{
				var system = systemPair.Value;
				if (system is ISystemExtEvents && components.ContainsKey(system.Tag))
				{
					((ISystemExtEvents)system).DrawBegin(FilterInactiveComponents(components[system.Tag]));
				}
			}
		}

		internal static void Draw(Dictionary<string, List<Component>> components)
		{
			foreach(var systemPair in _activeSystems)
			{
				var system = systemPair.Value;
				if (components.ContainsKey(system.Tag))
				{
					system.Draw(FilterInactiveComponents(components[system.Tag]));
				}
			}
		}

		internal static void DrawEnd(Dictionary<string, List<Component>> components)
		{
			foreach(var systemPair in _activeSystems)
			{
				var system = systemPair.Value;
				if (system is ISystemExtEvents && components.ContainsKey(system.Tag))
				{
					((ISystemExtEvents)system).DrawEnd(FilterInactiveComponents(components[system.Tag]));
				}
			}
		}

		internal static void DrawGUI(Dictionary<string, List<Component>> components)
		{
			foreach(var systemPair in _activeSystems)
			{
				var system = systemPair.Value;
				if (system is ISystemDrawGUIEvents && components.ContainsKey(system.Tag))
				{
					((ISystemDrawGUIEvents)system).DrawGUI(FilterInactiveComponents(components[system.Tag]));
				}
			}
		}


		#endregion Events.


		/// <summary>
		/// Creates an instance of each ISystem implementing class.
		/// </summary>
		internal static void InitSystemPool()
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


		/// <summary>
		/// Filters out inactive components.
		/// Component is inactive, if its owner is inactive.
		/// </summary>
		internal static List<Component> FilterInactiveComponents(List<Component> components)
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
		/// Puts system in active system list from system pool.
		/// </summary>
		public static void EnableSystem<T>() where T : ISystem
		{
			foreach(var systemPair in _systemPool)
			{
				if (systemPair.Value is T)
				{
					_activeSystems.Add(systemPair.Key, systemPair.Value);
					return;
				}
			}
		}


		/// <summary>
		/// Removes system from active system list and puts it back in pool.
		/// </summary>
		public static void DisableSystem<T>() where T : ISystem
		{
			foreach(var systemPair in _activeSystems.ToList()) // Quick way to clone list.
			{
				if (systemPair.Value is T)
				{
					_activeSystems.Remove(systemPair.Key);
					return;
				}
			}
		}


		/// <summary>
		/// Puts system in active system list from system pool.
		/// 
		/// NOTE: This method can activate more than one system.
		/// </summary>
		public static void EnableSystem(string tag)
		{
			foreach(var systemPair in _systemPool)
			{
				if (systemPair.Key == tag)
				{
					_activeSystems.Add(systemPair.Key, systemPair.Value);
				}
			}
		}


		/// <summary>
		/// Removes system from active system list and puts it back in pool.
		/// 
		/// NOTE: This method can deactivate more than one system.
		/// </summary>
		public static void DisableSystem(string tag)
		{
			foreach(var systemPair in _activeSystems.ToList()) // Quick way to clone list.
			{
				if (systemPair.Key == tag)
				{
					_activeSystems.Remove(systemPair.Key);
				}
			}
		}



		private static Dictionary<string, List<Component>> GetActiveComponents()
		{
			var list = new Dictionary<string, List<Component>>();
			foreach(var layer in Layer.Layers)
			{
				foreach(var componentsPair in layer._components)
				{
					if (list.ContainsKey(componentsPair.Key))
					{
						list[componentsPair.Key].AddRange(FilterInactiveComponents(componentsPair.Value));
					}
					else
					{
						list.Add(componentsPair.Key, FilterInactiveComponents(componentsPair.Value));
					}
				}
			}
			return list;
		}

		private static List<Component> GetNewComponents()
		{
			var list = new List<Component>();
			foreach(var layer in Layer.Layers)
			{	
				list.AddRange(FilterInactiveComponents(layer._newComponents));
			}
			return list;
		}


		/// <summary>
		/// Enables and disables systems depending on if there are any components for them.
		/// </summary>
		internal static void UpdateSystems()
		{
			foreach(var layer in Layer.Layers)
			{
				// Managing new components.
				if (layer._newComponents.Count > 0)
				{
					foreach(var component in layer._newComponents)
					{
						if (AutoSystemManagement && !_activeSystems.ContainsKey(component.Tag))
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

						if (layer._components.ContainsKey(component.Tag))
						{
							layer._components[component.Tag].Add(component);
						}
						else
						{
							var list = new List<Component>(new Component[] { component });
							layer._components.Add(component.Tag, list);
						}
					}
					layer._newComponents.Clear();
				}
				// Managing new components.


				// Disabling systems without components.
				if (layer._componentsWereRemoved)
				{
					foreach(var componentListPair in layer._components.ToList())
					{
						if (componentListPair.Value.Count == 0)
						{
							layer._components.Remove(componentListPair.Key);
							if (AutoSystemManagement)
							{
								_activeSystems.Remove(componentListPair.Key);
							}
						}
					}
					layer._componentsWereRemoved = false;
				}
				// Disabling systems without components.

			}
		}
		


	}
}
