namespace Monofoxe.Engine.Utils.CustomCollections
{
	public interface IPoolable
	{
		/// <summary>
		/// Signifies that the object is currently in a pool.
		/// </summary>
		bool InPool { get; set; }

		/// <summary>
		/// Called when the object is taken from a pool.
		/// </summary>
		void OnTakenFromPool();

		/// <summary>
		/// Called when the object is returned to a pool.
		/// </summary>
		void OnReturnedToPool();
	}
}
