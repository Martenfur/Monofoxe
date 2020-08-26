
namespace Monofoxe.Engine.Utils
{
	public delegate void AlarmDelegate(Alarm caller);

	public enum OnTriggerAction
	{
		/// <summary>
		/// On reaching trigger time, 
		/// alarm resets itself to 0, triggers OnTrigger event and stops.
		/// </summary>
		Stop,

		/// <summary>
		/// On reaching trigger time,
		/// alarm resets itself to 0, triggers OnTrigger event and continues counting.
		/// In this mode, alarm takes into account leftover counter delta to make 
		/// repeated counting precise.
		/// </summary>
		Loop,

		/// <summary>
		/// Alarm never triggers.
		/// </summary>
		None,
	}

	/// <summary>
	/// Counts down seconds. Needs to be updated manually.
	/// </summary>
	public class Alarm
	{
		/// <summary>
		/// Tells how much time has passed in seconds.
		/// </summary>
		public double Counter { get; private set; } = 0;

		/// <summary>
		/// If alarm's counter reaches this value and it's not in
		/// OnTriggerAction.None mode, alarm will call OnTrigger event.
		/// </summary>
		public double TriggerTime;
		
		/// <summary>
		/// Alarm won't update if it's paused.
		/// </summary>
		public bool Paused = false;

		public TimeKeeper TimeKeeper = TimeKeeper.Global;
		
		/// <summary>
		/// Gets called if the alarm is triggered. 
		/// </summary>
		public event AlarmDelegate TriggerEvent;

		/// <summary>
		/// Tells, if alarm is running.
		/// </summary>
		public bool Running {get; private set;}
		
		public OnTriggerAction OnTriggerAction;


		public Alarm(double time, OnTriggerAction onTriggerAction = OnTriggerAction.Stop) 
		{
			TriggerTime = time;
			OnTriggerAction = onTriggerAction;
		}
		
		
		public void Start()
		{
			Running = true;
			Counter = 0;
		}
		
		
		public void Stop()
		{
			Running = false;
			Counter = 0;
		}


		/// <summary>
		/// Updates alarm. Returns true, if alarm was triggered.
		/// </summary>
		public bool Update()
		{
			if (!Paused && Running)
			{
				Counter += TimeKeeper.Time();		
				
				if (
					OnTriggerAction != OnTriggerAction.None 
					&& Counter >= TriggerTime 
				)
				{
					if (OnTriggerAction == OnTriggerAction.Stop)
					{
						Running = false;
						Counter = 0;
					}
					if (OnTriggerAction == OnTriggerAction.Loop)
					{
						Counter -= TriggerTime; // Necessary for correct timing. 
					}
					TriggerEvent?.Invoke(this);
					return true;
				}
			}

			return false;
		}
	}
}
