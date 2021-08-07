using System.Collections;

namespace Monofoxe.Engine.CoroutineSystem
{
  public abstract class YieldInstruction
	{
		public abstract IEnumerator Yield(); // TODO: Rename.
	}
}
