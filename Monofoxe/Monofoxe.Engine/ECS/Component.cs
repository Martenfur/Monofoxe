namespace Monofoxe.Engine.ECS
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
		/// Updates at a fixed rate, if entity is enabled.
		/// </summary>
		public abstract void FixedUpdate();



		/// <summary>
		/// Updates every frame, if entity is enabled.
		/// </summary>
		public abstract void Update();



		/// <summary>
		/// Draw updates. Triggers only if entity is visible.
		/// 
		/// NOTE: DO NOT put any significant logic into Draw.
		/// It may skip frames.
		/// </summary>
		public abstract void Draw();



		/// <summary>
		///	Triggers right before destruction, if entity is enabled. 
		/// </summary>
		public abstract void Destroy();

		#endregion Events.

	}
}
