using Monofoxe.Engine.ECS;
using Monofoxe.Tiled.MapStructure.Objects;
using Monofoxe.Engine.SceneSystem;

namespace Monofoxe.Tiled
{
	public interface ITiledEntityFactory
	{
		/// <summary>
		/// Identifying tag.
		/// 
		/// NOTE: All factory tags should be unique!
		/// </summary>
		string Tag {get;}

		/// <summary>
		/// Creates entity from Tiled Object on a given layer.
		/// </summary>
		Entity Make(TiledObject obj, Layer layer);
	}
}
