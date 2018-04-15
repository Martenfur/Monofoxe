using System;

namespace Monofoxe.Utils
{
	/// <summary>
	/// Extends System.Random.
	/// </summary>
	public class RandomExt
	{
		Random Random;

		/// <summary>
		/// Returns a random number in range [0, Int32.MaxValue).
		/// </summary>
		public int Next() => Random.Next();
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
	}
}
