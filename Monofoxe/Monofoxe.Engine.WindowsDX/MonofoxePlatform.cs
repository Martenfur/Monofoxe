namespace Monofoxe.Engine.WindowsDX
{
  public static class MonofoxePlatform
	{
		public static void Init()
		{ 
			StuffResolver.AddStuffAs<IAlphaBlendEffectLoader>(new AlphaBlendEffectLoaderWindowsDX());
		}
	}
}
