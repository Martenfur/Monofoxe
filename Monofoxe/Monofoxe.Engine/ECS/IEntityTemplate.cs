using Monofoxe.Engine.SceneSystem;

namespace Monofoxe.Engine.ECS
{
	/// <summary>
	/// Entity template interface. Used to create entity by tag.
	/// </summary>
	public interface IEntityTemplate
	{
		/// <summary>
		/// Identifying tag. Will be assigned to created entities.
		/// 
		/// NOTE: All template tags should be unique!
		/// </summary>
		string Tag {get;}

		/// <summary>
		/// Creates an entity on a given layer.
		/// </summary>
		Entity Make(Layer layer);
	}
}
