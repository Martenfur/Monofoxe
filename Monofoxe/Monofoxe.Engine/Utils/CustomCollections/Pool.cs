using System;

namespace Monofoxe.Engine.Utils.CustomCollections
{
	public class Pool<T> where T : IPoolable, new()
	{
		private UnorderedList<T> _items = new UnorderedList<T>(32);

		public T Get()
		{
			if (_items.Count > 0)
			{
				T obj;
				obj = _items[_items.Count - 1];
				_items.RemoveLast();

				obj.OnTakenFromPool();
				obj.InPool = false;
				return obj;
			}
			else
			{
				T obj = new T();
				obj.OnTakenFromPool();
				obj.InPool = false;
				return obj;
			}
		}


		public void Return(T obj)
		{
			if (obj.InPool)
			{
				throw new InvalidOperationException("Provided object is already in a pool!");
			}
			obj.OnReturnedToPool();
			obj.InPool = true;
			_items.Add(obj);
		}
	}
}
