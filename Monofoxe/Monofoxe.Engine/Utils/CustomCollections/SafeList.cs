using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Monofoxe.Engine.Utils.CustomCollections
{
	/// <summary> 
	/// Safe list. Makes possible to safely remove from and add items to the list during enumeration.
	/// </summary>
	public class SafeList<T> : IEnumerable<T>
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
			_isOutdated = true;
			_items.Add(item);
		}

		public void Remove(T item)
		{
			_isOutdated = true;
			_items.Remove(item);
		}
		
		public bool Contains(T item) =>
			_items.Contains(item);
		
		public void Insert(int index, T item) =>
			_items.Insert(index, item);
		
		public void AddRange(SafeList<T> items) =>
			_items.AddRange(items);

		public int Count =>
			_items.Count;
		
		/// <summary>
		/// Clears out all items from the list.
		/// </summary>
		public void Clear()
		{
			_isOutdated = true;
			_items.Clear();
		}

		public T this[int index]
		{
			get => _items[index];
			set
			{
				_isOutdated = true;
				_items[index] = value;
			}
		}
		
		public List<T> ToList() =>
			new List<T>(_items);
		
		public T[] ToArray() =>
			_items.ToArray();
		
		
		public IOrderedEnumerable<T> OrdeByDescending<TKey>(Func<T, TKey> func) =>
			_items.OrderByDescending(func);

		public IOrderedEnumerable<T> OrdeBy<TKey>(Func<T, TKey> func) =>
			_items.OrderBy(func);
		

		/// <summary>
		/// Removes old elements from the list and adds new ones.
		/// </summary>
		private void Update()
		{
			if (_isOutdated)
			{
				_outdatedItems.Clear();
				_outdatedItems.AddRange(_items);
				
				_isOutdated = false;
			}
		}

		
		public IEnumerator<T> GetEnumerator()
		{
			Update();
			return _outdatedItems.GetEnumerator();
		}
		
		IEnumerator IEnumerable.GetEnumerator()
		{
			Update();
			return _outdatedItems.GetEnumerator();
		}
		
	}
}
