using System.Collections;

namespace Monofoxe.Engine.Utils.Coroutines
{
	/// <summary>
	/// Suspends the coroutine execution until a certain amount of seconds passes.
	/// </summary>
	public static class Wait
	{
		public static IEnumerator ForSeconds(double seconds) =>
			ForSeconds(seconds, TimeKeeper.Global);

		public static IEnumerator ForSeconds(double seconds, TimeKeeper keeper)
		{
			var counter = 0.0;

			counter += keeper.Time();

			while (counter < seconds)
			{
				yield return null;
				counter += keeper.Time();
			}
		}
	}
}
