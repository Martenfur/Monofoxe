/////////////////////////////////////////////////////////////////////////////////////////////
/// Original Unity version made by Ivan Pensionerov https://github.com/gasgiant/Camera-Shake
/// Ported and improved by Minkberry.
/////////////////////////////////////////////////////////////////////////////////////////////
using Microsoft.Xna.Framework;
using Monofoxe.Engine.Utils;
using Monofoxe.Engine.Shake.Utils;

namespace Monofoxe.Engine.Shake
{
	public class PerlinShake : IShake
	{
		private readonly Params _pars;
		private readonly Envelope _envelope;

		public IAmplitudeController AmplitudeController;

		private Vector2[] _seeds;
		private float _t;
		private Vector2? _sourcePosition;
		private float _norm;

		/// <summary>
		/// Creates an instance of PerlinShake.
		/// </summary>
		/// <param name="parameters">Parameters of the shake.</param>
		/// <param name="maxAmplitude">Maximum amplitude of the shake.</param>
		/// <param name="sourcePosition">World position of the source of the shake.</param>
		/// <param name="manualStrengthControl">Pass true if you want to control amplitude manually.</param>
		public PerlinShake(
			Params parameters,
			float maxAmplitude = 1,
			Vector2? sourcePosition = null,
			bool manualStrengthControl = false
		)
		{
			_pars = parameters;
			_envelope = new Envelope(_pars.Envelope, maxAmplitude,
				manualStrengthControl ?
					Envelope.EnvelopeControlMode.Manual : Envelope.EnvelopeControlMode.Auto);
			AmplitudeController = _envelope;
			_sourcePosition = sourcePosition;
		}


		public Displacement CurrentDisplacement { get; private set; }
		public bool IsFinished { get; private set; }


		public void Initialize(Vector2 cameraPosition, float cameraRotation)
		{
			_seeds = new Vector2[_pars.NoiseModes.Length];
			_norm = 0;
			for (int i = 0; i < _seeds.Length; i++)
			{
				_seeds[i] = RandomExt.Global.NextInsideUnitCircle() * 20;
				_norm += _pars.NoiseModes[i].Amplitude;
			}
		}


		public void Update(TimeKeeper time, Vector2 cameraPosition, float cameraRotation)
		{
			if (_envelope.IsFinished)
			{
				IsFinished = true;
				return;
			}
			_t += (float)time.Time();
			_envelope.Update((float)time.Time());

			Displacement disp = Displacement.Zero;
			for (int i = 0; i < _pars.NoiseModes.Length; i++)
			{
				disp += _pars.NoiseModes[i].Amplitude / _norm 
					* SampleNoise(_seeds[i], _pars.NoiseModes[i].Frequence);
			}

			CurrentDisplacement = _envelope.Intensity * Displacement.Scale(disp, _pars.Strength);
			if (_sourcePosition != null)
			{
				CurrentDisplacement *= _pars.Attenuation.Attenuate(_sourcePosition.Value, cameraPosition);
			}
		}


		private Displacement SampleNoise(Vector2 seed, float freq)
		{
			var position = new Vector2(
				PerlinNoise.CarmodyNoise(seed.X + _t * freq, seed.Y),
				PerlinNoise.CarmodyNoise(seed.X, seed.Y + _t * freq)
			);
			position -= Vector2.One * 0.5f;

			var rotation = PerlinNoise.CarmodyNoise(-seed.X - _t * freq, -seed.Y - _t * freq);
			rotation -= 0.5f;

			return new Displacement(position, rotation);
		}


		public class Params
		{
			/// <summary>
			/// Strength of the shake for each axis.
			/// </summary>
			public Displacement Strength = new Displacement(Vector2.Zero, 0.8f);

			/// <summary>
			/// Layers of perlin noise with different frequencies.
			/// </summary>
			public NoiseMode[] NoiseModes = { new NoiseMode(12, 1) };

			/// <summary>
			/// Strength over time.
			/// </summary>
			public Envelope.EnvelopeParams Envelope;

			/// <summary>
			/// How strength falls with distance from the shake source.
			/// </summary>
			public Attenuation Attenuation = new Attenuation();
		}


		public struct NoiseMode
		{
			public NoiseMode(float freq, float amplitude)
			{
				Frequence = freq;
				Amplitude = amplitude;
			}

			/// <summary>
			/// Frequency multiplier for the noise.
			/// </summary>
			public float Frequence;

			/// <summary>
			/// Amplitude of the mode. Ranges from 0 to 1.
			/// </summary>
			public float Amplitude;
		}
	}
}
