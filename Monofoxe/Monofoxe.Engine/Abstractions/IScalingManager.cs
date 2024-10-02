using System;

namespace Monofoxe.Engine.Abstractions
{
	/// <summary>
	/// Gets the current monitor's dpi and system scaling based on it.
	/// </summary>
	public interface IScalingManager
	{
		/// <summary>
		/// Minimum supported scale for the current platform. Usually 1.
		/// </summary>
		int MinSupportedScale { get; }

		/// <summary>
		/// Maximum supported scale for the current platform. Usually 2.
		/// </summary>
		int MaxSupportedScale { get; }

		/// <summary>
		/// Gets global OS scale.
		/// </summary>
		float GetScale();

		/// <summary>
		/// Gets the monitor's dpi. Base dpi is 96.
		/// </summary>
		float GetDpi();

		/// <summary>
		/// Refreshes dpi cache.
		/// </summary>
		void Refresh();

		/// <summary>
		/// After this method is called, GetScale() reports provided value instead of an actual one.
		/// </summary>
		void OverrideScale(float scale);

		/// <summary>
		/// Resets scale override if any is set.
		/// </summary>
		void ResetScaleOverride();

		/// <summary>
		/// Raised whenever scale gets changed.
		/// </summary>
		event Action<float> OnScaleChanged;
	}
}
