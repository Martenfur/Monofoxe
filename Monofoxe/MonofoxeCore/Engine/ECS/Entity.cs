using System.Collections.Generic;

namespace Monofoxe.Engine.ECS
{
	
	/// <summary>
	/// Parent class of every in-game object.
	/// </summary>
	public class Entity
	{
		public readonly string Tag;
		
		/// <summary>
		/// Depth of Draw event. Objects with the lowest depth draw the last.
		/// </summary>
		public int Depth;
	
		/// <summary>
		/// Tells f object was destroyed.
		/// </summary>
		public bool Destroyed = false;

		/// <summary>
		/// If false, Update and Draw events won't be executed.
		/// </summary>
		public bool Active = true;


		private Dictionary<string, IComponent> _components;


		public Entity(string tag = "entity")
		{
			Objects.AddEntity(this);
			_components = new Dictionary<string, IComponent>();
			Tag = tag;
		}



		


		#region Events.

		/// <summary>
		/// Begin of the update at a fixed rate.
		/// </summary>
		public virtual void FixedUpdateBegin() {}		

		/// <summary>
		/// Update at a fixed rate.
		/// </summary>
		public virtual void FixedUpdate() {}
		
		/// <summary>
		/// End of the update at a fixed rate.
		/// </summary>
		public virtual void FixedUpdateEnd() {}

		

		/// <summary>
		/// Begin of the update at every frame.
		/// </summary>
		public virtual void UpdateBegin() {}		

		/// <summary>
		/// Update at every frame.
		/// </summary>
		public virtual void Update() {}
		
		/// <summary>
		/// End of the update at every frame.
		/// </summary>
		public virtual void UpdateEnd() {}

		

		/// <summary>
		/// Begin of the draw event.
		/// </summary>
		public virtual void DrawBegin() {}		
		
		/// <summary>
		/// Draw event.
		/// </summary>
		public virtual void Draw() {}
		
		/// <summary>
		/// End of the draw event.
		/// </summary>
		public virtual void DrawEnd() {}

		/// <summary>
		///	Drawing on a GUI layer. 
		/// </summary>
		public virtual void DrawGUI() {}



		/// <summary>
		///	Triggers right before destruction, if object is active. 
		/// </summary>
		public virtual void Destroy() {}

		#endregion Events.

		#region Components.


		/// <summary>
		/// Adds component to the entity.
		/// </summary>
		public void AddComponent(IComponent component)
		{
			_components.Add(component.Tag, component);
			component.Owner = this;
			ECSMgr.AddComponent(component);
		}
		


		public IComponent this[string tag]
		{
			get
			{
				if (_components.ContainsKey(tag))
				{
					return _components[tag];
				}
				return null;
			}
		}



		public T GetComponent<T>() where T : IComponent
		{
			foreach(KeyValuePair<string, IComponent> component in _components)
			{
				if (component.Value is T)
				{
					return (T)component.Value;
				}
			}
			return default(T);
		}

		/// <summary>
		/// Removes all components.
		/// </summary>
		internal void RemoveAllComponents()
		{
			foreach(KeyValuePair<string, IComponent> component in _components)
			{
				ECSMgr.RemoveComponent(component.Value);
			}
			_components.Clear();
		}

		#endregion Components.

	}
}
