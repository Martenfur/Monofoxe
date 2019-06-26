using Monofoxe.Engine.ECS;
using Monofoxe.Engine.SceneSystem;
using Monofoxe.Tiled;
using Monofoxe.Playground.TiledDemo.ExtendedMapBuilder;

namespace Monofoxe.Playground.TiledDemo
{
	public class TiledDemo : Entity
	{
		
		MapBuilder _builder;

		public TiledDemo(Layer layer) : base(layer)
		{
			// TiledMap which is loaded from Content, is just a data structure
			// describing the map. We need to make an actual Scene object with entities on it.
			// You can write your own map builder, or use the default one.
			// Default map builder can also be expanded.
			//_builder = new MapBuilder(Resources.Maps.Test);
			_builder = new SolidMapBuilder(Resources.Maps.Test);
			_builder.Build();

		}

		public override void Update()
		{
			
		}

		public override void Draw()
		{
		
		}

		public override void Destroy()
		{
			_builder.Destroy();
		}


	}
}
