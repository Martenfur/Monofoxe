using Monofoxe.Engine.Abstractions;
using Monofoxe.Engine.WindowsDX.Implementations;

namespace Monofoxe.Engine.WindowsDX
{
	public static class MonofoxePlatform
	{
		public static void Init()
		{
			GameMgr.CurrentPlatform = Platform.Windows;
			GameMgr.CurrentGraphicsBackend = GraphicsBackend.DirectX;

			StuffResolver.AddStuffAs<IAlphaBlendEffectLoader>(new AlphaBlendEffectLoaderWindowsDX());
			StuffResolver.AddStuffAs<ITextInputBinder>(new WindowsTextInputBinder());
			StuffResolver.AddStuffAs<IClipboard>(new WindowsClipboard());
			StuffResolver.AddStuffAs<ILocalStorage>(new WindowsLocalStorage());
			StuffResolver.AddStuffAs<IWindowWatch>(new WindowsWindowWatch());
			StuffResolver.AddStuffAs<IScalingManager>(new WindowsScalingManager());
			StuffResolver.AddStuffAs<IMonitorFrequencyManager>(new WindowsMonitorFrequencyManager());
		}
	}
}
