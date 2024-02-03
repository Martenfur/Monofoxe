namespace Monofoxe.Engine.Utils.CustomCollections
{
	public interface IPoolable
	{
		bool InPool { get; set; }

		void OnTakenFromPool();
		void OnReturnedToPool();
	}
}
