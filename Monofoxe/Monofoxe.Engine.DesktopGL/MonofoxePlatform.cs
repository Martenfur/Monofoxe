using System;

namespace Monofoxe.Engine.DesktopGL
{
	public static class MonofoxePlatform
	{
		public static void Init()
		{
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
			}

			GameMgr.CurrentGraphicsBackend = GraphicsBackend.OpenGL;

			StuffResolver.AddStuffAs<IAlphaBlendEffectLoader>(new AlphaBlendEffectLoaderDesktopGl());
			StuffResolver.AddStuffAs<ITextInputBinder>(new TextInputBinderDesktopGL());
		}
	}
}
