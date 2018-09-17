using System;
using System.Collections.Generic;
using Monofoxe.Engine.ECS;
using System.Linq;


namespace Monofoxe.Engine.Drawing
{
	public class Layer
	{
		//TODO: Add new entity management.	

		/// <summary>
		/// List of all existing layers.
		/// </summary>
		public static IReadOnlyCollection<Layer> Layers => _layers;

		/// <summary>
		/// List of all existing layers.
		/// </summary>
		private static List<Layer> _layers = new List<Layer>();

		/// <summary>
		/// Layer's name. Used for searching.
		/// NOTE: All layers should have unique names!
		/// </summary>
		public readonly string Name;

		/// <summary>
		/// Priority of a layer.
		/// </summary>
		public int Priority
		{
			get => _priority;

			set
			{
				_priority = value;
				_layers.Remove(this);
				AddLayerToList(this);
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
		private List<Entity> _depthSortedEntities;

		internal List<Entity> _newEntities = new List<Entity>();
		

		/// <summary>
		/// Component dictionary.
		/// </summary>
		internal Dictionary<string, List<Component>> _components = new Dictionary<string, List<Component>>();
		private Dictionary<string, List<Component>> _depthSortedComponents;


		/// <summary>
		/// Newly created components. Used for Create event.
		/// </summary>
		internal List<Component> _newComponents = new List<Component>();
		

		/// <summary>
		/// Tells if any components were removed in the current step.
		/// </summary>
		internal bool _componentsWereRemoved = false;



		private Layer(string name, int depth)
		{
			Name = name;
			Priority = depth;

			DepthSorting = false;
		}
		

		
		/// <summary>
		/// Creates new layer with given name.
		/// </summary>
		public static Layer Create(string name, int depth = 0)
		{
			if (Exists(name))
			{
				throw(new Exception("Layer with such name already exists!"));
			}
			
			return new Layer(name, depth);
		}

		
		/// <summary>
		/// Destroys given layer.
		/// </summary>
		public static void Destroy(Layer layer)
		{
			if (_layers.Remove(layer))
			{
				foreach(var entity in layer._entities)
				{
					EntityMgr.DestroyEntity(entity);
				}
			}
		}

		/// <summary>
		/// Destroys layer with given name.
		/// </summary>
		public static void Destroy(string name)
		{
			foreach(var layer in _layers)
			{
				if (layer.Name == name)
				{
					 _layers.Remove(layer);
					foreach(var entity in layer._entities)
					{
						EntityMgr.DestroyEntity(entity);
					}
				}
			}
		}


		/// <summary>
		/// Returns layer with given name.
		/// </summary>
		public static Layer Get(string name)
		{
			foreach(var layer in _layers)
			{
				if (layer.Name == name)
				{
					return layer;
				}
			}
			return null;
		}


		/// <summary>
		/// Returns true, if there is a layer with given name. 
		/// </summary>
		public static bool Exists(string name)
		{
			foreach(var layer in _layers)
			{
				if (layer.Name == name)
				{
					return true;
				}
			}
			return false;
		}


		/// <summary>
		/// Sorts entites and components by depth, if depth sorting is enabled.
		/// </summary>
		internal void SortByDepth()
		{
			if (DepthSorting)
			{
				_depthSortedEntities = _entities.OrderByDescending(o => o.Depth).ToList();

				_depthSortedComponents.Clear();
				foreach(KeyValuePair<string, List<Component>> list in _components)
				{
					_depthSortedComponents.Add(list.Key, list.Value.OrderByDescending(o => o.Owner.Depth).ToList());
				}
			}
			else
			{
				_depthSortedEntities = _entities;
				_depthSortedComponents = _components;
			}
		}


		internal void AddEntity(Entity entity) =>
			_newEntities.Add(entity);
		
		internal void RemoveEntity(Entity entity) =>
			_entities.Remove(entity);
		

		internal void AddComponent(Component component) =>
			_newComponents.Add(component);
		
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


		/// <summary>
		/// Executes Draw, DrawBegin and DrawEnd events.
		/// </summary>
		internal static void CallDrawEvents()
		{
			foreach(var layer in _layers)
			{
				if (!layer.IsGUI)
				{
					SystemMgr.Draw(layer._depthSortedComponents);
					foreach(var entity in layer._depthSortedEntities)
					{
						if (entity.Active && !entity.Destroyed)
						{
							entity.Draw();
						}
					}
				}
			}
		}
		
		/// <summary>
		/// Executes Draw GUI events.
		/// </summary>
		internal static void CallDrawGUIEvents()
		{
			foreach(var layer in _layers)
			{
				if (layer.IsGUI)
				{
					SystemMgr.Draw(layer._depthSortedComponents);
					foreach(var entity in layer._depthSortedEntities)
					{
						if (entity.Active && !entity.Destroyed)
						{
							entity.Draw();
						}
					}
				}
			}
		}



		/// <summary>
		/// Adds new layer to main layer list, taking in account its proirity.
		/// </summary>
		private static void AddLayerToList(Layer layer)
		{
			for(var i = 0; i < _layers.Count; i += 1)
			{
				if (layer.Priority > _layers[i].Priority)
				{
					_layers.Insert(i, layer);
					return;
				}
			}
			_layers.Add(layer);
		}

	}
}
