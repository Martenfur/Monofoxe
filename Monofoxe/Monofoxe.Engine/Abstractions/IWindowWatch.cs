using System;

namespace Monofoxe.Engine.Abstractions
{
	/// <summary>
	/// Monitors system window's Close event.
	/// </summary>
	public interface IWindowWatch
	{
		event Func<bool> OnClosing;
		void Init(IntPtr handler);
	}
}
