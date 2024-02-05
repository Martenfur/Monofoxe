using Monofoxe.Engine.Utils.CustomCollections;

namespace Monofoxe.Engine.Collisions
{
	public static class ColliderPool
	{
		private static Pool<Collider> _colliderPool = new Pool<Collider>();
		

		public static Collider GetCollider() =>
			_colliderPool.Get();

		
		public static void Return(Collider collider)
		{
			_colliderPool.Return(collider);
		}
	}
}
