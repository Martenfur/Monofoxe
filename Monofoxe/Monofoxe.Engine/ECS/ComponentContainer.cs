using System;
using System.Collections.Generic;

namespace Monofoxe.Engine.ECS
{
	/// <summary>
	/// Custom container for entity components.
	/// Basically just a dictionary of lists of components.
	/// </summary>
	class ComponentContainer
	{
		private Dictionary<Type, List<Component>> _container;

		public ComponentContainer()
		{
			_container = new Dictionary<Type, List<Component>>(); 
		}


		/// <summary>
		/// Adds component to the container.
		/// </summary>
		public void Add(Component component)
		{
			var componentType = component.GetType();
			if (_container.TryGetValue(componentType, out List<Component> componentList))
			{
				componentList.Add(component);
			}
			else
			{
				// There may be no list at all, so we need to create one.
				var newComponentsList = new List<Component>(new Component[]{component});
				_container.Add(componentType, newComponentsList);
			}
		}

		/// <summary>
		/// Removes component from the container.
		/// </summary>
		public void Remove(Component component)
		{
			var componentType = component.GetType();
			
			if (_container.TryGetValue(componentType, out List<Component> componentList))
			{
				componentList.Remove(component);
				if (componentList.Count == 0)
				{
					// Removing whole list, because it's empty.
					_container.Remove(componentType);
				}
			}
		}


		/// <summary>
		/// Gets the component list associated with the specified key.
		/// </summary>
		public bool TryGetList(Type type, out List<Component> componentList) =>
			_container.TryGetValue(type, out componentList);

	}
}
