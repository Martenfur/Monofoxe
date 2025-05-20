/////////////////////////////////////////////////////////////////////////////////////////////
/////////////////////////////////////////////////////////////////////////////////////////////
/// Original Unity version made by Ivan Pensionerov https://github.com/gasgiant/Camera-Shake
/// Ported and improved by Minkberry.
/////////////////////////////////////////////////////////////////////////////////////////////

using Monofoxe.Engine.Shake.Utils;
using Microsoft.Xna.Framework;

namespace Monofoxe.Engine.Shake.Presets
{
	public abstract class ShakePreset
	{
		/// <summary>
		/// If true, the amount of shake will depend on distance from camera.
		/// </summary>
		public bool UsesAttenuation = false; 

		/// <summary>
		/// Source position of the shake relative to camera. 
		/// NOTE: This only takes effect if UsesAttenuation is true.
		/// </summary>
		public Vector2 SourcePosition = Vector2.Zero;

		/// <summary>
		/// Attenuation settings. Govern how ths shake will behave relative to camera position.
		/// NOTE: This only takes effect if UsesAttenuation is true.
		/// </summary>
		public Attenuation Attenuation = new Attenuation(100);

		public abstract IShake CreateShake();
	}
}
