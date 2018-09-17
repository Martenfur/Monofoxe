using System;
using System.Collections.Generic;
using Monofoxe.Engine.Drawing;

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
		public int Depth;
	
		/// <summary>
		/// Tells f object was destroyed.
		/// </summary>
		public bool Destroyed {get; internal set;} = false;

		/// <summary>
		/// If false, Update and Draw events won't be executed.
		/// </summary>
		public bool Active = true;
		
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
		/// Component hash table.
		/// </summary>
		private Dictionary<string, Component> _components;


		public Entity(Layer layer, string tag = "entity")
		{
			_components = new Dictionary<string, Component>();
			Tag = tag;
			Layer = layer;
		}


		
		#region Events.

		/*
		 * Event order:
		 * - FixedUpdate
		 * - Update
		 * - Draw
		 * - DrawGUI
		 * 
		 * NOTE: Component events are executed before entity events.
		 */

		
		/// <summary>
		/// Update at a fixed rate.
		/// </summary>
		public virtual void FixedUpdate() {}
		
		
		
		/// <summary>
		/// Update at every frame.
		/// </summary>
		public virtual void Update() {}
		
		

		/// <summary>
		/// Draw event.
		/// </summary>
		public virtual void Draw() {}
		


		/// <summary>
		///	Triggers right before destruction, if entity is active. 
		/// </summary>
		public virtual void Destroy() {}

		#endregion Events.



		#region Components.


		/// <summary>
		/// Adds component to the entity.
		/// </summary>
		public void AddComponent(Component component)
		{
			_components.Add(component.Tag, component);
			component.Owner = this;
			Layer.AddComponent(component);
		}
		

		/// <summary>
		/// Returns component with given tag.
		/// </summary>
		public Component this[string tag] => 
			_components[tag];


		/// <summary>
		/// Returns component of given class.
		/// </summary>
		public T GetComponent<T>() where T : Component
		{
			foreach(KeyValuePair<string, Component> component in _components)
			{
				if (component.Value is T)
				{
					return (T)component.Value;
				}
			}
			throw(new Exception("Entity doesn't contain this component!"));
		}

		
		/// <summary>
		/// Returns all the components. All of them.
		/// </summary>
		public Component[] GetAllComponents()
		{
			var array = new Component[_components.Count];
			var id = 0;

			foreach(KeyValuePair<string, Component> component in _components)
			{
				array[id] = component.Value;
				id += 1;
			}

			return array;
		}


		/// <summary>
		/// Checks of an entity has component with given tag.
		/// </summary>
		public bool HasComponent(string tag) =>
			_components.ContainsKey(tag);
		
		
		/// <summary>
		/// Removes component from an entity and returns it.
		/// </summary>
		public Component RemoveComponent(string tag)
		{
			if (_components.ContainsKey(tag))
			{
				var component = _components[tag];
				_components.Remove(tag);
				Layer.RemoveComponent(component);
				component.Owner = null;
				return component;
			}
			return null;
		}


		/// <summary>
		/// Removes all components.
		/// </summary>
		internal void RemoveAllComponents()
		{
			foreach(KeyValuePair<string, Component> component in _components)
			{
				Layer.RemoveComponent(component.Value);
			}
			_components.Clear();
		}

		#endregion Components.

	}
}
