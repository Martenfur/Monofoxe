/////////////////////////////////////////////////////////////////////////////////////////////
/// Original Unity version made by Ivan Pensionerov https://github.com/gasgiant/Camera-Shake
/// Ported and improved by Minkberry.
/////////////////////////////////////////////////////////////////////////////////////////////
using Microsoft.Xna.Framework;
using Monofoxe.Engine;

namespace Monofoxe.Engine.Shake.Utils
{
	/// <summary>
	/// Controls strength of the shake over time.
	/// </summary>
	public class Envelope : IAmplitudeController
	{
		private readonly EnvelopeParams _pars;
		private readonly EnvelopeControlMode _controlMode;

		private float _amplitude;
		private float _targetAmplitude;
		private float _sustainEndTime;
		private bool _finishWhenAmplitudeZero;
		private bool _finishImmediately;
		private EnvelopeState _state;

		/// <summary>
		/// Creates an Envelope instance.
		/// </summary>
		/// <param name="pars">Envelope parameters.</param>
		/// <param name="controlMode">Pass Auto for a single shake, or Manual for controlling strength manually.</param>
		public Envelope(EnvelopeParams pars, float initialTargetAmplitude, EnvelopeControlMode controlMode)
		{
			_pars = pars;
			_controlMode = controlMode;
			SetTarget(initialTargetAmplitude);
		}

		/// <summary>
		/// The value by which you want to multiply shake displacement.
		/// </summary>
		public float Intensity { get; private set; }


		public bool IsFinished
		{
			get
			{
				if (_finishImmediately) return true;
				return (_finishWhenAmplitudeZero || _controlMode == EnvelopeControlMode.Auto)
					&& _amplitude <= 0 && _targetAmplitude <= 0;
			}
		}


		public void Finish()
		{
			_finishWhenAmplitudeZero = true;
			SetTarget(0);
		}


		public void FinishImmediately()
		{
			_finishImmediately = true;
		}


		/// <summary>
		/// Update is called every frame by the shake.
		/// </summary>
		public void Update(float deltaTime)
		{
			if (IsFinished) return;

			if (_state == EnvelopeState.Increase)
			{
				if (_pars.Attack > 0)
					_amplitude += deltaTime * _pars.Attack;
				if (_amplitude > _targetAmplitude || _pars.Attack <= 0)
				{
					_amplitude = _targetAmplitude;
					_state = EnvelopeState.Sustain;
					if (_controlMode == EnvelopeControlMode.Auto)
						_sustainEndTime = (float)GameMgr.ElapsedTimeTotal + _pars.Sustain;
				}
			}
			else
			{
				if (_state == EnvelopeState.Decrease)
				{

					if (_pars.Decay > 0)
						_amplitude -= deltaTime * _pars.Decay;
					if (_amplitude < _targetAmplitude || _pars.Decay <= 0)
					{
						_amplitude = _targetAmplitude;
						_state = EnvelopeState.Sustain;
					}
				}
				else
				{
					if (_controlMode == EnvelopeControlMode.Auto && (float)GameMgr.ElapsedTimeTotal > _sustainEndTime)
					{
						SetTarget(0);
					}
				}
			}

			_amplitude = MathHelper.Clamp(_amplitude, 0, 1);
			Intensity = Power.Evaluate(_amplitude, _pars.Degree);
		}


		public void SetTargetAmplitude(float value)
		{
			if (_controlMode == EnvelopeControlMode.Manual && !_finishWhenAmplitudeZero)
			{
				SetTarget(value);
			}
		}


		private void SetTarget(float value)
		{
			_targetAmplitude = MathHelper.Clamp(value, 0, 1);
			_state = _targetAmplitude > _amplitude ? EnvelopeState.Increase : EnvelopeState.Decrease;
		}


		public class EnvelopeParams
		{
			/// <summary>
			/// How fast the amplitude rises.
			/// </summary>
			public float Attack = 10;

			/// <summary>
			/// How long in seconds the amplitude holds a maximum value.
			/// </summary>
			public float Sustain = 0;

			/// <summary>
			/// How fast the amplitude falls.
			/// </summary>
			public float Decay = 1f;

			/// <summary>
			/// Power in which the amplitude is raised to get intensity.
			/// </summary>
			public Degree Degree = Degree.Cubic;
		}

		public enum EnvelopeControlMode
		{
			Auto,
			Manual
		}

		public enum EnvelopeState
		{
			Sustain,
			Increase,
			Decrease
		}
	}


	public interface IAmplitudeController
	{
		/// <summary>
		/// Sets value to which amplitude will move over time.
		/// </summary>
		void SetTargetAmplitude(float value);

		/// <summary>
		/// Sets amplitude to zero and finishes the shake when zero is reached.
		/// </summary>
		void Finish();

		/// <summary>
		/// Immediately finishes the shake.
		/// </summary>
		void FinishImmediately();
	}
}
