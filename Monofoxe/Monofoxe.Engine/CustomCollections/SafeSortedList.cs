using System;
using System.Collections;
using System.Collections.Generic;

namespace Monofoxe.Egine.CustomCollections
{
	/// <summary>
	/// Safe sorted list. Old items are removes and new ones added only after Update is called. 
	/// This makes possible to safely remove and add items to the list.
	/// 
	/// NOTE: Sorting algorhitm is very basic and must be used only for small amounts (1-5) of new elements.
	/// DO NOT use this class for frequently updated collections with lots of elements.
	/// </summary>
	/// <typeparam name="T"></typeparam>
	public class SafeSortedList<T> : IEnumerable<T>
	{
		private Func<T, int> _sortingParameter;
		private List<T> _items;
		private List<T> _newItems;
		private List<T> _removedItems;


		public SafeSortedList(Func<T, int> sortingParameter)
		{
			_sortingParameter = sortingParameter;
			_items = new List<T>();
			_newItems = new List<T>();
			_removedItems = new List<T>();
		}


		public void Add(T obj) =>
			_newItems.Add(obj);

		public void Remove(T obj) =>
			_removedItems.Add(obj);
		
		public bool Contains(T obj) =>
			_items.Contains(obj) && !_removedItems.Contains(obj);
		
		
		public List<T> ToList() =>
			new List<T>(_items);
		
		public T[] ToArray() =>
			_items.ToArray();
		
		
		public void Update()
		{
			// Removing old items.
			foreach(var item in _removedItems)
			{
				_items.Remove(item);
			}
			// Removing old items.

			// Adding new items.
			foreach(var item in _newItems)
			{
				var added = false;
				for(var i = 0; i < _items.Count; i += 1)
				{
					if (_sortingParameter(item) > _sortingParameter(_items[i]))
					{
						_items.Insert(i, item);
						added = true;
						break;
					}
				}
				if (!added)
				{
					_items.Add(item); // Adding an item at the end, if it has lowest priority.
				}
			}
			// Adding new items.
		}


		public IEnumerator<T> GetEnumerator() =>
			((IEnumerable<T>)_items).GetEnumerator();
		
		IEnumerator IEnumerable.GetEnumerator() =>
			((IEnumerable<T>)_items).GetEnumerator();
	}
}
