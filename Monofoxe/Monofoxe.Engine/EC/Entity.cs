using System;
using System.Collections.Generic;
using Monofoxe.Engine.SceneSystem;
using Monofoxe.Engine.Utils.CustomCollections;

namespace Monofoxe.Engine.EC
{
	/// <summary>
	/// Parent class of every in-game object.
	/// Can hold components, or implement its own logic.
	/// </summary>
	public class Entity
	{
		
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
		public bool Enabled = true;
		
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
					_layer.RemoveEntity(this);
				}
				_layer = value;
				_layer.AddEntity(this);
			}
		}
		private Layer _layer;

		/// <summary>
		/// Scene that entity is currently on.
		/// </summary>
		public Scene Scene => _layer.Scene;

		// We need two collections to eliminate collection modification 
		// during foreach while keeping GetComponent faste.
		private Dictionary<Type, Component> _componentDictionary;
		private SafeList<Component> _componentList;


		public Entity(Layer layer)
		{
			_componentDictionary = new Dictionary<Type, Component>();
			_componentList = new SafeList<Component>();
			Layer = layer;
		}
		

		#region Events.

		/*
		 * Event order:
		 * - FixedUpdate
		 * - Update
		 * - Draw
		 */


		/// <summary>
		/// Updates at a fixed rate, if entity is enabled.
		/// </summary>
		public virtual void FixedUpdate() 
		{
			foreach(var component in _componentList)
			{
				if (component.Enabled)
				{
					component.FixedUpdate();
				}
			}
		}
		
		
		
		/// <summary>
		/// Updates every frame, if entity is enabled.
		/// </summary>
		public virtual void Update() 
		{
			foreach (var component in _componentList)
			{
				if (component.Enabled)
				{
					component.Update();
				}
			}
		}
		
		

		/// <summary>
		/// Draw updates. Triggers only if entity is visible.
		/// 
		/// NOTE: DO NOT put any significant logic into Draw.
		/// It may skip frames.
		/// </summary>
		public virtual void Draw() 
		{
			foreach (var component in _componentList)
			{
				if (component.Visible)
				{
					component.Draw();
				}
			}
		}
		


		/// <summary>
		///	Triggers right before destruction, if entity is enabled. 
		/// </summary>
		public virtual void Destroy() 
		{
			foreach (var component in _componentDictionary.Values)
			{
				component.Destroy();
			}
		}

		#endregion Events.



		#region Components.

		/// <summary>
		/// Adds component to the entity.
		/// </summary>
		public void AddComponent(Component component)
		{
			if (component.Owner != null)
			{
				throw new Exception("Component " + component + "already has an owner!");
			}
			_componentDictionary.Add(component.GetType(), component);
			_componentList.Add(component);
			component.Owner = this;
			component.Initialize();
		}
		
		

		/// <summary>
		/// Returns component of given class.
		/// </summary>
		public T GetComponent<T>() where T : Component =>
			(T)_componentDictionary[typeof(T)];
		
		/// <summary>
		/// Returns component of given class.
		/// </summary>
		public Component GetComponent(Type type) =>
			_componentDictionary[type];
		

		/// <summary>
		/// Retrieves component of given class, if it exists, and returns true. If it doesn't, returns false.
		/// </summary>
		public bool TryGetComponent<T>(out T component) where T : Component
		{
			var result = _componentDictionary.TryGetValue(typeof(T), out Component c);
			component = (T)c; // Needs a manual cast.
			return result;
		}
		
		/// <summary>
		/// Retrieves component of given class, if it exists, and returns true. If it doesn't, returns false.
		/// </summary>
		public bool TryGetComponent(out Component component, Type type) =>
			_componentDictionary.TryGetValue(type, out component);
		

		/// <summary>
		/// Returns all the components. All of them.
		/// </summary>
		public Component[] GetAllComponents()
		{
			var array = new Component[_componentDictionary.Count];
			var id = 0;

			foreach(var componentPair in _componentDictionary)
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
			_componentDictionary.ContainsKey(typeof(T));
		
		/// <summary>
		/// Checks if an entity has the component of given type.
		/// </summary>
		public bool HasComponent(Type type) =>
			_componentDictionary.ContainsKey(type);
		

		
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
			if (_componentDictionary.TryGetValue(type, out Component component))
			{
				_componentDictionary.Remove(type);
				_componentList.Remove(component);
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
			if (EntityTemplatePool.TemplatePool.TryGetValue(tag, out IEntityTemplate factory))
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
			}
		}

	}
}
