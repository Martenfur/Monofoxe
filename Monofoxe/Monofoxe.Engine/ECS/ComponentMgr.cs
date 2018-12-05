using System.Collections.Generic;
using Monofoxe.Engine.SceneSystem;

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
		/// </summary>
		public static void InitComponent(Component component)
		{
			var layer = component.Owner.Layer;	
			var componentType = component.GetType();

			if (!SystemMgr._activeSystems.ContainsKey(componentType))
			{
				// Component may be first one created. In this case, system is disabled and need to be activated.
				// NOTE: Second ContainsKey check below is necessary, because system may not exist in pool at all. 
				SystemMgr.EnableSystem(componentType);
			}

			// If component is even there.
			if (layer._newComponents.Contains(component) && SystemMgr._activeSystems.ContainsKey(componentType))
			{
				SystemMgr._activeSystems[componentType].Create(component);
				layer._newComponents.Remove(component);
				return;
			}
			
		}

		

		/// <summary>
		/// Returns list of components of given type from entities owning provided components.
		/// TODO: This is too messy description. Redo it, of remove mathod completely.
		/// </summary>
		public static List<T> GetComponentList<T>(List<Component> components) where T : Component
		{
			var list = new List<T>();

			foreach(var component in components)
			{
				list.Add(component.Owner.GetComponent<T>());
			}
			return list;
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
