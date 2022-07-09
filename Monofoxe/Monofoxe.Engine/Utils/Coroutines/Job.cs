using System.Diagnostics;

namespace Monofoxe.Engine.Utils.Coroutines
{
	/// <summary>
	/// Jobs are similar to coroutines, but instead of scheduling execution
	/// to the next frame every time, they have a time budget. 
	/// If there is enough time left, yield return null will immediately return
	/// back the the job in the same frame.
	/// Jobs are well-suited for large tasks that should happen in the background, like pathfinding, map loading, etc.
	/// NOTE: DO NOT use Wait methods, they are incompatible with jobs!!!
	/// </summary>
	public class Job : Coroutine
	{
		/// <summary>
		/// The amount of time after which the job force pauses and skips to the next frame.
		/// </summary>
		public float MillisecondBudget;

		private Stopwatch _watch = new Stopwatch();


		public Job(float millisecondBudget = 0.1f)
		{
			MillisecondBudget = millisecondBudget;
		}

		
		internal override bool Update()
		{
			var tickBudget = (long)(Stopwatch.Frequency * MillisecondBudget / 1000f);
			var workComplete = false;

			_watch.Restart();
			while ((!workComplete) && (_watch.ElapsedTicks < tickBudget))
			{
				workComplete = !base.Update();
			}
			_watch.Stop();

			return !workComplete;
		}
	}
}
