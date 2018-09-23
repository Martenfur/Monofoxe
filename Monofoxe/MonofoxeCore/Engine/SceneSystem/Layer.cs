using System.Collections.Generic;
using Monofoxe.Engine.ECS;
using System.Linq;

namespace Monofoxe.Engine.SceneSystem
{
	/// <summary>
	/// A layer is a container for entities and components.
	/// </summary>
	public class Layer
	{
		
		/// <summary>
		/// Layer's name. Used for searching.
		/// NOTE: All layers should have unique names!
		/// </summary>
		public readonly string Name;

		internal bool _depthListOutdated = false;

		/// <summary>
		/// Priority of a layer. 
		/// </summary>
		public int Priority
		{
			get => _priority;

			set
			{
				_priority = value;
				LayerMgr.UpdateLayerPriority(this);
			}
		}
		private int _priority;

		/// <summary>
		/// If true, entities and components will be sorted by their depth.
		/// </summary>
		public bool DepthSorting 
		{
			get => _depthSorting;
			set
			{
				_depthSorting = value;
				if (value)
				{
					_depthSortedEntities = new List<Entity>();
					_depthSortedComponents = new Dictionary<string, List<Component>>();
				}
				else
				{
					// Linking "sorted" lists directly to primary lists.
					_depthSortedEntities = _entities;
					_depthSortedComponents = _components;
				}
			}
		}
		private bool _depthSorting;


		/// <summary>
		/// If true, draws everything directly to the backbuffer instead of cameras.
		/// </summary>
		public bool IsGUI = false;


		/// <summary>
		/// List of all layer's entities.
		/// </summary>
		internal List<Entity> _entities = new List<Entity>();
		internal List<Entity> _depthSortedEntities;

		internal List<Entity> _newEntities = new List<Entity>();
		

		/// <summary>
		/// Component dictionary.
		/// </summary>
		internal Dictionary<string, List<Component>> _components = new Dictionary<string, List<Component>>();
		internal Dictionary<string, List<Component>> _depthSortedComponents;


		/// <summary>
		/// Newly created components. Used for Create event.
		/// </summary>
		internal List<Component> _newComponents = new List<Component>();
		

		/// <summary>
		/// Tells if any components were removed in the current step.
		/// </summary>
		internal bool _componentsWereRemoved = false;



		internal Layer(string name, int depth)
		{
			Name = name;
			Priority = depth; // Also adds layer to priority list.

			DepthSorting = false;
		}
		
		

		/// <summary>
		/// Sorts entites and components by depth, if depth sorting is enabled.
		/// </summary>
		internal void SortByDepth()
		{
			if (DepthSorting)
			{
				if (_depthListOutdated)
				{
					_depthSortedEntities = _entities.OrderByDescending(o => o.Depth).ToList();

					_depthSortedComponents.Clear();
					foreach(KeyValuePair<string, List<Component>> list in _components)
					{
						_depthSortedComponents.Add(list.Key, list.Value.OrderByDescending(o => o.Owner.Depth).ToList());
					}

					_depthListOutdated = false;
				}
			}
			else
			{
				_depthSortedEntities = _entities;
				_depthSortedComponents = _components;
			}
		}
		

		internal void AddEntity(Entity entity)
		{
			_newEntities.Add(entity);
			_depthListOutdated = true;
		}

		internal void RemoveEntity(Entity entity) =>
			_entities.Remove(entity);
		

		internal void AddComponent(Component component)
		{
			_newComponents.Add(component);
			_depthListOutdated = true;
		}


		internal void RemoveComponent(Component component)
		{
			// Removing from lists.
			_newComponents.Remove(component);
			if (_components.ContainsKey(component.Tag))
			{
				_components[component.Tag].Remove(component);
			}

			// Performing Destroy event.
			if (SystemMgr._activeSystems.ContainsKey(component.Tag))
			{
				SystemMgr._activeSystems[component.Tag].Destroy(component);
			}

			_componentsWereRemoved = true;
		}


		
	}
}
