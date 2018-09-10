using System;
using System.Collections.Generic;
using Monofoxe.Engine.ECS;
using System.Linq;


namespace Monofoxe.Engine.Drawing
{
	public class Layer
	{

		public static IReadOnlyCollection<Layer> Layers => _layers;

		private static List<Layer> _layers = new List<Layer>();


		public readonly string Name;

		public int Depth 
		{
			get => _depth;

			set
			{
				_depth = value;
				_layers.Remove(this);
				AddLayerToList(this);
			}
		}
		private int _depth;

		public bool DepthSorting 
		{
			get => _depthSorting;
			set
			{
				_depthSorting = value;
				if (value)
				{
					_depthSortedComponents = new Dictionary<string, List<Component>>();
				}
				else
				{
					_depthSortedComponents = _components;
				}
			}
		}
		private bool _depthSorting = false;

		private List<Entity> _entities = new List<Entity>();
		private List<Entity> _depthSortedEntities;


		/// <summary>
		/// Component dictionary.
		/// </summary>
		internal Dictionary<string, List<Component>> _components = new Dictionary<string, List<Component>>();
		
		/// <summary>
		/// Newly created components. Used for Create event.
		/// </summary>
		internal List<Component> _newComponents = new List<Component>();
		Dictionary<string, List<Component>> _depthSortedComponents;


		/// <summary>
		/// Tells if any components were removed in the current step.
		/// </summary>
		internal bool _componentsWereRemoved = false;



		private Layer(string name, int depth)
		{
			Name = name;
			Depth = depth;

			_depthSortedEntities = _entities;
			_depthSortedComponents = _components;
		}
		
		
		// TODO: Add depth sorting mode.

		
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
		}


		internal void AddEntity(Entity entity) =>
			_entities.Add(entity);
		
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



		


		internal static void CallDrawEvents()
		{
			foreach(var layer in _layers)
			{
				SystemMgr.DrawBegin(layer._depthSortedComponents);
				foreach(Entity obj in layer._depthSortedEntities)
				{
					if (obj.Active && !obj.Destroyed)
					{
						obj.DrawBegin();
					}
				}

				SystemMgr.Draw(layer._depthSortedComponents);
				foreach(Entity obj in layer._depthSortedEntities)
				{
					if (obj.Active && !obj.Destroyed)
					{
						obj.Draw();
					}
				}
				
				SystemMgr.DrawEnd(layer._depthSortedComponents);
				foreach(Entity obj in layer._depthSortedEntities)
				{
					if (obj.Active && !obj.Destroyed)
					{
						obj.DrawEnd();
					}
				}
			}
		}



		internal static void CallDrawGUIEvents()
		{
			foreach(var layer in _layers)
			{
				SystemMgr.DrawGUI(layer._depthSortedComponents);
				foreach(Entity obj in layer._depthSortedEntities)
				{
					if (obj.Active && !obj.Destroyed)
					{
						obj.DrawGUI();
					}
				}
			}
		}



		private static void AddLayerToList(Layer layer)
		{
			for(var i = 0; i < _layers.Count; i += 1)
			{
				if (layer.Depth > _layers[i].Depth)
				{
					_layers.Insert(i, layer);
					return;
				}
			}
			_layers.Add(layer);
		}

	}
}
