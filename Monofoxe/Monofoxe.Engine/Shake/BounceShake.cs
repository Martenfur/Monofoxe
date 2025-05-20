/////////////////////////////////////////////////////////////////////////////////////////////
/// Original Unity version made by Ivan Pensionerov https://github.com/gasgiant/Camera-Shake
/// Ported and improved by Minkberry.
/////////////////////////////////////////////////////////////////////////////////////////////
using Monofoxe.Engine.Shake.Utils;
using Microsoft.Xna.Framework;
using Monofoxe.Engine.Utils;

namespace Monofoxe.Engine.Shake
{
	public class BounceShake : IShake
	{
		private readonly Params _pars;
		private readonly Easing _moveCurve = Easing.EaseInOutCubic;
		private readonly Vector2? _sourcePosition = null;

		private float _attenuation = 1;
		private Displacement _direction;
		private Displacement _previousWaypoint;
		private Displacement _currentWaypoint;
		private int _bounceIndex;
		private float _t;


		/// <summary>
		/// Creates an instance of BounceShake.
		/// </summary>
		/// <param name="parameters">Parameters of the shake.</param>
		/// <param name="sourcePosition">World position of the source of the shake.</param>
		public BounceShake(Params parameters, Vector2? sourcePosition = null)
		{
			_sourcePosition = sourcePosition;
			_pars = parameters;
			Displacement rnd = Displacement.InsideUnitSpheres();
			_direction = Displacement.Scale(rnd, _pars.AxesMultiplier).Normalized;
		}


		/// <summary>
		/// Creates an instance of BounceShake.
		/// </summary>
		/// <param name="parameters">Parameters of the shake.</param>
		/// <param name="initialDirection">Initial direction of the shake motion.</param>
		/// <param name="sourcePosition">World position of the source of the shake.</param>
		public BounceShake(Params parameters, Displacement initialDirection, Vector2? sourcePosition = null)
		{
			_sourcePosition = sourcePosition;
			_pars = parameters;
			_direction = Displacement.Scale(initialDirection, _pars.AxesMultiplier).Normalized;
		}


		public Displacement CurrentDisplacement { get; private set; }
		public bool IsFinished { get; private set; }


		public void Initialize(Vector2 cameraPosition, float cameraRotation)
		{
			_attenuation = _sourcePosition == null ?
				1 : _pars.Attenuation.Attenuate(_sourcePosition.Value, cameraPosition);
			_currentWaypoint = _attenuation * _direction.ScaledBy(_pars.PositionStrength, _pars.RotationStrength);
		}


		public void Update(TimeKeeper time, Vector2 cameraPosition, float cameraRotation)
		{
			if (_t < 1)
			{
				_t += (float)time.Time() * _pars.Frequence;
				if (_pars.Frequence == 0) _t = 1;

				CurrentDisplacement = Displacement.Lerp(_previousWaypoint, _currentWaypoint,
					_moveCurve.GetEasing(_t));
			}
			else
			{
				_t = 0;
				CurrentDisplacement = _currentWaypoint;
				_previousWaypoint = _currentWaypoint;
				_bounceIndex++;
				if (_bounceIndex > _pars.NumBounces)
				{
					IsFinished = true;
					return;
				}

				Displacement rnd = Displacement.InsideUnitSpheres();
				_direction = -_direction
					+ _pars.Randomness * Displacement.Scale(rnd, _pars.AxesMultiplier).Normalized;
				_direction = _direction.Normalized;
				float decayValue = 1 - (float)_bounceIndex / _pars.NumBounces;
				_currentWaypoint = decayValue * decayValue * _attenuation
					* _direction.ScaledBy(_pars.PositionStrength, _pars.RotationStrength);
			}
		}


		public class Params
		{
			/// <summary>
			/// Strength of the shake for positional axes.
			/// </summary>
			public float PositionStrength = 0.05f;

			/// <summary>
			/// Strength of the shake for rotational axes.
			/// </summary>
			public float RotationStrength = 0.1f;

			/// <summary>
			/// Preferred direction of shaking.
			/// </summary>
			public Displacement AxesMultiplier = new Displacement(Vector2.One, 0);

			/// <summary>
			/// Frequency of shaking.
			/// </summary>
			public float Frequence = 25;

			/// <summary>
			/// Number of vibrations before stop.
			/// </summary>
			public int NumBounces = 5;

			/// <summary>
			/// Randomness of motion.
			/// </summary>
			public float Randomness = 0.5f;

			/// <summary>
			/// How strength falls with distance from the shake source.
			/// </summary>
			public Attenuation Attenuation = new Attenuation();
		}
	}
}
