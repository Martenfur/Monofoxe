using System;
using System.Collections.Generic;

namespace Monofoxe.Engine.ECS
{
	/// <summary>
	/// Basic system interface. 
	/// </summary>
	public abstract class BaseSystem
	{
		/// <summary>
		/// Type of components, which are processed by this system.
		/// </summary>
		public abstract Type ComponentType {get;}
		
		/// <summary>
		/// System priority. If priority is higher, system will be processed sooner.
		/// </summary>
		public virtual int Priority {get;}
		

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
