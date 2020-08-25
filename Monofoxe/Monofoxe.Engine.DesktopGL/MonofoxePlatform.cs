
namespace Monofoxe.Engine.DesktopGl
{
  public static class MonofoxePlatform
	{
		public static void Init()
		{ 
			StuffResolver.AddStuffAs<IAlphaBlendEffectLoader>(new AlphaBlendEffectLoaderDesktopGl());
		}
	}
}
