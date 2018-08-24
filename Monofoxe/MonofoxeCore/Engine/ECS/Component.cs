using System;

namespace Monofoxe.Engine.ECS
{
	/// <summary>
	/// Stores data, which will be processed by corresponding systems.
	/// </summary>
	public abstract class Component : ICloneable
	{
		/// <summary>
		/// Identifying tag. 
		/// 
		/// NOTE: Tags for different components HAVE to be unique.
		/// Systems will only process components with matching tags.
		/// </summary>
		public abstract string Tag {get;}

		/// <summary>
		/// Owner of a component.
		/// 
		/// NOTE: Component should ALWAYS have an owner. 
		/// </summary>
		public Entity Owner {get; internal set;}

		public abstract object Clone();

	}
}
