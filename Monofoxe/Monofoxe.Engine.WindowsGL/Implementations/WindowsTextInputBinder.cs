namespace Monofoxe.Engine.WindowsGL.Implementations
{
	public class WindowsTextInputBinder : ITextInputBinder
	{
		public void Init()
		{
			GameMgr.Game.Window.TextInput += Input.TextInput;
		}
	}
}
