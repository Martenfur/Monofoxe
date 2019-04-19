using Monofoxe.Engine.SceneSystem;

namespace Monofoxe.Engine.ECS
{
	// TODO: Maybe rename to Template?

	/// <summary>
	/// Entity factory interface. Used to create entity by tag.
	/// </summary>
	public interface IEntityFactory
	{
		/// <summary>
		/// Identifying tag. Will be assigned to created entities.
		/// 
		/// NOTE: All factory tags should be unique!
		/// </summary>
		string Tag {get;}

		/// <summary>
		/// Creates an entity on a given layer.
		/// </summary>
		Entity Make(Layer layer);
	}
}
