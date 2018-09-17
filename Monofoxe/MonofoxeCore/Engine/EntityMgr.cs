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
using Monofoxe.Engine.Drawing;

namespace Monofoxe.Engine
{
	public static class EntityMgr
	{

		/// <summary>
		/// Counts time until next fixed update.
		/// </summary>
		private static double _fixedUpdateTimer;


		private static Dictionary<string, EntityTemplate> _entityTemplates = new Dictionary<string, EntityTemplate>();
		private static ContentManager _entityTemplatesContent = new ContentManager(GameMgr.Game.Services);


		internal static void Update(GameTime gameTime)
		{
			// Clearing main list from destroyed objects.
			foreach(var layer in Layer.Layers)
			{
				var updatedList = new List<Entity>();
				foreach(Entity obj in layer._entities)
				{
					if (!obj.Destroyed)
					{
						updatedList.Add(obj);
					}
				}
				layer._entities = updatedList;
				// Clearing main list from destroyed objects.


				// Adding new objects to the list.
				layer._entities.AddRange(layer._newEntities);		
				layer._newEntities.Clear();
				// Adding new objects to the list.
			}
			
			SystemMgr.UpdateSystems();
			

			// Fixed updates.
			_fixedUpdateTimer += gameTime.ElapsedGameTime.TotalSeconds;

			if (_fixedUpdateTimer >= GameMgr.FixedUpdateRate)
			{
				var overflow = (int)(_fixedUpdateTimer / GameMgr.FixedUpdateRate); // In case of lags.
				_fixedUpdateTimer -= GameMgr.FixedUpdateRate * overflow;

				SystemMgr.FixedUpdate(GetActiveComponents());
				foreach(var layer in Layer.Layers)
				{
					foreach(var entity in layer._entities)
					{
						if (entity.Active && !entity.Destroyed)
						{
							entity.FixedUpdate();
						}
					}
				}
			}
			// Fixed updates.


			// Normal updates.
			SystemMgr.Update(GetActiveComponents());
			foreach(var layer in Layer.Layers)
			{
				foreach(var entity in layer._entities)
				{
					if (entity.Active && !entity.Destroyed)
					{
						entity.Update(); 
					}
				}
			}
			// Normal updates.


			// Updating depth list for drawing stuff.
			foreach(var layer in Layer.Layers)
			{
				layer.SortByDepth();
			}
		}
		
		
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
		/// Destroys entity and all of its components.
		/// </summary>
		public static void DestroyEntity(Entity entity)
		{
			if (!entity.Destroyed)
			{
				entity.Destroyed = true;
				if (entity.Active)
				{
					entity.Destroy();
				}
				entity.RemoveAllComponents();
			}
		}


		/// <summary>
		/// Returns list of objects of certain type.
		/// </summary>
		public static List<T> GetList<T>() where T : Entity
		{ 
			var entities = new List<T>();
			foreach(var layer in Layer.Layers)
			{
				entities.AddRange(layer._entities.OfType<T>());
			}
			return entities;
		}
		
		/// <summary>
		/// Counts amount of objects of certain type.
		/// </summary>
		public static int Count<T>() where T : Entity
		{
			var count = 0;
			foreach(var layer in Layer.Layers)
			{
				count += layer._entities.OfType<T>().Count();
			}
			return count;
		}

		/// <summary>
		/// Checks if any instances of an entity exist.
		/// </summary>
		public static bool EntityExists<T>() where T : Entity
		{
			foreach(var layer in Layer.Layers)
			{
				foreach(var entity in layer._entities)
				{
					if (entity is T)
					{
						return true;
					}
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

			foreach(var layer in Layer.Layers)
			{
				foreach(var entity in layer._entities)
				{
					if (entity is T)
					{
						if (counter >= count)
						{
							return (T)entity;
						}
						counter += 1;
					}
				}
			}
			return null;
		}

		#endregion User functions.



		#region ECS user functions.

		public static Entity CreateEntity(Layer layer, string tag)
		{
			if (_entityTemplates.ContainsKey(tag))
			{
				var entity = new Entity(layer, tag);

				foreach(var component in _entityTemplates[tag].Components)
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

			foreach(var layer in Layer.Layers)
			{
				foreach(var entity in layer._entities)
				{
					if (entity.Tag == tag)
					{
						list.Add(entity);
					}
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

			foreach(var layer in Layer.Layers)
			{
				foreach(var entity in layer._entities)
				{
					if (entity.Tag == tag)
					{
						counter += 1;
					}
				}
			}
			return counter;
		}
		

		/// <summary>
		/// Checks if given instance exists.
		/// </summary>
		public static bool EntityExists(string tag)
		{
			foreach(var layer in Layer.Layers)
			{
				foreach(var entity in layer._entities)
				{
					if (entity.Tag == tag)
					{
						return true;
					}
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

			foreach(var layer in Layer.Layers)
			{
				foreach(var entity in layer._entities)
				{
					if (entity.Tag == tag)
					{
						if (counter >= count)
						{
							return entity;
						}
						counter += 1;
					}
				}
			}
			return null;
		}

		#endregion ECS user functions.



		
		private static Dictionary<string, List<Component>> GetActiveComponents()
		{
			var list = new Dictionary<string, List<Component>>();
			foreach(var layer in Layer.Layers)
			{
				foreach(var componentsPair in layer._components)
				{
					if (list.ContainsKey(componentsPair.Key))
					{
						list[componentsPair.Key].AddRange(ComponentMgr.FilterInactiveComponents(componentsPair.Value));
					}
					else
					{
						list.Add(componentsPair.Key, ComponentMgr.FilterInactiveComponents(componentsPair.Value));
					}
				}
			}
			return list;
		}


	}
}
