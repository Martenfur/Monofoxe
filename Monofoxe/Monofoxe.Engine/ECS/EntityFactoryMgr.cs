using System;
using System.Collections.Generic;
using Monofoxe.Engine.SceneSystem;


namespace Monofoxe.Engine.ECS
{
	static class EntityFactoryMgr
	{
		/// <summary>
		/// Pool of all factories in all assemblies. Sorted by their tags.
		/// </summary>
		internal static Dictionary<string, IEntityFactory> FactoryPool;

		// TODO: Rename to EntityFactoryMgr.

		/// <summary>
		/// Initializes Tiled map loading stuff.
		/// HAS to be called in the beginning of the game, if you want to load Tiled maps.
		/// </summary>
		public static void Init()
		{
			FactoryPool = new Dictionary<string, IEntityFactory>();
			
			// Creating an instance of each.
			foreach(var type in GameMgr.Types)
			{
				if (typeof(IEntityFactory).IsAssignableFrom(type.Value) && !type.Value.IsInterface)
				{
					var newFactory = (IEntityFactory)Activator.CreateInstance(type.Value);
					FactoryPool.Add(newFactory.Tag.ToLower(), newFactory);
				}
			}
		}
	}
}
