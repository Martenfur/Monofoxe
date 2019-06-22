namespace Monofoxe.Engine
{
	/// <summary>
	/// Extends basic C# number types. 
	/// </summary>
	public static class NumberExtensions
	{
		/// <summary>
		/// Returns true, if number is not zero.
		/// </summary>
		public static bool ToBool(this byte num) =>
			(num != 0);
		
		/// <summary>
		/// Returns true, if number is not zero.
		/// </summary>
		public static bool ToBool(this sbyte num) =>
			(num != 0);
		
		/// <summary>
		/// Returns true, if number is not zero.
		/// </summary>
		public static bool ToBool(this short num) =>
			(num != 0);
		
		/// <summary>
		/// Returns true, if number is not zero.
		/// </summary>
		public static bool ToBool(this ushort num) =>
			(num != 0);
		
		/// <summary>
		/// Returns true, if number is not zero.
		/// </summary>
		public static bool ToBool(this int num) =>
			(num != 0);
		
		/// <summary>
		/// Returns true, if number is not zero.
		/// </summary>
		public static bool ToBool(this uint num) =>
			(num != 0);
		
		/// <summary>
		/// Returns true, if number is not zero.
		/// </summary>
		public static bool ToBool(this long num) =>
			(num != 0);
		
		/// <summary>
		/// Returns true, if number is not zero.
		/// </summary>
		public static bool ToBool(this ulong num) =>
			(num != 0);

		/// <summary>
		/// Returns true, if number is not zero.
		/// </summary>
		public static bool ToBool(this float num) =>
			(num != 0);
		
		/// <summary>
		/// Returns true, if number is not zero.
		/// </summary>
		public static bool ToBool(this double num) =>
			(num != 0);
		
		/// <summary>
		/// Returns true, if number is not zero.
		/// </summary>
		public static bool ToBool(this decimal num) =>
			(num != 0);
		
		
		
		/// <summary>
		/// If true, returns one, otherwise - zero.
		/// </summary>
		public static int ToInt(this bool b)
		{
			if (b)
			{
				return 1;
			}
			return 0;
		}
		

	}
}
