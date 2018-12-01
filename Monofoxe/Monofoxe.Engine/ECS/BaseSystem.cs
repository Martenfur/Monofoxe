using System.Collections.Generic;

namespace Monofoxe.Engine.ECS
{
	/// <summary>
	/// Basic system interface. 
	/// </summary>
	public abstract class BaseSystem
	{
		/// <summary>
		/// Identifying tag. 
		/// 
		/// NOTE: Tags for different systems don't really have to be unique.
		/// Systems will only process components with matching tags.
		/// So, different systems with same tags will process same sets of components.
		/// </summary>
		public abstract string Tag {get;}
		
		/// <summary>
		/// Tells, how many layers are using this system.
		/// </summary>
		internal int _usedLayersCount;

		/// <summary>
		/// Create event is called right after new component is created.
		/// 
		/// NOTE: This event won't be called right after component creation.
		/// It will be called in the very beginning of next step, so keep this in mind.
		/// However, you can speed up this process by calling InitComponent().
		/// </summary>
		public virtual void Create(Component component) {}
		public virtual void Destroy(Component component) {}
		
		public virtual void FixedUpdate(List<Component> components) {}
		public virtual void Update(List<Component> components) {}
		public virtual void Draw(Component component) {}
	}

}
