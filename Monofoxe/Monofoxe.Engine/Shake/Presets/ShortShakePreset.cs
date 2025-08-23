/////////////////////////////////////////////////////////////////////////////////////////////
/// Original Unity version made by Ivan Pensionerov https://github.com/gasgiant/Camera-Shake
/// Ported and improved by Minkberry.
/////////////////////////////////////////////////////////////////////////////////////////////
using Microsoft.Xna.Framework;

namespace Monofoxe.Engine.Shake.Presets
{
	/// <summary>
	/// Preset for bounce shake.
	/// Suitable for short and snappy shakes. Moves camera in X and Y axes and rotates it in Z axis. 
	/// </summary>
	public class ShortShakePreset : ShakePreset
	{
		public float PositionStrength = 10f;
		public float RotationStrength = 0.02f;
		
		public float Frequency = 25;

		/// <summary>
		/// Number of vibrations before stop.
		/// </summary>
		public int BounceCount = 5;

		public override IShake CreateShake()
		{
			var pars = new BounceShake.Params
			{
				PositionStrength = PositionStrength,
				RotationStrength = RotationStrength,
				Frequence = Frequency,
				NumBounces = BounceCount,
				Attenuation = Attenuation,
			};

			return new BounceShake(pars, UsesAttenuation ? (Vector2?)SourcePosition : null);
		}
	}
}
