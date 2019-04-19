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
		public bool Enabled 
		{
			get => _enabled; 
			set
			{
				if (_enabled != value)
				{
					_enabled = value;
					if (value)
					{
						Owner.Layer.EnableComponent(this);
					}
					else
					{
						Owner.Layer.DisableComponent(this);
					}
				}
			}
		}
		private bool _enabled = true;
		
		/// <summary>
		/// If component is visible, it will be processed by Draw method.
		/// 
		/// NOTE: components are NOT visible by default!
		/// </summary>
		public bool Visible = false;
		

		/// <summary>
		/// Component's assigned system. Will be null only if no systems exist for this component.
		/// </summary>
		public BaseSystem System {get; internal set;}
	}
}
