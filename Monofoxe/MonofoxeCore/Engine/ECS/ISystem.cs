using System.Collections.Generic;

namespace Monofoxe.Engine.ECS
{
	/// <summary>
	/// Basic system interface. 
	/// </summary>
	public interface ISystem
	{
		/// <summary>
		/// Identifying tag. 
		/// 
		/// NOTE: Tags for different systems don't really have to be unique.
		/// Systems will only process components with matching tags.
		/// So, different systems with same tags will process same sets of components.
		/// </summary>
		string Tag {get;}
		
		
		/// <summary>
		/// Create event is called right after new component is created.
		/// 
		/// NOTE: This event won't be called right after component creation.
		/// It will be called in the very beginning of next step, so keep this in mind.
		/// However, you can speed up this process by calling 
		/// </summary>
		void Create(Component component);
		void Destroy(Component component);	
		
		void Update(List<Component> components);
		void Draw(List<Component> components);
	}
	

	public interface ISystemFixedUpdateEvents // TODO: Move back to ISystem.
	{
		void FixedUpdate(List<Component> components);
	}


}
