using Monofoxe.Engine.SceneSystem;
using System.Collections;

namespace Monofoxe.Engine.Utils.Coroutines
{
  /// <summary>
  /// Suspends the coroutine execution until the fixed update is hit.
  /// IMPORTANT: TimeKeeper.Time() WILL NOT be equal to GameMgr.FixedUpdateRate.
  /// </summary>
  public class WaitForFixedUpdate : YieldInstruction
	{
		public override IEnumerator Run()
		{
			while (!SceneMgr.IsFixedUpdateFrame)
			{
				yield return null;
			}
		}
	}
}
