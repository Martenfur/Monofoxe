using System;
using System.Collections;
using System.Collections.Generic;

namespace Monofoxe.Engine.CustomCollections
{
	/// <summary>
	/// Safe sorted list. Old items are removed and new ones added only after Update is called. 
	/// This makes possible to safely remove from and add items to the list during for\foreach.
	/// 
	/// NOTE: Sorting algorhitm is very basic and must be used only for small amounts (1-5) of new elements.
	/// DO NOT use this class for frequently updated collections with lots of elements.
	/// It also does not resort list every update, so be careful with changing item's sorting parameter on the fly.
	/// Good idea will be to re-add item back to the list.
	/// </summary>
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
			_items.Contains(obj);
		
		public int Count =>
			_items.Count;
		
		/// <summary>
		/// Clears out all current and new items from the list.
		/// </summary>
		public void Clear()
		{
			_newItems.Clear();
			_removedItems.AddRange(_items);
		}

		public T this[int index]
		{
			get => _items[index];
			set => _items[index] = value;
		}
		
		public List<T> ToList() =>
			new List<T>(_items);
		
		public T[] ToArray() =>
			_items.ToArray();
		
		
		/// <summary>
		/// Removes old elements from the list and adds new ones.
		/// </summary>
		public void Update()
		{
			// Removing old items.
			foreach(var item in _removedItems)
			{
				_items.Remove(item);
			}
			_removedItems.Clear();
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
			_newItems.Clear();
			// Adding new items.
		}


		public IEnumerator<T> GetEnumerator() =>
			((IEnumerable<T>)_items).GetEnumerator();
		
		IEnumerator IEnumerable.GetEnumerator() =>
			((IEnumerable<T>)_items).GetEnumerator();
	}
}
