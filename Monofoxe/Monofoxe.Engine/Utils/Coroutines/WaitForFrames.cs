using System.Collections;

namespace Monofoxe.Engine.Utils.Coroutines
{
	/// <summary>
	/// Suspends the coroutine execution until a certain amount of frames passes.
	/// </summary>
	public class WaitForFrames : YieldInstruction
	{
		private double _waitCount;
		
		public WaitForFrames(int frames)
		{
			_waitCount = frames;
		}


		public override IEnumerator Run()
		{
			var counter = 0.0;

			counter += 1;

			while (counter < _waitCount)
			{
				yield return null;
				counter += 1;
			}
		}
	}
}
