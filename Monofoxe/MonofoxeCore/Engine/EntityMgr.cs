using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Monofoxe.Engine.ECS;
using Microsoft.Xna.Framework.Content;
using Monofoxe.Engine.SceneSystem;

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
			foreach(var scene in SceneMgr.Scenes)
			{
				foreach(var layer in scene.Layers)
				{
					layer.UpdateEntities();
				}
			}
			
			SystemMgr.UpdateSystems();
			

			// Fixed updates.
			_fixedUpdateTimer += gameTime.ElapsedGameTime.TotalSeconds;

			if (_fixedUpdateTimer >= GameMgr.FixedUpdateRate)
			{
				var overflow = (int)(_fixedUpdateTimer / GameMgr.FixedUpdateRate); // In case of lags.
				_fixedUpdateTimer -= GameMgr.FixedUpdateRate * overflow;

				foreach(var scene in SceneMgr.Scenes)
				{
					SystemMgr.FixedUpdate(GetActiveComponents(scene));
				}
				foreach(var scene in SceneMgr.Scenes)
				{
					foreach(var layer in scene.Layers)
					{
						foreach(var entity in layer.Entities)
						{
							if (entity.Active && !entity.Destroyed)
							{
								entity.FixedUpdate();
							}
						}
					}
				}
			}
			// Fixed updates.


			// Normal updates.
			foreach(var scene in SceneMgr.Scenes)
			{
				SystemMgr.Update(GetActiveComponents(scene));
			}
			foreach(var scene in SceneMgr.Scenes)
			{
				foreach(var layer in scene.Layers)
				{
					foreach(var entity in layer.Entities)
					{
						if (entity.Active && !entity.Destroyed)
						{
							entity.Update(); 
						}
					}
				}
			}
			// Normal updates.


			// Updating depth list for drawing stuff.
			foreach(var scene in SceneMgr.Scenes)
			{
				foreach(var layer in scene.Layers)
				{
					layer.SortByDepth();
				}
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
		/// Returns a list of all active components in all layers.
		/// </summary>
		private static Dictionary<string, List<Component>> GetActiveComponents(Scene scene)
		{
			var list = new Dictionary<string, List<Component>>();
			
			foreach(var layer in scene.Layers)
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
