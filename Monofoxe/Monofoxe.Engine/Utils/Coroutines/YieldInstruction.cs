using System.Collections;

namespace Monofoxe.Engine.Utils.Coroutines
{
	/// <summary>
	/// Base class for yield instructions used for coroutines.
	/// Derive from it if you want your own WaitFor instructions.
	/// </summary>
	public abstract class YieldInstruction
	{
		/// <summary>
		/// This method runs within a coroutine when the YieldInstruction 
		/// is being returned with yield return. The coroutine will pause until 
		/// this method finishes.
		/// </summary>
		public abstract IEnumerator Run();
	}
}
