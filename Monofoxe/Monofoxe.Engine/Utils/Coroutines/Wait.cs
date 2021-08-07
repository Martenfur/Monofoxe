using Monofoxe.Engine.SceneSystem;
using System;
using System.Collections;

namespace Monofoxe.Engine.Utils.Coroutines
{
	/// <summary>
	/// Suspends the coroutine execution until a certain amount of seconds passes.
	/// </summary>
	public static class Wait
	{
		/// <summary>
		/// Suspends the coroutine execution until a certain amount of seconds passes.
		/// </summary>
		public static IEnumerator ForSeconds(double seconds) =>
			ForSeconds(seconds, TimeKeeper.Global);

		/// <summary>
		/// Suspends the coroutine execution until a certain amount of seconds passes.
		/// </summary>
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


		/// <summary>
		/// Suspends the coroutine execution until a certain amount of frames passes.
		/// </summary>
		public static IEnumerator ForFrames(int frames)
		{
			var counter = 0.0;

			counter += 1;

			while (counter < frames)
			{
				yield return null;
				counter += 1;
			}
		}


		/// <summary>
		/// Suspends the coroutine execution until the fixed update is hit.
		/// IMPORTANT: TimeKeeper.Time() WILL NOT be equal to GameMgr.FixedUpdateRate.
		/// </summary>
		public static IEnumerator ForFixedUpdate()
		{
			while (!SceneMgr.IsFixedUpdateFrame)
			{
				yield return null;
			}
		}


		/// <summary>
		/// Suspends the coroutine execution until the supplied delegate evaluates to false.
		/// </summary>
		public static IEnumerator Until(Func<bool> action)
		{
			while (!action())
			{
				yield return null;
			}
		}


		/// <summary>
		/// Suspends the coroutine execution until the supplied delegate evaluates to true.
		/// </summary>
		public static IEnumerator While(Func<bool> action)
		{
			while (action())
			{
				yield return null;
			}
		}
	}
}
