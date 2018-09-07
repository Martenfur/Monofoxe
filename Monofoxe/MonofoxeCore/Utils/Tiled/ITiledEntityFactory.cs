using MonoGame.Extended.Tiled;
using Monofoxe.Engine.ECS;


namespace Monofoxe.Utils.Tiled
{
	public interface ITiledEntityFactory
	{
		string Name {get;}
		Entity Make(TiledMapObject obj);
	}
}
