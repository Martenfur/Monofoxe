using System;

namespace Monofoxe.Engine.ECS
{
	/// <summary>
	/// Stores data, which will be processed by corresponding systems.
	/// </summary>
	public abstract class Component : ICloneable
	{
		
		/// <summary>
		/// Owner of a component.
		/// 
		/// NOTE: Component should ALWAYS have an owner. 
		/// </summary>
		public Entity Owner {get; internal set;}

		public abstract object Clone();

	}
}
