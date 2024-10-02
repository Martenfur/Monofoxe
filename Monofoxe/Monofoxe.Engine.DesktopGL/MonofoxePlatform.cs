using Monofoxe.Engine.Abstractions;
using Monofoxe.Engine.DesktopGL.Implementations;

namespace Monofoxe.Engine.DesktopGL
{
	public static class MonofoxePlatform
	{
		public static void Init()
		{
			// New OpenGL platform has a bunch of Windows-specific implementations.
			// If you want Linux or Mac OS support you'll have to reimplement/plug up interfaces in Abstractions.
			GameMgr.CurrentPlatform = Platform.Windows;
			/*
			if (Environment.OSVersion.Platform == PlatformID.Win32NT)
			{
				GameMgr.CurrentPlatform = Platform.Windows;
			}
			else if (Environment.OSVersion.Platform == PlatformID.Unix)
			{
				GameMgr.CurrentPlatform = Platform.Linux;
			}
			else if (Environment.OSVersion.Platform == PlatformID.MacOSX)
			{
				GameMgr.CurrentPlatform = Platform.MacOS;
			}
			else
			{
				GameMgr.CurrentPlatform = Platform.Other;
			}*/

			GameMgr.CurrentGraphicsBackend = GraphicsBackend.OpenGL;

			StuffResolver.AddStuffAs<IAlphaBlendEffectLoader>(new AlphaBlendEffectLoaderDesktopGl());
			StuffResolver.AddStuffAs<ITextInputBinder>(new WindowsTextInputBinder());
			StuffResolver.AddStuffAs<IClipboard>(new WindowsClipboard());
			StuffResolver.AddStuffAs<ILocalStorage>(new WindowsLocalStorage());
			StuffResolver.AddStuffAs<IWindowWatch>(new WindowsWindowWatch());
			StuffResolver.AddStuffAs<IScalingManager>(new WindowsScalingManager());
			StuffResolver.AddStuffAs<IMonitorFrequencyManager>(new WindowsMonitorFrequencyManager());
		}
	}
}
