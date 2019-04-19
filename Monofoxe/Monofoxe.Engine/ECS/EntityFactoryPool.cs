using System;
using System.Collections.Generic;

namespace Monofoxe.Engine.ECS
{
	/// <summary>
	/// Entity factory pool stores in instance of each entity factory.
	/// </summary>
	static class EntityFactoryPool
	{
		/// <summary>
		/// Pool of all factories in all assemblies. Sorted by their tags.
		/// </summary>
		public static Dictionary<string, IEntityFactory> FactoryPool;

		/// <summary>
		/// Initialized factory pool by creating an instance of each IEntityFactory class.
		/// </summary>
		public static void InitFactoryPool()
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
