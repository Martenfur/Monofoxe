using System;
using System.Collections.Generic;
using System.Linq;
using Monofoxe.Engine.SceneSystem;
using Monofoxe.Engine.CustomCollections;

namespace Monofoxe.Engine.ECS
{
	/// <summary>
	/// Manages ECS systems.
	/// </summary>
	public static class SystemMgr
	{
		/// <summary>
		/// List of currently active systems.
		/// </summary>
		internal static SafeSortedDictionary<string, BaseSystem> _activeSystems 
			= new SafeSortedDictionary<string, BaseSystem>(x => x.Priority);
		
		/// <summary>
		/// Pool of all game systems.
		/// </summary>
		internal static Dictionary<string, BaseSystem> _systemPool = new Dictionary<string, BaseSystem>();

		/// <summary>
		/// Tells if any components were removed in the current step.
		/// </summary>
		internal static bool _componentsWereRemoved = false;


		/// <summary>
		/// If true, systems will be enabled and disabled automatically.
		/// </summary>
		public static bool AutoSystemManagement 
		{
			get => _autoSystemManagement;
			set
			{
				_autoSystemManagement = value;
				_componentsWereRemoved = true;
			}
		}
		static bool _autoSystemManagement = true;


		#region Events.
		/*
		 * Event order:
		 * - FixedUpdate
		 * - Update
		 * - Draw
		 * 
		 * NOTE: Component events are executed before entity events.
		 */

		/// <summary>
		/// Updates at a fixed rate.
		/// </summary>
		internal static void FixedUpdate(Dictionary<string, List<Component>> components)
		{
			foreach(var system in _activeSystems)
			{
				if (components.ContainsKey(system.Tag))
				{
					system.FixedUpdate(components[system.Tag]);
				}
			}
		}


		/// <summary>
		/// Updates every frame.
		/// </summary>
		internal static void Update(Dictionary<string, List<Component>> components)
		{
			foreach(var system in _activeSystems)
			{
				if (components.ContainsKey(system.Tag))
				{
					var componentList = components[system.Tag];
					if (componentList.Count > 0)
					{
						system._usedLayersCount += 1; // Telling that a layer is using this system.
						system.Update(components[system.Tag]);
					}
				}
			}
		}
		

		/// <summary>
		/// Draw updates.
		/// 
		/// NOTE: DO NOT put any significant logic into Draw.
		/// It may skip frames.
		/// </summary>
		internal static void Draw(Component component)//Dictionary<string, List<Component>> components)
		{
			if (_activeSystems.ContainsKey(component.Tag))
			{
				_activeSystems[component.Tag].Draw(component);
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
			 * BaseSystem.
			 */

			// Extracting all types from assemblies.
			var systemTypes = new List<Type>();

			foreach(var typePair in GameMgr.Types)
			{
				if (typeof(BaseSystem).IsAssignableFrom(typePair.Value))
				{
					systemTypes.Add(typePair.Value);
				}
			}
			// Extracting all types from assemblies.
			
			
			// Creating an instance of each system.
			foreach(var systemType in systemTypes)
			{
				if (systemType != typeof(BaseSystem))
				{
					var newSystem = (BaseSystem)Activator.CreateInstance(systemType);
					Console.WriteLine("System:" + newSystem.Tag);
					_systemPool.Add(newSystem.Tag, newSystem);
				}
			}
			// Creating an instance of each system.
		}



		/// <summary>
		/// Puts system in active system list from system pool.
		/// </summary>
		public static void EnableSystem<T>() where T : BaseSystem
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
		public static void DisableSystem<T>() where T : BaseSystem
		{
			foreach(var system in _activeSystems)
			{
				if (system is T)
				{
					_activeSystems.Remove(system.Tag);
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
		public static void DisableSystem(string tag) =>
			_activeSystems.Remove(tag);
		


		/// <summary>
		/// Enables and disables systems depending on if there are any components for them.
		/// </summary>
		internal static void UpdateSystems()
		{
			// Disabling systems without components.
			if (AutoSystemManagement && _componentsWereRemoved)
			{
				var unusedSystems = new List<string>();
				foreach(var system in _activeSystems)
				{
					if (system._usedLayersCount == 0)
					{
						unusedSystems.Add(system.Tag);	
						_componentsWereRemoved = false;
					}
				}
				foreach(var systemName in unusedSystems)
				{
					_activeSystems.Remove(systemName);
				}
			}
			// Disabling systems without components.
			
			// Resetting system counters.
			foreach(var system in _activeSystems)
			{
				system._usedLayersCount = 0;
			}
			// Resetting system counters.

			// Managing new components.
			foreach(var scene in SceneMgr.Scenes)
			{
				foreach(var layer in scene.Layers)
				{
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
				}
			}	
			// Managing new components.
		}
		


	}
}
