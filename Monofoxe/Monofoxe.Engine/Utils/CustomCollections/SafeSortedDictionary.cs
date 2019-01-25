using System;
using System.Collections;
using System.Collections.Generic;

namespace Monofoxe.Engine.Utils.CustomCollections
{
	/// <summary> 
	/// Safe sorted dictionary. Makes possible to safely remove from and add items to the list during foreach.
	/// 
	/// NOTE: Sorting algorhitm is very basic and must be used only for small amounts (1-5) of new elements.
	/// DO NOT use this class for frequently updated collections with lots of elements.
	/// It also does not resort whole list every update, so be careful with changing item's sorting parameter on the fly.
	/// Good idea will be to re-add item back to the list.
	/// </summary>
	public class SafeSortedDictionary<TKey, TValue> : IEnumerable<TValue>
	{
		private Dictionary<TKey, TValue> _items;
		private SafeSortedList<TValue> _sortedItems; 	

		public SafeSortedDictionary(Func<TValue, int> sortingParameter)
		{
			_items = new Dictionary<TKey, TValue>();
			_sortedItems = new SafeSortedList<TValue>(sortingParameter);
		}

		public void Add(TKey key, TValue value)
		{
			_items.Add(key, value);
			_sortedItems.Add(value);
		}

		public void Remove(TKey key)
		{
			TValue item;
			if (_items.TryGetValue(key, out item))
			{
				_items.Remove(key);
				_sortedItems.Remove(item);
			}
		}
		
		public bool ContainsKey(TKey key) =>
			_items.ContainsKey(key);

		public bool ContainsValue(TValue value) =>
			_items.ContainsValue(value);
		
		public bool TryGetValue(TKey key, out TValue value) =>
			_items.TryGetValue(key, out value);

		public TValue this[TKey key]
		{
			get => _items[key];
			set => _items[key] = value;
		}


		IEnumerator<TValue> IEnumerable<TValue>.GetEnumerator() =>
			_sortedItems.GetEnumerator();

		IEnumerator IEnumerable.GetEnumerator() =>
			_sortedItems.GetEnumerator();
	}
}
