using System.Reflection;

namespace Monofoxe.Engine.WindowsGL
{
	public class AlphaBlendEffectLoaderWindowsGL: AlphaBlendEffectLoader
	{
		protected override string _effectName => "Monofoxe.Engine.WindowsGL.AlphaBlend_gl.mgfxo";

		protected override Assembly _assembly => Assembly.GetAssembly(typeof(AlphaBlendEffectLoaderWindowsGL));

	}
}
