/////////////////////////////////////////////////////////////////////////////////////////////
/// Original Unity version made by Ivan Pensionerov https://github.com/gasgiant/Camera-Shake
/// Ported and improved by Minkberry.
/////////////////////////////////////////////////////////////////////////////////////////////
using Monofoxe.Engine.Shake.Utils;
using Microsoft.Xna.Framework;

namespace Monofoxe.Engine.Shake.Presets
{
	/// <summary>
	/// Preset for perlin shake.
	/// Suitable for longer and stronger shakes. Moves camera in X and Y axes and rotates it in Z axis.
	/// </summary>
	public class ExplosionShakePreset : ShakePreset
	{
		public float PositionStrength = 10f;
		public float RotationStrength = 0.02f;
		public float Duration = 0.5f;

		public PerlinShake.NoiseMode[] NoiseModes =
		{
			new PerlinShake.NoiseMode(8, 1),
			new PerlinShake.NoiseMode(20, 0.3f)
		};


		public override IShake CreateShake()
		{
			var envelopePars = new Envelope.EnvelopeParams();
			envelopePars.Decay = Duration <= 0 ? 1 : 1 / Duration;

			var pars = new PerlinShake.Params()
			{
				Strength = new Displacement(Vector2.One * PositionStrength, RotationStrength),
				NoiseModes = NoiseModes,
				Envelope = envelopePars,
				Attenuation = Attenuation,
			};

			return new PerlinShake(pars, 1, UsesAttenuation ? (Vector2?)SourcePosition : null);
		}
	}
}
