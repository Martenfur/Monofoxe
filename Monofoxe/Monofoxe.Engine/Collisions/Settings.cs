namespace Monofoxe.Engine.Collisions
{
	public static class Settings
	{
		public const float MaxFloat = 3.402823466e+38f;
		public const float Epsilon = 1.192092896e-07f;


		/// <summary>
		/// The maximum number of vertices on a convex polygon.
		/// </summary>
		public static int MaxPolygonVertices = 8;
		
		/// <summary>
		/// It is recommended to downscale your colliders and not make them too large, that's why collider sizes are measured in meters and not pixels. 
		/// This value signifies how many pixels there are in one meter.
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

		public static float OneOverWorldScale { get; private set; } = 1 / _worldScale;
	}
}
