using Monofoxe.Engine.Utils;
using System.Collections;

namespace Monofoxe.Engine.CoroutineSystem
{
	public class WaitForSeconds : YieldInstruction
	{
		private double _waitTime;
		private TimeKeeper _keeper = TimeKeeper.Global;

		public WaitForSeconds(double seconds)
		{
			_waitTime = seconds;
		}


		public WaitForSeconds(double seconds, TimeKeeper keeper)
		{
			_waitTime = seconds;
			_keeper = keeper;
		}

		public override IEnumerator Yield()
		{
			var counter = 0.0;

			counter += _keeper.Time();

			while (counter < _waitTime)
			{
				yield return null;
				counter += _keeper.Time();
			}
		}
	}
}
