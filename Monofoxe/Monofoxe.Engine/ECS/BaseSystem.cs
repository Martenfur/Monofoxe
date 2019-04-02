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
		/// Tells, if any layer is currently using this system.
		/// </summary>
		internal bool _usedByLayers;

		/// <summary>
		/// If system is enabled, it will invoke its Update and Draw methods.
		/// </summary>
		public bool Enabled {get; internal set;} = false;

		
		public virtual void Create(Component component) {}
		public virtual void Destroy(Component component) {}
		
		public virtual void FixedUpdate(List<Component> components) {}
		public virtual void Update(List<Component> components) {}
		public virtual void Draw(Component component) {}
	}

}
