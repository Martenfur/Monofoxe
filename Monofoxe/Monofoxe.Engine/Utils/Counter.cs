
namespace Monofoxe.Engine.Utils
{
	public delegate void CounterDelegate(Counter caller);

	/// <summary>
	/// Counts frames. Needs to be updated manually.
	/// </summary>
	public class Counter
	{
		/// <summary>
		/// Counter value.
		/// </summary>
		public int Count { get; private set; } = 0;

		/// <summary>
		/// If counters's count reaches this value and it's not in
		/// OnTriggerAction.None mode, it will call OnTrigger event.
		/// </summary>
		public int TriggerCount;
		
		/// <summary>
		/// Counter won't update if it's paused.
		/// </summary>
		public bool Paused = false;

		/// <summary>
		/// Gets called if the counter is triggered. 
		/// </summary>
		public event CounterDelegate TriggerEvent;

		/// <summary>
		/// Tells, if the counter is running.
		/// </summary>
		public bool Running {get; private set;}
		

		public OnTriggerAction OnTriggerAction;


		public Counter(int count, OnTriggerAction onTriggerAction = OnTriggerAction.Stop, bool started = false) 
		{
			TriggerCount = count;
			OnTriggerAction = onTriggerAction;
			if (started)
			{
				Start();
			}
		}


		public void Start(int count)
		{
			TriggerCount = count;
			Start();
		}


		public void Start()
		{
			Running = true;
			Count = 0;
		}
		
		
		public void Stop()
		{
			Running = false;
			Count = 0;
		}


		public void Pause() =>
			Paused = true;


		public void Unpause() =>
			Paused = false;


		/// <summary>
		/// Adds 1 to the counter and looks for trigger events. Returns true, if counter was triggered.
		/// </summary>
		public bool Update()
		{
			if (!Paused && Running)
			{
				Count += 1;	
				
				if (
					OnTriggerAction != OnTriggerAction.None 
					&& Count >= TriggerCount 
				)
				{
					if (OnTriggerAction == OnTriggerAction.Stop)
					{
						Running = false;
					}
					Count = 0;
					TriggerEvent?.Invoke(this);
					return true;
				}
			}

			return false;
		}
	}
}
