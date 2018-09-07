using System;
using System.Collections.Generic;
using System.Text;
using Monofoxe.Engine;
using Microsoft.Xna.Framework;
using Monofoxe.Engine.ECS;
using System.Linq;
using System.Collections;


namespace Monofoxe.Engine.Drawing
{
	public class Layer
	{
		public int Depth;

		public bool DepthSorting = false;

		private List<Entity> _entities = new List<Entity>();

		/// <summary>
		/// Component dictionary.
		/// </summary>
		internal Dictionary<string, List<Component>> _components = new Dictionary<string, List<Component>>();
		
		/// <summary>
		/// Newly created components. Used for Create event.
		/// </summary>
		internal List<Component> _newComponents = new List<Component>();
		Dictionary<string, List<Component>> _depthSortedComponents = new Dictionary<string, List<Component>>();


		/// <summary>
		/// Tells if any components were removed in the current step.
		/// </summary>
		bool _componentsWereRemoved = false;



		internal void Draw()
		{
			ComponentSystemMgr.DrawBegin(_components);
			foreach(Entity obj in _entities)
			{
				if (obj.Active && !obj.Destroyed)
				{
					obj.DrawBegin();
				}
			}

			ComponentSystemMgr.Draw(_components);
			foreach(Entity obj in _entities)
			{
				if (obj.Active && !obj.Destroyed)
				{
					obj.Draw();
				}
			}
			
			ComponentSystemMgr.DrawEnd(_components);
			foreach(Entity obj in _entities)
			{
				if (obj.Active && !obj.Destroyed)
				{
					obj.DrawEnd();
				}
			}
		}


		internal void DrawGUI()
		{
			ComponentSystemMgr.DrawGUI(_components);
			foreach(Entity obj in _entities)
			{
				if (obj.Active && !obj.Destroyed)
				{
					obj.DrawGUI();
				}
			}
		}


		public void AddEntity(Entity entity) =>
			_entities.Add(entity);
		
		public void RemoveEntity(Entity entity) =>
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
			if (ComponentSystemMgr._activeSystems.ContainsKey(component.Tag))
			{
				ComponentSystemMgr._activeSystems[component.Tag].Destroy(component);
			}

			_componentsWereRemoved = true;
		}


		/// <summary>
		/// Enables and disables systems depending on if there are any components for them.
		/// </summary>
		internal void UpdateSystems()
		{

			// Managing new components.
			if (_newComponents.Count > 0)
			{
				foreach(var component in _newComponents)
				{
					if (ComponentSystemMgr.AutoSystemManagement && !ComponentSystemMgr._activeSystems.ContainsKey(component.Tag))
					{
						if (ComponentSystemMgr._systemPool.ContainsKey(component.Tag))
						{
							var newSystem = ComponentSystemMgr._systemPool[component.Tag];
							ComponentSystemMgr._activeSystems.Add(component.Tag, newSystem);
							newSystem.Create(component);
						}
					}
					else
					{
						ComponentSystemMgr._activeSystems[component.Tag].Create(component);
					}

					if (_components.ContainsKey(component.Tag))
					{
						_components[component.Tag].Add(component);
					}
					else
					{
						var list = new List<Component>(new Component[] {component});
						_components.Add(component.Tag, list);
					}
				}
				_newComponents.Clear();
			}
			// Managing new components.
			

			// Disabling systems without components.
			if (_componentsWereRemoved)
			{
				foreach(var componentListPair in _components.ToList())
				{
					if (componentListPair.Value.Count == 0)
					{
						_components.Remove(componentListPair.Key);
						if (ComponentSystemMgr.AutoSystemManagement)
						{
							ComponentSystemMgr._activeSystems.Remove(componentListPair.Key);
						}
					}
				}
				_componentsWereRemoved = false;
			}
			// Disabling systems without components.
		}
		
	}
}
