using System;
using System.Collections;
using System.Collections.Generic;

namespace Monofoxe.Engine.Utils.CustomCollections
{
	/// <summary>
	/// Operates like a regular list but the order of items is not guaranteed to stay the same. 
	/// As a tradeoff, it is way faster to remove items form this list. 
	/// Use it when you don't care about the order of items but want it to be as fast as possible.
	/// </summary>
	public class UnorderedList<T> : IEnumerable<T>, IEnumerable
	{
		private T[] _items;
		private int _count = 0;

		public UnorderedList(int capacity = 1)
		{
			if (capacity <= 0)
			{
				throw new ArgumentOutOfRangeException(nameof(capacity));
			}

			_items = new T[capacity];
		}


		public T this[int index]
		{
			get
			{
				if (index >= _count)
				{
					throw new ArgumentOutOfRangeException(nameof(index));
				}
				return _items[index];
			}
			set
			{
				if (index >= _count)
				{
					throw new ArgumentOutOfRangeException(nameof(index));
				}
				_items[index] = value;
			}
		}


		public int Count => _count;


		public void Add(T item)
		{
			_count += 1;
			if (_count >= _items.Length)
			{
				Array.Resize(ref _items, _items.Length * 2);
			}
			_items[_count - 1] = item;
		}


		public void Clear()
		{
			_count = 0;
		}


		public bool Contains(T item)
		{
			throw new NotImplementedException();
		}


		public bool Remove(T item)
		{
			throw new NotImplementedException();
		}


		public void RemoveAt(int index)
		{
			if (index < 0 || index >= _count)
			{
				throw new ArgumentOutOfRangeException(nameof(index));
			}

			_count -= 1;
			if (index == _count)
			{
				return;
			}

			// This changes the order of items but is way faster.
			_items[index] = _items[_count];
		}


		public void RemoveLast()
		{
			if (_count > 0)
			{
				_count -= 1;
			}
		}


		public IEnumerator<T> GetEnumerator()
		{
			throw new NotImplementedException();
		}


		IEnumerator IEnumerable.GetEnumerator()
		{
			throw new NotImplementedException();
		}
	}
}
