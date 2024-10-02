using Monofoxe.Engine.Abstractions;
using System.Windows.Forms;

namespace Monofoxe.Engine.WindowsGL.Implementations
{
	internal class WindowsClipboard : IClipboard
	{
		public void Copy(string text) =>
			Clipboard.SetText(text);

		public string Paste() =>
			Clipboard.GetText();

	}
}
