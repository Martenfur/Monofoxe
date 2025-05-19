
namespace Monofoxe.Engine.Abstractions
{
	/// <summary>
	/// Interface for accessing the current monitor's refresh rate.
	/// </summary>
	public interface IMonitorFrequencyManager
	{
		int GetCurrentMonitorFrequency();
	}
}
