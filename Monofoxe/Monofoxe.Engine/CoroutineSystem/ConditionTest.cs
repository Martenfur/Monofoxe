using System.Collections;

namespace Monofoxe.Engine.CoroutineSystem
{
	public class ConditionTest : YieldInstruction
	{
		public override IEnumerator Yield()
		{
			while (!Input.CheckButtonPress(Buttons.A))
			{
				yield return null;
			}
		}
	}
}
