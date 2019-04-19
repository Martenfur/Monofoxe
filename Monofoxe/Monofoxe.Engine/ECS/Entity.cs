using System;
using System.Collections.Generic;
using Monofoxe.Engine.SceneSystem;

namespace Monofoxe.Engine.ECS
{
	/// <summary>
	/// Parent class of every in-game object.
	/// Can hold components, or implement its own logic.
	/// </summary>
	public class Entity
	{
		/// <summary>
		/// Unique tag for identifying entity.
		/// NOTE: Entity tags should be unique!
		/// </summary>
		public readonly string Tag;
		
		/// <summary>
		/// Depth of Draw event. Objects with the lowest depth draw the last.
		/// </summary>
		public int Depth
		{
			get => _depth;
			set
			{
				if (value != _depth)
				{
					_depth = value;
					Layer._depthListOutdated = true;
				}
			}
		}
		private int _depth;


		/// <summary>
		/// Tells f object was destroyed.
		/// </summary>
		public bool Destroyed {get; private set;} = false;

		/// <summary>
		/// If false, Update and Destroy events won't be executed.
		/// NOTE: This also applies to entity's components.
		/// </summary>
		public bool Enabled
		{
			get => _enabled;
			set
			{
				// When entity is being enabled, it checks if there are systems for its components.
				// This is needed for cases when this is the only active entity with component of this type.
				if (value && !_enabled)
				{
					foreach(var componentPair in _components)
					{
						if (componentPair.Value.System != null && !componentPair.Value.System.Enabled)
						{
							SystemMgr.EnableSystem(componentPair.Value.System.GetType());
						}
					}
				}
				_enabled = value;
			}
		}

		private bool _enabled = true;
		
		/// <summary>
		/// If false, Draw events won't be executed.
		/// NOTE: This also applies to entity's components.
		/// </summary>
		public bool Visible = true;
		

		/// <summary>
		/// Layer that entity is currently on.
		/// </summary>
		public Layer Layer
		{
			get => _layer;
			set
			{
				if (_layer != null)
				{
					foreach(var componentPair in _components)
					{
						_layer.RemoveComponent(componentPair.Value);
					}
					_layer.RemoveEntity(this);
				}
				_layer = value;
				foreach(var componentPair in _components)
				{
					_layer.AddComponent(componentPair.Value);
				}
				_layer.AddEntity(this);
			}
		}
		private Layer _layer;

		/// <summary>
		/// Scene that entity is currently on.
		/// </summary>
		public Scene Scene => _layer.Scene;


		/// <summary>
		/// Component dictionary.
		/// </summary>
		internal Dictionary<Type, Component> _components;

		public Entity(Layer layer, string tag = "entity")
		{
			_components = new Dictionary<Type, Component>();
			Tag = tag;
			Layer = layer;
		}


		
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
		/// Updates at a fixed rate, if entity is enabled.
		/// </summary>
		public virtual void FixedUpdate() {}
		
		
		
		/// <summary>
		/// Updates every frame, if entity is enabled.
		/// </summary>
		public virtual void Update() {}
		
		

		/// <summary>
		/// Draw updates. Triggers only if entity is visible.
		/// 
		/// NOTE: DO NOT put any significant logic into Draw.
		/// It may skip frames.
		/// </summary>
		public virtual void Draw() {}
		


		/// <summary>
		///	Triggers right before destruction, if entity is enabled. 
		/// </summary>
		public virtual void Destroy() {}

		#endregion Events.



		#region Components.

		/// <summary>
		/// Adds component to the entity.
		/// </summary>
		public void AddComponent(Component component)
		{
			if (component.Owner != null)
			{
				// If component is assigned to other entity - take it away.
				component.Owner.RemoveComponent(component.GetType());
			}
			_components.Add(component.GetType(), component);
			component.Owner = this;
			Layer.AddComponent(component);
		}
		
		

		/// <summary>
		/// Returns component of given class.
		/// </summary>
		public T GetComponent<T>() where T : Component =>
			(T)_components[typeof(T)];
		
		/// <summary>
		/// Returns component of given class.
		/// </summary>
		public Component GetComponent(Type type) =>
			_components[type];
		

		/// <summary>
		/// Retrieves component of given class, if it exists, and returns true. If it doesn't, returns false.
		/// </summary>
		public bool TryGetComponent<T>(out T component) where T : Component
		{
			var result = _components.TryGetValue(typeof(T), out Component c);
			component = (T)c; // Needs a manual cast.
			return result;
		}
		
		/// <summary>
		/// Retrieves component of given class, if it exists, and returns true. If it doesn't, returns false.
		/// </summary>
		public bool TryGetComponent(out Component component, Type type) =>
			_components.TryGetValue(type, out component);
		

		/// <summary>
		/// Returns all the components. All of them.
		/// </summary>
		public Component[] GetAllComponents()
		{
			var array = new Component[_components.Count];
			var id = 0;

			foreach(var componentPair in _components)
			{
				array[id] = componentPair.Value;
				id += 1;
			}

			return array;
		}


		/// <summary>
		/// Checks if an entity has the component of given type.
		/// </summary>
		public bool HasComponent<T>() where T : Component =>
			_components.ContainsKey(typeof(T));
		
		/// <summary>
		/// Checks if an entity has the component of given type.
		/// </summary>
		public bool HasComponent(Type type) =>
			_components.ContainsKey(type);
		

		
		/// <summary>
		/// Removes component from an entity and returns it.
		/// </summary>
		public Component RemoveComponent<T>() where T : Component =>
			RemoveComponent(typeof(T));
		
		/// <summary>
		/// Removes component from an entity and returns it.
		/// </summary>
		public Component RemoveComponent(Type type)
		{
			if (_components.TryGetValue(type, out Component component))
			{
				_components.Remove(type);
				Layer.RemoveComponent(component);
				component.Owner = null;
				return component;
			}
			return null;
		}
		
		#endregion Components.



		/// <summary>
		/// Creates new entity from existing template.
		/// </summary>
		public static Entity CreateFromTemplate(Layer layer, string tag)
		{
			// TODO: Maybe make better tags.
			if (EntityFactoryPool.FactoryPool.TryGetValue(tag.ToLower(), out IEntityFactory factory))
			{
				return factory.Make(layer);
			}
			return null;
		}


		/// <summary>
		/// Destroys entity and all of its components.
		/// </summary>
		public void DestroyEntity()
		{
			if (!Destroyed)
			{
				Destroyed = true;
				if (Enabled)
				{
					// Performs Destroy event only if entity is enabled.
					Destroy();
				}
				foreach(var componentPair in _components)
				{
					Layer.RemoveComponent(componentPair.Value);
				}
			}
		}

	}
}
