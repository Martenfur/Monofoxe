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
		internal static SafeSortedDictionary<Type, BaseSystem> _systemPool 
			= new SafeSortedDictionary<Type, BaseSystem>(x => x.Priority);
		

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
			foreach(var system in _systemPool)
			{
				if (!system.Enabled)
				{
					continue;
				}
				
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
					system.FixedUpdate(sceneComponents);
				}
			}
		}


		/// <summary>
		/// Updates every frame.
		/// </summary>
		internal static void Update()
		{
			foreach(var system in _systemPool)
			{
				if (!system.Enabled)
				{
					continue;
				}
				
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
					system.Update(sceneComponents);
				}
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
			if (component.System != null && component.System.Enabled)
			{
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
		
	}
}
