using System.Collections;

namespace Monofoxe.Engine.Utils.Coroutines
{
	public class WaitForButton : YieldInstruction
	{
		private Buttons _button;

		public WaitForButton(Buttons button)
		{
			_button = button;
		}


		public override IEnumerator Run()
		{
			while (!Input.CheckButtonPress(_button))
			{
				yield return null;
			}
		}
	}
}
