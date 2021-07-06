using System.Collections.Generic;

namespace Monofoxe.Engine.Utils.CustomCollections
{
	/// <summary> 
	/// Safe list. Makes it possible to safely remove and add items to and from the list during enumeration.
	/// </summary>
	public class SafeList<T>
	{
		private List<T> _items, _outdatedItems;
		private bool _isOutdated = false;

		// TODO: Make parent for SafeSortedList. 

		public SafeList()
		{
			_items = new List<T>();
			_outdatedItems = new List<T>();
		}

		public SafeList(SafeList<T> list)
		{
			_items = new List<T>(list._items);
			_outdatedItems = new List<T>(list._items);
		}

		public SafeList(List<T> list)
		{
			_items = new List<T>(list);
			_outdatedItems = new List<T>(list);
		}


		public void Add(T item)
		{
			lock (_lock)
			{
				_isOutdated = true;
				_items.Add(item);
			}
		}

		public void Remove(T item)
		{
			lock (_lock)
			{
				_isOutdated = true;
				_items.Remove(item);
			}
		}

		public bool Contains(T item)
		{
			lock (_lock)
			{
				return _items.Contains(item);
			}
		}

		public void Insert(int index, T item)
		{
			lock (_lock)
			{
				_items.Insert(index, item);
			}
		}

		public void AddRange(SafeList<T> items)
		{
			lock (_lock)
			{
				_items.AddRange(items._items);
			}
		}

		public int Count =>
			_items.Count;

		/// <summary>
		/// Clears out all items from the list.
		/// </summary>
		public void Clear()
		{
			lock (_lock)
			{
				_isOutdated = true;
				_items.Clear();
			}
		}

		public T this[int index]
		{
			get => _items[index];
			set
			{
				lock (_lock)
				{
					_isOutdated = true;
					_items[index] = value;
				}
			}
		}

		public List<T> ToList()
		{
			lock (_lock)
			{
				return new List<T>(_items);
			}
		}

		public T[] ToArray()
		{
			lock (_lock)
			{
				return _items.ToArray();
			}
		}

		public void Sort(IComparer<T> comparer)
		{
			lock (_lock)
			{
				_isOutdated = true;
				_items.Sort(comparer);
			}
		}
		private static object _lock = new object();

		/// <summary>
		/// Removes old elements from the list and adds new ones.
		/// </summary>
		private void Update()
		{
			lock (_lock)
			{
				if (_isOutdated)
				{
					_outdatedItems.Clear();
					_outdatedItems.AddRange(_items);

					_isOutdated = false;
				}
			}
		}

		public List<T>.Enumerator GetEnumerator()
		{
			lock (_lock)
			{
				Update();
				return _outdatedItems.GetEnumerator();
			}
		}

	}

}
