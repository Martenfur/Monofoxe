using System;
using System.Collections.Generic;
using System.Text;
using Monofoxe.Engine.CustomCollections;
using Monofoxe.Engine.ECS;


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
		public IReadOnlyCollection<Layer> Layers => _layers.ToList();
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
			if (LayerExists(name))
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
					EntityMgr.DestroyEntity(entity);
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
				if (_layers[i].Name == name)
				{
					foreach(var entity in _layers[i].Entities)
					{
						EntityMgr.DestroyEntity(entity);
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
					if (layer.Name == name)
					{
						return layer;
					}
				}
				return null;
			}
		}


		/// <summary>
		/// Returns true, if there is a layer with given name. 
		/// </summary>
		public bool LayerExists(string name)
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
		/// Returns info about layers.
		/// </summary>
		public string __GetLayerInfo()
		{
			var str = new StringBuilder();

			foreach(var layer in _layers)
			{
				str.Append(
					layer.Name + 
					"; Priority: " + layer.Priority + 
					"; is GUI: " + layer.IsGUI + 
					"; Ent: " + layer.Entities.Count +
					Environment.NewLine
				);
			}

			return str.ToString();
		}

		#endregion Layer methods.



		#region Entity methods.

		/// <summary>
		/// Returns list of objects of certain type.
		/// </summary>
		public List<T> GetList<T>() where T : Entity
		{
			var entities = new List<T>();
			
			foreach(var layer in _layers)
			{
				entities.AddRange(layer.GetList<T>());
			}
			return entities;
		}
		
		/// <summary>
		/// Counts amount of objects of certain type.
		/// </summary>
		public int Count<T>() where T : Entity
		{
			var count = 0;
			
			foreach(var layer in _layers)
			{
				count += layer.Count<T>();				
			}
			return count;
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
		/// Returns list of entities with given tag.
		/// </summary>
		public List<Entity> GetList(string tag)
		{
			var list = new List<Entity>();

			foreach(var layer in _layers)
			{
				list.AddRange(layer.GetList(tag));
			}
			return list;
		}
		

		/// <summary>
		/// Counts amount of entities with given tag.
		/// </summary>
		public int Count(string tag)
		{
			var counter = 0;

			foreach(var layer in _layers)
			{
				counter += layer.Count(tag);
			}
			
			return counter;
		}
		

		/// <summary>
		/// Checks if given instance exists.
		/// </summary>
		public bool EntityExists(string tag)
		{
			foreach(var layer in _layers)
			{
				if (layer.EntityExists(tag))
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
			foreach(var layer in _layers)
			{
				var entity = layer.FindEntity(tag);
				if (entity != null)
				{
					return entity;
				}
			}
			
			return null;
		}

		#endregion Entity methods.
	}
}
