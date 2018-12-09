using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Monofoxe.Engine.SceneSystem;

namespace Monofoxe.Engine.ECS
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
					layer.UpdateEntityList();
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
					if (scene.Enabled)
					{
						SceneMgr.CurrentScene = scene;
						
						foreach(var layer in scene.Layers)
						{
							if (layer.Enabled)
							{
								SceneMgr.CurrentLayer = layer;
								SystemMgr.Update(layer._components);
								foreach(var entity in layer.Entities)
								{
									if (entity.Enabled && !entity.Destroyed)
									{
										entity.FixedUpdate();
									}
								}
							}
						}
					}
				}
			}
			// Fixed updates.

			
			// Normal updates.
			foreach(var scene in SceneMgr.Scenes)
			{		
				if (scene.Enabled)
				{
					SceneMgr.CurrentScene = scene;
					
					foreach(var layer in scene.Layers)
					{
						if (layer.Enabled)
						{
							SceneMgr.CurrentLayer = layer;
							SystemMgr.Update(layer._components);

							foreach(var entity in layer.Entities)
							{
								if (entity.Enabled && !entity.Destroyed)
								{
									entity.Update(); 
								}
							}
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

			foreach(var entityPath in info)
			{
				var template = _entityTemplatesContent.Load<EntityTemplate>(entityPath);
				_entityTemplates.Add(template.Tag, template);
			}
		}



		/// <summary>
		/// Creates new entity from existing template.
		/// </summary>
		public static Entity CreateEntityFromTemplate(Layer layer, string tag)
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
				if (entity.Enabled)
				{
					entity.Destroy();
				}
				entity.RemoveAllComponents();
			}
		}
		
		
	}
}
