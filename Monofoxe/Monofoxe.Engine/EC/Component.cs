namespace Monofoxe.Engine.EC
{
	/// <summary>
	/// Stores data, which will be processed by corresponding systems.
	/// </summary>
	public abstract class Component
	{
		/// <summary>
		/// Owner of a component.
		/// 
		/// NOTE: Component should ALWAYS have an owner. 
		/// </summary>
		public Entity Owner {get; internal set;}

		/// <summary>
		/// Tells if this component was initialized.
		/// </summary>
		public bool Initialized {get; internal set;} = false;

		/// <summary>
		/// If component is enabled, it will be processed by Create and Update methods.
		/// </summary>
		public bool Enabled = true;

		/// <summary>
		/// If component is visible, it will be processed by Draw method.
		/// 
		/// NOTE: components are NOT visible by default!
		/// </summary>
		public bool Visible = false;


		#region Events.

		/*
		 * Event order:
		 * - FixedUpdate
		 * - Update
		 * - Draw
		 */

		/// <summary>
		/// Gets called when component is added to the entity. 
		/// If removed and added several times, the event will still be called.
		/// </summary>
		public virtual void Initialize() { }

		

		/// <summary>
		/// Updates at a fixed rate, if entity is enabled.
		/// </summary>
		public virtual void FixedUpdate() { }



		/// <summary>
		/// Updates every frame, if entity is enabled.
		/// </summary>
		public virtual void Update() { }



		/// <summary>
		/// Draw updates. Triggers only if entity is visible.
		/// 
		/// NOTE: DO NOT put any significant logic into Draw.
		/// It may skip frames.
		/// </summary>
		public virtual void Draw() { }



		/// <summary>
		///	Triggers right before destruction.
		/// </summary>
		public virtual void Destroy() { }

		#endregion Events.

	}
}
