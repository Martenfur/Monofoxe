using System;
using System.Collections.Generic;

namespace Monofoxe.Engine.Utils
{
	/// <summary>
	/// Extends System.Random.
	/// </summary>
	public class RandomExt
	{
		private Random Random;
		public readonly int Seed;

		/// <summary>
		/// Creates random generator with system time used as a seed.
		/// </summary>
		public RandomExt()
		{
			// System.Random only accepts int as a seed. This kinda sucks. 
			// DateTime.Now overflows int 700+ times, so we have to store it as a double.
			double time = DateTime.Now.Subtract(new DateTime(1970, 1, 1, 0, 0, 0)).TotalMilliseconds; 

			// We need a conversion to int here to remove fraction, but still need double to prevent overflow.
			double overflowsCount = (int)(time / int.MaxValue); 

			time -= int.MaxValue * overflowsCount; // Removing a chunk of a number so int can actually handle it.
			Seed = (int)time;

			Random = new Random(Seed);
		}

		/// <summary>
		/// Creates random number generator with a specific seed.
		/// </summary>
		public RandomExt(int seed)
		{
			Random = new Random(seed);
			Seed = seed;
		}



		/// <summary>
		/// Returns a random number in range [0, Int32.MaxValue).
		/// </summary>
		public int Next() => 
			Random.Next();

		/// <summary>
		/// Returns a random number in range [0, maxValue).
		/// </summary>
		public int Next(int maxValue) => 
			Random.Next(maxValue);

		/// <summary>
		/// Returns a random number in range [minValue, maxValue).
		/// </summary>
		public int Next(int minValue, int maxValue) => 
			Random.Next(minValue, maxValue);



		/// <summary>
		/// Returns a random number in range [0.0, 1.0).
		/// </summary>
		public double NextDouble() => 
			Random.NextDouble();

		/// <summary>
		/// Returns a random number in range [0.0, maxValue).
		/// </summary>
		public double NextDouble(double maxValue) => 
			Random.NextDouble() * maxValue;
		
		/// <summary>
		/// Returns a random number in range [minValue, maxValue).
		/// </summary>
		public double NextDouble(double minValue, double maxValue) => 
			minValue + Random.NextDouble() * (maxValue - minValue);
		


		/// <summary>
		/// Fills the elements of a specified array of bytes with random numbers.
		/// </summary>
		public void NextBytes(byte[] buffer) => 
			Random.NextBytes(buffer);
		


		/// <summary>
		/// Returns a random element out of given arguments.
		/// </summary>
		public T Choose<T>(params T[] args) =>
			args[Random.Next(args.Length)];


		/// <summary>
		/// Returns list of numbers from minValue to maxValue in random order.
		/// </summary>
		public List<int> GetListWithoutRepeats(int listSize, int minValue, int maxValue)
		{
			int delta = maxValue - minValue;

			if (delta < listSize)
			{
				throw new Exception("List size is bigger than value delta!");
			}

			var sequenceList = new List<int>();
			for(var i = 0; i < delta; i += 1)
			{
				sequenceList.Add(minValue + i);
			}

			var list = new List<int>();

			for(var i = 0; i < listSize; i += 1)
			{
				var index = Random.Next(sequenceList.Count);
				list.Add(sequenceList[index]);
				sequenceList.RemoveAt(index);
			}
			
			return list;
		}
	}
}
