
namespace Monofoxe.Engine.Abstractions
{
	/// <summary>
	/// Interface for accessing system clipboard.
	/// </summary>
	public interface IClipboard
	{
		void Copy(string text);

		string Paste();
	}
}
