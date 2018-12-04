using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Monofoxe.Engine.ECS;

namespace Monofoxe.Engine.SceneSystem
{
	/// <summary>
	/// A layer is a container for entities and components.
	/// </summary>
	public class Layer
	{
		/// <summary>
		/// Layer's parent scene.
		/// </summary>
		public readonly Scene Scene;

		/// <summary>
		/// Layer's name. Used for searching.
		/// NOTE: All layers should have unique names!
		/// </summary>
		public readonly string Name;

		/// <summary>
		/// If false, layer won't be rendered.
		/// </summary>
		public bool Visible = true;

		/// <summary>
		/// If true, layer won't be updated.
		/// </summary>
		public bool Enabled = true;


		internal bool _depthListOutdated = false;


		/// <summary>
		/// Priority of a layer. Layers with highest priority are processed first.
		/// </summary>
		public int Priority
		{
			get => _priority;

			set
			{
				_priority = value;
				Scene._layers.Remove(this); // Re-adding element to update its priority.
				Scene._layers.Add(this);
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
					_depthListOutdated = true;
				}
				else
				{
					// Linking "sorted" lists directly to primary lists.
					_depthSortedEntities = _entities;
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
		public IReadOnlyCollection<Entity> Entities => _entities;

		private List<Entity> _entities = new List<Entity>();
		internal List<Entity> _depthSortedEntities;

		private List<Entity> _newEntities = new List<Entity>();
		

		/// <summary>
		/// Component dictionary.
		/// </summary>
		internal Dictionary<Type, List<Component>> _components = new Dictionary<Type, List<Component>>();
		

		/// <summary>
		/// Newly created components. Used for Create event.
		/// </summary>
		internal List<Component> _newComponents = new List<Component>();



		/// <summary>
		/// Shaders applied to the layer.
		/// NOTE: You should enable postprocessing in camera.
		/// NOTE: Shaders won't be applied, if layer is GUI.
		/// </summary>
		public List<Effect> PostprocessorEffects {get; private set;} = new List<Effect>();


		internal Layer(string name, int priority, Scene scene)
		{
			Name = name;
			Scene = scene;
			Priority = priority; // Also adds layer to priority list.
			
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
					_depthListOutdated = false;
				}
			}
			else
			{
				_depthSortedEntities = _entities;
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
			var componentType = component.GetType();
			if (_components.ContainsKey(componentType))
			{
				var componentList = _components[componentType];
				if (componentList.Count == 1)
				{
					// Removing whole list, because it's empty.
					_components.Remove(componentType);
				}
				else
				{
					componentList.Remove(component);
				}
			}

			// Performing Destroy event.
			if (SystemMgr._activeSystems.ContainsKey(componentType))
			{
				SystemMgr._activeSystems[componentType].Destroy(component);
			}

			SystemMgr._componentsWereRemoved = true;
		}


		internal void UpdateEntityList()
		{
			// Clearing main list from destroyed objects.
			var updatedList = new List<Entity>();
			foreach(var entity in _entities)
			{
				if (!entity.Destroyed)
				{
					updatedList.Add(entity);
				}
			}
			_entities = updatedList;
			// Clearing main list from destroyed objects.


			// Adding new objects to the list.
			_entities.AddRange(_newEntities);		
			_newEntities.Clear();
			// Adding new objects to the list.
		}



		#region Entity methods.

		/// <summary>
		/// Returns list of entities of certain type.
		/// </summary>
		public List<T> GetList<T>() where T : Entity =>
			_entities.OfType<T>().ToList();
		
		/// <summary>
		/// Counts amount of objects of certain type.
		/// </summary>
		public int Count<T>() where T : Entity =>
			_entities.OfType<T>().Count();

		/// <summary>
		/// Checks if any instances of an entity exist.
		/// </summary>
		public bool EntityExists<T>() where T : Entity
		{
			foreach(var entity in _entities)
			{
				if (entity is T)
				{
					return true;
				}
			}			
			return false;
		}


		/// <summary>
		/// Finds first entity of given type.
		/// </summary>
		public T FindEntity<T>() where T : Entity
		{
			foreach(var entity in _entities)
			{
				if (entity is T)
				{
					return (T)entity;
				}
			}
			return null;
		}
		


		/// <summary>
		/// Returns list of entities with given tag.
		/// </summary>
		public List<Entity> GetList(string tag)
		{
			var list = new List<Entity>();
			
			foreach(var entity in _entities)
			{
				if (entity.Tag == tag)
				{
					list.Add(entity);
				}
			}
			return list;
		}
		

		/// <summary>
		/// Counts amount of entities with given tag.
		/// </summary>
		public int Count(string tag)
		{
			var counter = 0;

			foreach(var entity in _entities)
			{
				if (entity.Tag == tag)
				{
					counter += 1;
				}
			}
			
			return counter;
		}
		

		/// <summary>
		/// Checks if given instance exists.
		/// </summary>
		public bool EntityExists(string tag)
		{
			foreach(var entity in _entities)
			{
				if (entity.Tag == tag)
				{
					return true;
				}
			}
			return false;
		}
		

		/// <summary>
		/// Finds first entity with given tag.
		/// </summary>
		public Entity FindEntity(string tag)
		{
			foreach(var entity in _entities)
			{
				if (entity.Tag == tag)
				{
					return entity;
				}
			}
			
			return null;
		}

		#endregion Entity methods.
		


		/// <summary>
		/// Applies shaders to the camera surface.
		/// </summary>
		internal void ApplyPostprocessing()
		{
			var camera = DrawMgr.CurrentCamera;
			
			var sufraceChooser = false;
				
			for(var i = 0; i < PostprocessorEffects.Count - 1; i += 1)
			{
				DrawMgr.CurrentEffect = PostprocessorEffects[i];
				if (sufraceChooser)
				{
					DrawMgr.SetSurfaceTarget(camera._postprocessorLayerBuffer);
					DrawMgr.Device.Clear(Color.TransparentBlack);
					DrawMgr.DrawSurface(camera._postprocessorBuffer, Vector2.Zero, Vector2.One, 0, Color.White);
				}
				else
				{
					DrawMgr.SetSurfaceTarget(camera._postprocessorBuffer);
					DrawMgr.Device.Clear(Color.TransparentBlack);
					DrawMgr.DrawSurface(camera._postprocessorLayerBuffer, Vector2.Zero, Vector2.One, 0, Color.White);
				}
				
				DrawMgr.ResetSurfaceTarget();
				sufraceChooser = !sufraceChooser;
			}
			
			DrawMgr.CurrentEffect = PostprocessorEffects[PostprocessorEffects.Count - 1];
			if ((PostprocessorEffects.Count % 2) != 0)
			{
				DrawMgr.DrawSurface(camera._postprocessorLayerBuffer, Vector2.Zero, Vector2.One, 0, Color.White);
			}
			else
			{
				DrawMgr.DrawSurface(camera._postprocessorBuffer, Vector2.Zero, Vector2.One, 0, Color.White);
			}

			DrawMgr.CurrentEffect = null;
		}


	}
}
