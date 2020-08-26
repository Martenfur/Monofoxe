
namespace Monofoxe.Engine.DesktopGL
{
	public class TextInputBinderDesktopGL : ITextInputBinder
	{
		public void Init()
		{
			GameMgr.Game.Window.TextInput += Input.TextInput;
		}
	}
}
