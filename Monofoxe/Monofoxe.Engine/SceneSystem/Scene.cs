using System;
using System.Collections.Generic;
using System.Text;
using Monofoxe.Engine.EC;
using Monofoxe.Engine.Utils.CustomCollections;


namespace Monofoxe.Engine.SceneSystem
{
	/// <summary>
	/// Container for layers.
	/// </summary>
	public class Scene : IEntityMethods
	{
		public readonly string Name;
		
		/// <summary>
		/// List of all scene's layers.
		/// </summary>
		public List<Layer> Layers => _layers.ToList();
		internal SafeSortedList<Layer> _layers = new SafeSortedList<Layer>(x => x.Priority);

		/// <summary>
		/// If false, scene won't be rendered.
		/// </summary>
		public bool Visible = true;

		/// <summary>
		/// If true, scene won't be updated.
		/// </summary>
		public bool Enabled = true;

		/// <summary>
		/// Priority of a scene. Scenes with highest priority are processed first.
		/// </summary>
		public int Priority
		{
			get => _priority;

			set
			{
				_priority = value;
				SceneMgr._scenes.Remove(this); // Re-adding element to update its priority.
				SceneMgr._scenes.Add(this);
			}
		}
		private int _priority;


		public Scene(string name) =>
			Name = name;
		
		
		internal void Destroy()
		{
			foreach(var layer in _layers)
			{
				DestroyLayer(layer);
			}
			_layers.Clear(); // Also removes newly added layers from the list.
		}
		
		
		#region Layer methods.

		/// <summary>
		/// Creates new layer with given name.
		/// </summary>
		public Layer CreateLayer(string name, int priority = 0)
		{
			if (HasLayer(name))
			{
				throw new Exception("Layer with such name already exists!");
			}
			
			return new Layer(name, priority, this);
		}

		/// <summary>
		/// Destroys given layer.
		/// </summary>
		public void DestroyLayer(Layer layer)
		{
			if (_layers.Contains(layer))
			{
				foreach(var entity in layer.Entities)
				{
					entity.DestroyEntity();
				}
			}
			_layers.Remove(layer);
		}

		/// <summary>
		/// Destroys layer with given name.
		/// </summary>
		public void DestroyLayer(string name)
		{
			for(var i = _layers.Count - 1; i >= 0; i += 1)
			{
				if (string.Equals(_layers[i].Name, name, StringComparison.OrdinalIgnoreCase))
				{
					foreach(var entity in _layers[i].Entities)
					{
						entity.DestroyEntity();
					}
					_layers.Remove(_layers[i]);
				}
			}
		}


		/// <summary>
		/// Returns layer with given name.
		/// </summary>
		public Layer this[string name]
		{
			get
			{
				foreach(var layer in _layers)
				{
					if (string.Equals(layer.Name, name, StringComparison.OrdinalIgnoreCase))
					{
						return layer;
					}
				}
				return null;
			}
		}

		/// <summary>
		/// Finds layer with given name. Returns true, if layer was found.
		/// </summary>
		public bool TryGetLayer(string name, out Layer layer)
		{
			foreach(var l in _layers)
			{
				if (string.Equals(l.Name, name, StringComparison.OrdinalIgnoreCase))
				{
					layer = l;
					return true;
				}
			}
			layer = null;
			return false;
		}



		/// <summary>
		/// Returns true, if there is a layer with given name. 
		/// </summary>
		public bool HasLayer(string name)
		{
			foreach(var layer in _layers)
			{
				if (string.Equals(layer.Name, name, StringComparison.OrdinalIgnoreCase))
				{
					return true;
				}
			}
			return false;
		}
		
		#endregion Layer methods.



		#region Entity methods.

		/// <summary>
		/// Returns list of objects of certain type.
		/// </summary>
		public List<T> GetEntityList<T>() where T : Entity
		{
			var entities = new List<T>();
			
			foreach(var layer in _layers)
			{
				entities.AddRange(layer.GetEntityList<T>());
			}
			return entities;
		}
		

		/// <summary>
		/// Checks if any instances of an entity exist.
		/// </summary>
		public bool EntityExists<T>() where T : Entity
		{
			foreach(var layer in _layers)	
			{
				if (layer.EntityExists<T>())
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
			foreach(var layer in _layers)
			{
				var entity = layer.FindEntity<T>();
				if (entity != null)
				{
					return entity;
				}
			}
			return null;
		}
		
		
		/// <summary>
		/// Returns list of entities on a scene, which have component of given type.
		/// </summary>
		public List<Entity> GetEntityListByComponent<T>() where T : Component
		{
			var list = new List<Entity>();
			foreach(var layer in _layers)
			{
				list.AddRange(layer.GetEntityListByComponent<T>());
			}
			return list;
		}


		/// <summary>
		/// Finds first entity on a scene, which has component of given type.
		/// </summary>
		public Entity FindEntityByComponent<T>() where T : Component
		{
			foreach(var layer in _layers)
			{
				var entity = layer.FindEntityByComponent<T>();
				if (entity != null)
				{
					return entity;
				}
			}
			return null;
		}



		/// <summary>
		/// Returns list of all components on the scene - enabled and disabled - of given type.
		/// </summary>
		public List<Component> GetComponentList<T>() where T : Component
		{
			var list = new List<Component>();
			foreach(var layer in _layers)
			{
				list.AddRange(layer.GetComponentList<T>());
			}
			return list;
		}

		#endregion Entity methods.
	}
}
