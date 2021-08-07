using System;
using System.Collections;

namespace Monofoxe.Engine.Utils.Coroutines
{
	/// <summary>
	/// Suspends the coroutine execution until the supplied delegate evaluates to true.
	/// </summary>
	public class WaitUntil : YieldInstruction
	{
		private Func<bool> _action;

		public WaitUntil(Func<bool> action)
		{
			_action = action;
		}


		public override IEnumerator Run()
		{
			while (!_action())
			{
				yield return null;
			}
		}
	}
}
