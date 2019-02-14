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

		/// <summary>
		/// If component is enabled, it will be processed by systems.
		/// </summary>
		public bool Enabled {get; internal set;} = true;


		/// <summary>
		/// Creates a new object that is a copy of the current instance.
		/// 
		/// NOTE: If you are going to use json entity templates, 
		/// this method has to be properly implemented! 
		/// </summary>
		public abstract object Clone();
	}
}
