using System;
using System.Collections.Generic;
using Monofoxe.Engine;
using Monofoxe.Engine.ECS;
using Monofoxe.Engine.SceneSystem;
using Monofoxe.Tiled.MapStructure.Objects;

namespace Monofoxe.Tiled
{
	/// <summary>
	/// Stores an instance of each class which implements ITiledEntityFactory.
	/// </summary>
	public static class TiledEntityFactoryPool
	{
		/// <summary>
		/// Pool of all factories in all assemblies. Sorted by their tags.
		/// </summary>
		private static Dictionary<string, ITiledEntityFactory> _factoryPool;

		/// <summary>
		/// Creates pool of all factories.
		/// </summary>
		public static void InitFactoryPool()
		{
			_factoryPool = new Dictionary<string, ITiledEntityFactory>();
			
			// Creating an instance of each.
			foreach(var type in GameMgr.Types)
			{
				if (typeof(ITiledEntityFactory).IsAssignableFrom(type.Value) && !type.Value.IsInterface)
				{
					var newFactory = (ITiledEntityFactory)Activator.CreateInstance(type.Value);
					_factoryPool.Add(newFactory.Tag.ToLower(), newFactory);
				}
			}
		}

		/// <summary>
		/// Makes entity from Tiled temmplate using factory pool.
		/// </summary>
		public static Entity MakeEntity(TiledObject obj, Layer layer, MapBuilder map)
		{
			if (_factoryPool.TryGetValue(obj.Type.ToLower(), out ITiledEntityFactory factory))
			{
				return factory.Make(obj, layer, map);
			}
			return null;
		}

	}
}
