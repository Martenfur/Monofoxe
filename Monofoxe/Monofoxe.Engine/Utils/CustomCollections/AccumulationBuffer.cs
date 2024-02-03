using System;

namespace Monofoxe.Engine.Utils.CustomCollections
{
	public class AccumulationBuffer<T> where T : struct
	{
		private T[] buffer;


		public int Length { get; private set; }


		public AccumulationBuffer(int capacity = 32)
		{
			buffer = new T[capacity];
		}

		public ref T this[int i] => ref buffer[i];


		public void Add(T obj)
		{
			if (Length >= buffer.Length)
			{
				Array.Resize(ref buffer, buffer.Length * 2);
			}
			buffer[Length] = obj;
			Length += 1;
		}


		public void Clear()
		{
			Length = 0;
		}
	}
}
