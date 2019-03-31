using System.Collections.Generic;

namespace Monofoxe.Engine.ECS
{
	/// <summary>
	/// Set of handy methods for components.
	/// </summary>
	public static class ComponentMgr
	{
		/// <summary>
		/// Invokes Create event for corresponding system.
		/// Create events are usually executed at the beginning of each step,
		/// so components stay uninitialized for a bit right after creation.
		/// In most cases it's fine, but you may need to init your component
		/// right here and right now.
		/// 
		/// NOTE: If component's Initialized property is true, this method will do nothing!
		/// </summary>
		internal static void InitComponent(Component component)
		{
			if (component.Initialized)
			{
				return;
			}
			
			var layer = component.Owner.Layer;	
			var componentType = component.GetType();
			
			if (!SystemMgr._activeSystems.ContainsKey(componentType))
			{
				// Component may be first one created. In this case, system is disabled and need to be activated.
				// NOTE: TryGetValue below is necessary, because system may not exist in pool at all. 
				SystemMgr.EnableSystemByComponentType(componentType);
			}

			// If component is even there.
			if (SystemMgr._activeSystems.TryGetValue(componentType, out BaseSystem system))
			{
				system.Create(component);
				system._usedByLayers = true;
				component.Initialized = true;
				return;
			}
		}
		
		
		/// <summary>
		/// Filters out inactive components.
		/// Component is inactive, if its owner is inactive.
		/// </summary>
		internal static List<Component> FilterInactiveComponents(List<Component> components)
		{
			var activeComponents = new List<Component>();
			
			foreach(var component in components)
			{
				if (component.Owner.Enabled && !component.Owner.Destroyed)
				{
					activeComponents.Add(component);
				}
			}
			return activeComponents;	
		}
		
	}
}
