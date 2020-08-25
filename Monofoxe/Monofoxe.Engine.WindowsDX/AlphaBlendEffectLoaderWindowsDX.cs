using System.Reflection;

namespace Monofoxe.Engine.WindowsDX
{
	public class AlphaBlendEffectLoaderWindowsDX : AlphaBlendEffectLoader
	{
		protected override string _effectName => "Monofoxe.Engine.WindowsDX.AlphaBlend_dx.mgfxo";

		protected override Assembly _assembly => Assembly.GetAssembly(typeof(AlphaBlendEffectLoaderWindowsDX));
	}
}
