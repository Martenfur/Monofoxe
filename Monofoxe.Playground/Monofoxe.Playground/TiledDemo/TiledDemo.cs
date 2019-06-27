using Monofoxe.Engine;
using Monofoxe.Engine.ECS;
using Monofoxe.Engine.SceneSystem;
using Monofoxe.Playground.TiledDemo.ExtendedMapBuilder;
using Monofoxe.Tiled;


namespace Monofoxe.Playground.TiledDemo
{
	public class TiledDemo : Entity
	{

		MapBuilder _builder;

		public const Buttons BuildCustomMapBuilderButton = Buttons.B;
		public const Buttons BuildDefaultMapBuilderButton = Buttons.N;
		public const Buttons DestroyMapButton = Buttons.U;


		public TiledDemo(Layer layer) : base(layer)
		{
			// TiledMap which is loaded from Content, is just a data structure
			// describing the map. We need to make an actual Scene object with entities on it.
			// You can write your own map builder, or use the default one.
			// Default map builder can also be expanded.

			_builder = new SolidMapBuilder(Resources.Maps.Test);
			_builder.Build();
		}

		public override void Update()
		{

			if (Input.CheckButtonPress(BuildCustomMapBuilderButton))
			{
				if (_builder != null)
				{
					_builder.Destroy();
					_builder = null;
				}
				_builder = new SolidMapBuilder(Resources.Maps.Test);
				_builder.Build();
			}

			if (Input.CheckButtonPress(BuildDefaultMapBuilderButton))
			{
				if (_builder != null)
				{
					_builder.Destroy();
					_builder = null;
				}
				_builder = new MapBuilder(Resources.Maps.Test);
				_builder.Build();
			}


			if (_builder != null && Input.CheckButtonPress(DestroyMapButton))
			{
				_builder.Destroy();
				_builder = null;
			}
			
		}


		public override void Destroy()
		{
			if (_builder != null)
			{
				_builder.Destroy();
			}
		}


	}
}
