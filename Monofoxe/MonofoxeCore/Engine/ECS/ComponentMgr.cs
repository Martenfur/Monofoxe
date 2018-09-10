using System.Collections.Generic;
using Monofoxe.Engine.Drawing;

namespace Monofoxe.Engine.ECS
{
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
			foreach(var layer in Layer.Layers)
			{
				// If component is even there.
				if (layer._newComponents.Contains(component) && SystemMgr._activeSystems.ContainsKey(component.Tag))
				{
					SystemMgr._activeSystems[component.Tag].Create(component);
					layer._newComponents.Remove(component);
					return;
				}
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
