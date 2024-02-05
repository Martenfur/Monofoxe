using Monofoxe.Engine.Collisions.Colliders;
using Monofoxe.Engine.Utils.CustomCollections;

namespace Monofoxe.Engine.Collisions
{
	public static class ColliderPool
	{
		private static Pool<Collider> _colliderPool = new Pool<Collider>();
		private static Pool<CircleCollider> _circleColliderPool = new Pool<CircleCollider>();
		private static Pool<RectangleCollider> _rectangleColliderPool = new Pool<RectangleCollider>();
		private static Pool<LineCollider> _lineColliderPool = new Pool<LineCollider>();


		public static Collider GetCollider() => _colliderPool.Get();
		public static CircleCollider GetCircleCollider() => _circleColliderPool.Get();
		public static RectangleCollider GetRectangleCollider() => _rectangleColliderPool.Get();
		public static LineCollider GetLineCollider() => _lineColliderPool.Get();

		
		public static void Return(Collider collider) => _colliderPool.Return(collider);
		public static void Return(CircleCollider collider) => _circleColliderPool.Return(collider);
		public static void Return(RectangleCollider collider) => _rectangleColliderPool.Return(collider);
		public static void Return(LineCollider collider) => _lineColliderPool.Return(collider);
	}
}
