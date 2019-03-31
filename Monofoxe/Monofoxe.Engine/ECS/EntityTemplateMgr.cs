using System;
using System.Collections.Generic;
using System.IO;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Monofoxe.Engine.ECS
{
	/// <summary>
	/// Contains and loads all entity templates.
	/// </summary>
	public static class EntityTemplateMgr
	{
		/// <summary>
		/// All entity templates. Used for creating entities.
		/// </summary>
		public static Dictionary<string, EntityTemplate> EntityTemplates = new Dictionary<string, EntityTemplate>();
		private static ContentManager _entityTemplatesContent = new ContentManager(GameMgr.Game.Services);

		private static string _configFileName = "_config.json";
		private static string _entityNamespacesKey = "entityNamespaces";
		private static string _spriteNamespacesKey = "spriteNamespaces";
		
		/// <summary>
		/// Loads entitiy templates from the content.
		/// 
		/// Needs to be called once to use CreateEntityFromTemplate().
		/// </summary>
		public static void LoadEntityTemplates()
		{
			ReadConfig();
			var info = AssetMgr.GetAssetPaths(AssetMgr.EntityTemplatesDir);
			_entityTemplatesContent.RootDirectory = AssetMgr.ContentDir;
			
			foreach(var entityPath in info)
			{
				if (!entityPath.EndsWith(_configFileName)) // Ignoring config.
				{
					var templates = _entityTemplatesContent.Load<List<EntityTemplate>>(entityPath);
					foreach(var template in templates)
					{
						EntityTemplates.Add(template.Tag.ToLower(), template);
					}
				}
			}
		}


		/// <summary>
		/// Reads config, which contains namespaces for entities and sprite groups.
		/// </summary>
		private static void ReadConfig()
		{
			JObject json;
			try
			{
				var stream = TitleContainer.OpenStream(
					AssetMgr.ContentDir + "/" + AssetMgr.EntityTemplatesDir + "/" + _configFileName
				);
				var reader = new StreamReader(stream);
				json = JObject.Parse(reader.ReadToEnd());
			}
			catch(Exception)
			{
				// File doesn't exists, so we just leave it as is.
				return;
			}
			

			if (json[_spriteNamespacesKey] != null)
			{
				Converters.SpriteConverter._namespaces = JsonConvert.DeserializeObject<string[]>(
					json[_spriteNamespacesKey].ToString()
				);
			}

			if (json[_entityNamespacesKey] != null)
			{
				ContentReaders.EntityTemplateReader._namespaces = JsonConvert.DeserializeObject<string[]>(
					json[_entityNamespacesKey].ToString()
				);
			}
		}

		
		
		
	}
}
