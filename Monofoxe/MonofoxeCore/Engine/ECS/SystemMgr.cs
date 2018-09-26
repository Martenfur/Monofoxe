using System;
using System.Collections.Generic;
using System.Linq;
using Monofoxe.Engine.SceneSystem;

namespace Monofoxe.Engine.ECS
{
	public static class SystemMgr
	{
		/// <summary>
		/// List of currently active systems.
		/// </summary>
		internal static Dictionary<string, AbstractSystem> _activeSystems = new Dictionary<string, AbstractSystem>();
		// TODO: Add system priorities.

		/// <summary>
		/// Pool of all game systems.
		/// </summary>
		internal static Dictionary<string, AbstractSystem> _systemPool = new Dictionary<string, AbstractSystem>();

		public static int __dbgSysCount => _activeSystems.Count; // TODO: REMOVE
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
				foreach(var scene in SceneMgr.Scenes)
				{
					foreach(var layer in scene.Layers)
					{	
						layer._componentsWereRemoved = true;
					}
				}
			}
		}
		static bool _autoSystemManagement = true;


		#region Events.

		// TODO: Add a description, I dunno.
		/*
		 * For event explanation, see Entity. 
		 */
		
		internal static void FixedUpdate(Dictionary<string, List<Component>> components)
		{
			foreach(var systemPair in _activeSystems)
			{
				var system = systemPair.Value;
				if (components.ContainsKey(system.Tag))
				{
					system.FixedUpdate(components[system.Tag]);
				}
			}
		}


		internal static void Update(Dictionary<string, List<Component>> components)
		{
			foreach(var systemPair in _activeSystems)
			{
				var system = systemPair.Value;
				if (components.ContainsKey(system.Tag))
				{
					system.Update(components[system.Tag]);
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
					system.Draw(ComponentMgr.FilterInactiveComponents(components[system.Tag]));
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
				mytype => typeof(AbstractSystem).IsAssignableFrom(mytype) 
				&& mytype.GetInterfaces().Contains(typeof(AbstractSystem))
			);

			foreach(Type systemType in systemTypes)
			{
				var newSystem = (AbstractSystem)Activator.CreateInstance(systemType);
				_systemPool.Add(newSystem.Tag, newSystem);
			}

		}
		


		/// <summary>
		/// Puts system in active system list from system pool.
		/// </summary>
		public static void EnableSystem<T>() where T : AbstractSystem
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
		public static void DisableSystem<T>() where T : AbstractSystem
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



		/// <summary>
		/// Enables and disables systems depending on if there are any components for them.
		/// </summary>
		internal static void UpdateSystems()
		{
			foreach(var scene in SceneMgr.Scenes)
			{
				foreach(var layer in scene.Layers)
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
								var list = new List<Component>(new Component[] {component});
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
									// This won't work with multiple layers.
									// TODO: Need to figure out a way to quickly check if system has no components.
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
}
