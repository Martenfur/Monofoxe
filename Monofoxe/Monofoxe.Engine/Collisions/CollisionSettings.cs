namespace Monofoxe.Engine.Collisions
{
	public static class CollisionSettings
	{
		public const float Epsilon = 1.192092896e-07f;

		/// <summary>
		/// The maximum number of vertices on a convex polygon. It is recommended to keep this number low for performance reasons.
		/// </summary>
		public static int MaxPolygonVertices = 8;
		
		/// <summary>
		/// This value signifies how many pixels there are in one meter.
		/// Making colliders too large will result in performance issues, that's why collider sizes are downscaled and measured in meters. 
		/// </summary>
		public static float WorldScale 
		{
			get => _worldScale;
			set
			{
				_worldScale = value;
				OneOverWorldScale = 1 / _worldScale;
			}
		}
		private static float _worldScale = 64f;

		/// <summary>
		/// This value signifies how many meters there are in one pixel.
		/// Making colliders too large will result in performance issues, that's why collider sizes are downscaled and measured in meters. 
		/// </summary>
		public static float OneOverWorldScale { get; private set; } = 1 / _worldScale;
	}
}
