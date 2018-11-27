using System;
using System.Collections;
using System.Collections.Generic;

namespace Monofoxe.Engine.CustomCollections
{
	/// <summary> 
	/// Safe sorted list. Makes possible to safely remove from and add items to the list during foreach.
	/// 
	/// NOTE: Sorting algorhitm is very basic and must be used only for small amounts (1-5) of new elements.
	/// DO NOT use this class for frequently updated collections with lots of elements.
	/// It also does not resort list every update, so be careful with changing item's sorting parameter on the fly.
	/// Good idea will be to re-add item back to the list.
	/// </summary>
	public class SafeSortedList<T> : IEnumerable<T>
	{
		private Func<T, int> _sortingParameter;
		private List<T> _items, _outdatedItems;
		private bool _isOutdated = false;

		public SafeSortedList(Func<T, int> sortingParameter)
		{
			_sortingParameter = sortingParameter;
			_items = new List<T>();
			_outdatedItems = new List<T>();
		}


		public void Add(T item)
		{
			_isOutdated = true;
			
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

		public void Remove(T item)
		{
			_isOutdated = true;
			_items.Remove(item);
		}
		
		public bool Contains(T item) =>
			_items.Contains(item);
		
		public int Count =>
			_items.Count;
		
		/// <summary>
		/// Clears out all current and new items from the list.
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
			return ((IEnumerable<T>)_outdatedItems).GetEnumerator();
		}
		
		IEnumerator IEnumerable.GetEnumerator()
		{
			Update();
			return ((IEnumerable<T>)_outdatedItems).GetEnumerator();
		}
	}
}
