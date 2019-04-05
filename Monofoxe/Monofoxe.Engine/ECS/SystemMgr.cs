using System;
using System.Collections.Generic;
using Monofoxe.Engine.Utils.CustomCollections;
using Monofoxe.Engine.SceneSystem;

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
		internal static SafeSortedDictionary<Type, BaseSystem> _activeSystems 
			= new SafeSortedDictionary<Type, BaseSystem>(x => x.Priority);
		
		/// <summary>
		/// Pool of all game systems.
		/// </summary>
		internal static Dictionary<Type, BaseSystem> _systemPool = new Dictionary<Type, BaseSystem>();

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
		internal static void FixedUpdate()
		{
			foreach(var system in _activeSystems)
			{
				var sceneComponents = new List<Component>();
				foreach(var layer in SceneMgr.CurrentScene.Layers)
				{
					if (layer.Enabled && layer._components.TryGetList(system.ComponentType, out List<Component> layerComponents))
					{
						// Disabled components are not in this list, so here we only need to check
						// if entity itself was disabled.
						sceneComponents.AddRange(layerComponents.FindAll(x => x.Owner.Enabled));
					}
				}
			
				if (sceneComponents.Count > 0)
				{
					system._usedByLayers = true;
					system.FixedUpdate(sceneComponents);
				}
			}
		}


		/// <summary>
		/// Updates every frame.
		/// </summary>
		internal static void Update()
		{
			foreach(var system in _activeSystems)
			{
				var sceneComponents = new List<Component>();
				foreach(var layer in SceneMgr.CurrentScene.Layers)
				{
					if (layer.Enabled && layer._components.TryGetList(system.ComponentType, out List<Component> layerComponents))
					{
						// Disabled components are not in this list, so here we only need to check
						// if entity itself was disabled.
						sceneComponents.AddRange(layerComponents.FindAll(x => x.Owner.Enabled));
					}
				}
			
				if (sceneComponents.Count > 0)
				{
					system._usedByLayers = true;
				}
				system.Update(sceneComponents);
			}
		}
		

		/// <summary>
		/// Draw updates.
		/// 
		/// NOTE: DO NOT put any significant logic into Draw.
		/// It may skip frames.
		/// </summary>
		internal static void Draw(Component component)
		{
			if (component.System != null)
			{
				component.System._usedByLayers = true;
				component.System.Draw(component);
			}
		}

		#endregion Events.


		/// <summary>
		/// Creates an instance of each ISystem implementing class.
		/// </summary>
		internal static void InitSystemPool()
		{

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
					_systemPool.Add(newSystem.ComponentType, newSystem);
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
					systemPair.Value.Enabled = true;
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
					_activeSystems.Remove(system.ComponentType);
					system.Enabled = false;
					return;
				}
			}
		}


		/// <summary>
		/// Puts system in active system list from system pool.
		/// </summary>
		public static void EnableSystemByComponentType<T>() where T : Component
		{
			foreach(var systemPair in _systemPool)
			{
				if (systemPair.Value.ComponentType == typeof(T))
				{
					_activeSystems.Add(systemPair.Key, systemPair.Value);
					systemPair.Value.Enabled = true;
					return;
				}
			}
		}


		/// <summary>
		/// Removes system from active system list and puts it back in pool.
		/// </summary>
		public static void DisableSystemByComponentType<T>() where T : Component
		{
			foreach(var system in _activeSystems)
			{
				if (system.ComponentType == typeof(T))
				{
					_activeSystems.Remove(system.ComponentType);
					system.Enabled = false;
					return;
				}
			}
		}
		

		/// <summary>
		/// Puts system in active system list from system pool.
		/// </summary>
		public static void EnableSystem(Type systemType)
		{
			foreach(var systemPair in _systemPool)
			{	
				// Systems are sorted by component type, so we cannot use systemPair.Key.
				if (systemPair.Value.GetType() == systemType) 
				{
					_activeSystems.Add(systemPair.Key, systemPair.Value);
					systemPair.Value.Enabled = true;
					return;
				}
			}
		}

		/// <summary>
		/// Puts system in active system list from system pool.
		/// </summary>
		public static void EnableSystemByComponentType(Type componentType)
		{
			foreach(var systemPair in _systemPool)
			{	
				// Systems are sorted by component type, so we cannot use systemPair.Key.
				if (systemPair.Value.ComponentType == componentType) 
				{
					_activeSystems.Add(systemPair.Key, systemPair.Value);
					systemPair.Value.Enabled = true;
					return;
				}
			}
		}


		/// <summary>
		/// Removes system from active system list and puts it back in pool.
		/// </summary>
		public static void DisableSystem(Type systemType)
		{
			foreach(var system in _activeSystems)
			{
				if (system.GetType() == systemType) 
				{
					_activeSystems.Remove(system.ComponentType);
					system.Enabled = false;
					return;
				}
			}
		}

		/// <summary>
		/// Removes system from active system list and puts it back in pool.
		/// </summary>
		public static void DisableSystemByComponentType(Type componentType)
		{
			foreach(var system in _activeSystems)
			{
				if (system.ComponentType == componentType) 
				{
					_activeSystems.Remove(system.ComponentType);
					system.Enabled = false;
					return;
				}
			}
		}



		/// <summary>
		/// Disables systms if no components are using them.
		/// </summary>
		internal static void DisableInactiveSystems()
		{
			// Disabling systems without components.
			if (AutoSystemManagement && _componentsWereRemoved)
			{
				var unusedSystems = new List<Type>();
				foreach(var system in _activeSystems)
				{
					if (!system._usedByLayers)
					{
						unusedSystems.Add(system.ComponentType);	
						system.Enabled = false;
						_componentsWereRemoved = false;
					}
				}
				foreach(var systemComponentType in unusedSystems)
				{
					_activeSystems.Remove(systemComponentType);
				}
			}
			// Disabling systems without components.
			
			// Resetting usage flags.
			foreach(var system in _activeSystems)
			{
				system._usedByLayers = false;
			}
			// Resetting usage flags.
		}
		


	}
}
