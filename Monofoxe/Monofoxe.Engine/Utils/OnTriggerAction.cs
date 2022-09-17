
namespace Monofoxe.Engine.Utils
{
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
}
