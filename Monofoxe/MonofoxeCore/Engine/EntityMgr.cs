using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Monofoxe.Engine;
using System.Diagnostics;
using Monofoxe.Engine.ECS;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.IO;
using Microsoft.Xna.Framework.Content;

namespace Monofoxe.Engine
{
	public static class EntityMgr
	{
		/// <summary>
		/// List of all entities.
		/// </summary>
		private static List<Entity> _entities = new List<Entity>();

		/// <summary>
		/// List of newly created entitiess. Since it won't be that cool 
		/// to modify main list in mid-step, they'll be added in next one.
		/// </summary>
		private static List<Entity> _newEntities = new List<Entity>();

		/// <summary>
		/// Entity list sorted by depth.
		/// </summary>
		private static List<Entity> _depthSortedEntities = new List<Entity>();


		/// <summary>
		/// Counts time until next fixed update.
		/// </summary>
		private static double _fixedUpdateTimer;


		private static Dictionary<string, EntityTemplate> _entityTemplates = new Dictionary<string, EntityTemplate>();
		private static ContentManager _entityTemplatesContent = new ContentManager(GameMgr.Game.Services);


		internal static void Update(GameTime gameTime)
		{		
			// Clearing main list from destroyed objects.
			var updatedList = new List<Entity>();
			foreach(Entity obj in _entities)
			{
				if (!obj.Destroyed)
				{
					updatedList.Add(obj);
				}
			}
			_entities = updatedList;
			// Clearing main list from destroyed objects.


			// Adding new objects to the list.
			_entities.AddRange(_newEntities);		
			_newEntities.Clear();
			// Adding new objects to the list.


			// Performing Create event for all newly added componenmts.
			ComponentSystemMgr.Create();


			// Fixed updates.
			_fixedUpdateTimer += gameTime.ElapsedGameTime.TotalSeconds;

			if (_fixedUpdateTimer >= GameMgr.FixedUpdateRate)
			{
				var overflow = (int)(_fixedUpdateTimer / GameMgr.FixedUpdateRate); // In case of lags.
				_fixedUpdateTimer -= GameMgr.FixedUpdateRate * overflow;

				ComponentSystemMgr.FixedUpdate();
				foreach(Entity obj in _entities)
				{
					if (obj.Active)
					{
						obj.FixedUpdateBegin();
					}
				}

				ComponentSystemMgr.FixedUpdateBegin();
				foreach(Entity obj in _entities)
				{
					if (obj.Active)
					{
						obj.FixedUpdate();
					}
				}

				ComponentSystemMgr.FixedUpdateEnd();
				foreach(Entity obj in _entities)
				{
					if (obj.Active)
					{
						obj.FixedUpdateEnd(); 
					}
				}
			}
			// Fixed updates.


			// Normal updates.
			ComponentSystemMgr.UpdateBegin();
			foreach(Entity obj in _entities)
			{
				if (obj.Active)
				{
					obj.UpdateBegin();
				}
			}

			ComponentSystemMgr.Update();
			foreach(Entity obj in _entities)
			{
				if (obj.Active)
				{ 
					obj.Update(); 
				}
			}

			ComponentSystemMgr.UpdateEnd();
			foreach(Entity obj in _entities)
			{
				if (obj.Active)
				{ 
					obj.UpdateEnd();
				}
			}
			// Normal updates.


			// Updating depth list for drawing stuff.
			_depthSortedEntities = _entities.OrderByDescending(o => o.Depth).ToList();
			ComponentSystemMgr.SortComponentsByDepth();
		}


		internal static void Draw()
		{
			ComponentSystemMgr.DrawBegin();
			foreach(Entity obj in _depthSortedEntities)
			{
				if (obj.Active)
				{
					obj.DrawBegin();
				}
			}

			ComponentSystemMgr.Draw();
			foreach(Entity obj in _depthSortedEntities)
			{
				if (obj.Active)
				{
					obj.Draw();
				}
			}
			
			ComponentSystemMgr.DrawEnd();
			foreach(Entity obj in _depthSortedEntities)
			{
				if (obj.Active)
				{
					obj.DrawEnd();
				}
			}
		}


		internal static void DrawGUI()
		{
			ComponentSystemMgr.DrawGUI();
			foreach(Entity obj in _depthSortedEntities)
			{
				if (obj.Active)
				{
					obj.DrawGUI();
				}
			}
		}



		/// <summary>
		/// Adds object to object list.
		/// </summary>
		internal static void AddEntity(Entity obj) => 
			_newEntities.Add(obj);
		


		public static void LoadEntityTemplates()
		{
			var info = AssetMgr.GetAssetPaths(AssetMgr.EntityTemplatesDir);

			_entityTemplatesContent.RootDirectory = AssetMgr.ContentDir;

			foreach(string entityPath in info)
			{
				var template = _entityTemplatesContent.Load<EntityTemplate>(entityPath);
				_entityTemplates.Add(template.Tag, template);
			}
		}





		#region User functions. 

		/// <summary>
		/// Destroys entity and all its components.
		/// </summary>
		public static void Destroy(Entity obj)
		{
			if (!obj.Destroyed)
			{
				obj.Destroyed = true;
				if (obj.Active)
				{
					obj.Destroy();
				}
				obj.RemoveAllComponents();
			}
		}


		/// <summary>
		/// Returns list of objects of certain type.
		/// </summary>
		public static List<T> GetList<T>() where T : Entity => 
			_entities.OfType<T>().ToList();


		/// <summary>
		/// Counts amount of objects of certain type.
		/// </summary>
		public static int Count<T>() where T : Entity => 
			_entities.OfType<T>().Count();


		/// <summary>
		/// Checks if any instances of an entity exist.
		/// </summary>
		public static bool EntityExists<T>() where T : Entity
		{
			foreach(Entity obj in _entities)
			{
				if (obj is T)
				{
					return true;
				}
			}
			return false;
		}


		/// <summary>
		/// Finds n-th entity of given type.
		/// </summary>
		public static T FindEntity<T>(int count) where T : Entity
		{
			var counter = 0;

			foreach(Entity obj in _entities)
			{
				if (obj is T)
				{
					if (counter >= count)
					{
						return (T)obj;
					}
					counter += 1;
				}
			}
			return null;
		}

		#endregion User functions.



		#region ECS user functions.

		public static Entity CreateEntity(string tag)
		{
			if (_entityTemplates.ContainsKey(tag))
			{
				var entity = new Entity(tag);

				foreach(Component component in _entityTemplates[tag].Components)
				{
					entity.AddComponent((Component)component.Clone());
				}
				return entity;
			}

			return null;
		}


		/// Due to ECS fun, there may be lots of objects with same type, 
		/// but different component sets. They differ only by their tag.
		/// This is why we need tag overloads.

		/// <summary>
		/// Returns list of entities with given tag.
		/// </summary>
		public static List<Entity> GetList(string tag)
		{
			var list = new List<Entity>();

			foreach(Entity obj in _entities)
			{
				if (obj.Tag == tag)
				{
					list.Add(obj);
				}
			}
			return list;
		}
		

		/// <summary>
		/// Counts amount of entities with given tag.
		/// </summary>
		public static int Count(string tag)
		{
			var counter = 0;

			foreach(Entity obj in _entities)
			{
				if (obj.Tag == tag)
				{
					counter += 1;
				}
			}
			return counter;
		}
		

		/// <summary>
		/// Checks if given instance exists.
		/// </summary>
		public static bool EntityExists(string tag)
		{
			foreach(Entity obj in _entities)
			{
				if (obj.Tag == tag)
				{
					return true;
				}
			}
			return false;
		}
		

		/// <summary>
		/// Finds n-th entities with given tag.
		/// </summary>
		public static Entity FindEntity(string tag, int count)
		{
			var counter = 0;

			foreach(Entity obj in _entities)
			{
				if (obj.Tag == tag)
				{
					if (counter >= count)
					{
						return obj;
					}
					counter += 1;
				}
			}
			return null;
		}

		#endregion ECS user functions.

	}
}
