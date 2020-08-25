using Monofoxe.Engine.Drawing;

namespace Monofoxe.Engine.DesktopGl
{
  public static class MonofoxePlatform
	{
		public static void Init()
		{ 
			StuffResolver.AddStuffAs<IAlphaBlendEffectLoader>(new AlphaBlendEffectLoaderDesktopGl());
			
			// -0.5 pixel offset because OpenGL is a snowflake.
			VertexBatch.UsesHalfPixelOffset = true;
		}
	}
}
