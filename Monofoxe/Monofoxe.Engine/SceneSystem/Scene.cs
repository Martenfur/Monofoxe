using Microsoft.Xna.Framework;
using Monofoxe.Engine.Cameras;
using Monofoxe.Engine.Drawing;
using Monofoxe.Engine.EC;
using Monofoxe.Engine.Utils.CustomCollections;
using System;
using System.Collections.Generic;


namespace Monofoxe.Engine.SceneSystem
{
	public delegate void SceneEventDelegate(Scene scene);

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
		/// If false, scene won't be updated.
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


		/// <summary>
		/// Current active layer.
		/// </summary>
		public static Layer CurrentLayer { get; private set; }


		public Scene(string name) =>
			Name = name;


		internal void Destroy()
		{
			for (var i = 0; i < _layers.Count; i += 1)
			{
				DestroyLayer(_layers[i]);
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
				foreach (var entity in layer.Entities)
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
			for (var i = _layers.Count - 1; i >= 0; i += 1)
			{
				if (string.Equals(_layers[i].Name, name, StringComparison.OrdinalIgnoreCase))
				{
					foreach (var entity in _layers[i].Entities)
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
				for (var i = 0; i < _layers.Count; i += 1)
				{
					if (string.Equals(_layers[i].Name, name, StringComparison.OrdinalIgnoreCase))
					{
						return _layers[i];
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
			for (var i = 0; i < _layers.Count; i += 1)
			{
				if (string.Equals(_layers[i].Name, name, StringComparison.OrdinalIgnoreCase))
				{
					layer = _layers[i];
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
			for (var i = 0; i < _layers.Count; i += 1)
			{
				if (string.Equals(_layers[i].Name, name, StringComparison.OrdinalIgnoreCase))
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

			for (var i = 0; i < _layers.Count; i += 1)
			{
				entities.AddRange(_layers[i].GetEntityList<T>());
			}
			return entities;
		}


		/// <summary>
		/// Checks if any instances of an entity exist.
		/// </summary>
		public bool EntityExists<T>() where T : Entity
		{
			for (var i = 0; i < _layers.Count; i += 1)
			{
				if (_layers[i].EntityExists<T>())
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
			for (var i = 0; i < _layers.Count; i += 1)
			{
				var entity = _layers[i].FindEntity<T>();
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
			for (var i = 0; i < _layers.Count; i += 1)
			{
				list.AddRange(_layers[i].GetEntityListByComponent<T>());
			}
			return list;
		}


		/// <summary>
		/// Finds first entity on a scene, which has component of given type.
		/// </summary>
		public Entity FindEntityByComponent<T>() where T : Component
		{
			for (var i = 0; i < _layers.Count; i += 1)
			{
				var entity = _layers[i].FindEntityByComponent<T>();
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
			for (var i = 0; i < _layers.Count; i += 1)
			{
				list.AddRange(_layers[i].GetComponentList<T>());
			}
			return list;
		}

		#endregion Entity methods.



		#region Events.
		
		/// <summary>
		/// Triggers every frame before all layers perform Update.
		/// </summary>
		public event SceneEventDelegate OnPreUpdate;
		/// <summary>
		/// Triggers every frame after all layers perform Update.
		/// </summary>
		public event SceneEventDelegate OnPostUpdate;
		/// <summary>
		/// Triggers every frame before all layers perform FixedUpdate.
		/// </summary>
		public event SceneEventDelegate OnPreFixedUpdate;
		/// <summary>
		/// Triggers every frame after all layers perform FixedUpdate.
		/// </summary>
		public event SceneEventDelegate OnPostFixedUpdate;
		/// <summary>
		/// Triggers every frame before all non-GUI layers perform Draw.
		/// </summary>
		public event SceneEventDelegate OnPreDraw;
		/// <summary>
		/// Triggers every frame after all non-GUI layers perform Draw.
		/// </summary>
		public event SceneEventDelegate OnPostDraw;
		/// <summary>
		/// Triggers every frame before all GUI layers perform Draw.
		/// </summary>
		public event SceneEventDelegate OnPreDrawGUI;
		/// <summary>
		/// Triggers every frame after all GUI layers perform Draw.
		/// </summary>
		public event SceneEventDelegate OnPostDrawGUI;


		internal void FixedUpdate()
		{
			OnPreFixedUpdate?.Invoke(this);
			foreach (var layer in _layers)
			{
				if (layer.Enabled)
				{
					CurrentLayer = layer;

					layer.FixedUpdate();
				}
			}
			OnPostFixedUpdate?.Invoke(this);
		}

		internal void Update()
		{
			OnPreUpdate?.Invoke(this);
			foreach (var layer in _layers)
			{
				if (layer.Enabled)
				{
					CurrentLayer = layer;

					layer.Update();
				}
			}
			OnPostUpdate?.Invoke(this);
		}


		internal void Draw()
		{
			OnPreDraw?.Invoke(this);
			foreach (var layer in _layers)
			{
				if (
					layer.Visible &&
					!layer.IsGUI &&
					!GraphicsMgr.CurrentCamera.Filter(Name, layer.Name)
				)
				{
					CurrentLayer = layer;

					layer.Draw();
				}
			}
			OnPostDraw?.Invoke(this);
		}

		internal void DrawGUI()
		{
			OnPreDrawGUI?.Invoke(this);
			foreach (var layer in _layers)
			{
				if (layer.Visible && layer.IsGUI)
				{
					CurrentLayer = layer;

					layer.DrawGUI();
				}
			}
			OnPostDrawGUI?.Invoke(this);
		}

		#endregion Events.

	}
}
