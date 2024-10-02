using Monofoxe.Engine.Abstractions;
using System;
using System.ComponentModel;
using System.Windows.Forms;

namespace Monofoxe.Engine.WindowsDX.Implementations
{
	internal class WindowsWindowWatch : IWindowWatch
	{
		public event Func<bool> OnClosing;

		public void Init(IntPtr handle)
		{
			var form = (Form)Form.FromHandle(handle);
			if (form != null)
			{
				// On DesktopGL, there is no form.
				// TODO: Add a way for DesktopGL to intercept the forms somehow.
				form.Closing += ClosingForm;
			}
		}

		public void ClosingForm(object sender, CancelEventArgs e) =>
			e.Cancel = (bool)(OnClosing?.Invoke());
	}
}
