using System;
using System.Collections.Generic;
using System.Linq;
using Monofoxe.Tiled.MapStructure.Objects;
using Monofoxe.Tiled.MapStructure;
using Monofoxe.Engine.SceneSystem;
using Monofoxe.Engine.ECS;


namespace Monofoxe.Tiled
{
	public static class MapMgr
	{
		/// <summary>
		/// Pool of all factories in all assemblies. Sorted by their tags.
		/// </summary>
		private static Dictionary<string, ITiledEntityFactory> _factoryPool;

		
		/// <summary>
		/// Initializes Tiled map loading stuff.
		/// HAS to be called in the beginning of the game, if you want to load Tiled maps.
		/// </summary>
		public static void Init()
		{
			InitFactoryPool();
		}



		/// <summary>
		/// Makes entity from Tiled temmplate using factory pool.
		/// </summary>
		public static Entity MakeEntity(TiledObject obj, Layer layer)
		{
			if (_factoryPool.ContainsKey(obj.Type))
			{
				return _factoryPool[obj.Type].Make(obj, layer);
			}
			return null;
		}


		/// <summary>
		/// Creates pool of all factories.
		/// </summary>
		private static void InitFactoryPool()
		{
			_factoryPool = new Dictionary<string, ITiledEntityFactory>();
			
			var factoryTypes = AppDomain.CurrentDomain.GetAssemblies().SelectMany(x => x.GetTypes());
			factoryTypes = factoryTypes.Where(
				mytype => typeof(ITiledEntityFactory).IsAssignableFrom(mytype) // Taking all classes immplementing interface.
			).Where(mytype => !mytype.IsInterface); // Filtering out interface itself.
			
			// Creating one instance of each.
			foreach(var systemType in factoryTypes)
			{
				var newFactory = (ITiledEntityFactory)Activator.CreateInstance(systemType);
				_factoryPool.Add(newFactory.Tag, newFactory);
			}
		}


	}
}
