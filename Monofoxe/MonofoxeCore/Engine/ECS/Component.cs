
namespace Monofoxe.Engine.ECS
{
	/// <summary>
	/// Component interface.
	/// </summary>
	public class Component
	{
		/// <summary>
		/// Identifying tag. 
		/// 
		/// NOTE: Tags for different HAVE to be unique.
		/// Systems will only process components with matching tags.
		/// </summary>
		public string Tag {get; protected set;}

		/// <summary>
		/// Owner of a component.
		/// 
		/// NOTE: Component should ALWAYS have an owner. 
		/// </summary>
		public Entity Owner {get; internal set;}
	}
}
