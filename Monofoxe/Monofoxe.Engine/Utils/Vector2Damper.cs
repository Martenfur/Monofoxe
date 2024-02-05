using Microsoft.Xna.Framework;
using System;

namespace Monofoxe.Engine.Utils
{
	/// <summary>
	/// Smooth damper based on second order motion. 
	/// Stolen from https://www.youtube.com/watch?v=KPoeNZZ6H4s
	/// </summary>
	public class Vector2Damper
	{
		/// <summary>
		/// The speed at which the system responds to changes in input and the frequency of vibration.
		/// Measured in Hertz.
		/// </summary>
		public float Frequency
		{
			get => _frequency;
			set
			{
				if (_frequency != value)
				{
					_frequency = value;
					_dirtyConstants = true;
				}
			}
		}
		private float _frequency;


		/// <summary>
		/// Controls how the system damps.
		/// 0 - No damping, infinite vibration.
		/// 0..1 - Vibration depending on the magnitude.
		/// >1 - No vibration, system slowly settles towards the target.
		/// </summary>
		public float DampingCoefficient
		{
			get => _dampingCoefficient;
			set
			{
				if (_dampingCoefficient != value)
				{
					_dampingCoefficient = value;
					_dirtyConstants = true;
				}
			}
		}
		private float _dampingCoefficient;


		/// <summary>
		/// Controls how the system responds to change.
		/// 0 - The system takes time to begin accelerating from rest.
		/// >0 - The system reacts immediately.
		/// >1 - The system overshoots the target.
		/// less than 0 - The system anticilates.
		/// </summary>
		public float InitialResponse
		{
			get => _initialResponse;
			set
			{
				if (_initialResponse != value)
				{
					_initialResponse = value;
					_dirtyConstants = true;
				}
			}
		}
		private float _initialResponse;
		
		
		public Vector2 Value => _y;


		public TimeKeeper Time = TimeKeeper.Global;


		/// <summary>
		/// If true, constants need to be recalculated.
		/// </summary>
		private bool _dirtyConstants = false;


		private Vector2 _oldValue; // Previous input.
		private Vector2 _y, _yd; // State variables.
		private float _k1, _k2, _k3; // Dynamic constants.


		public Vector2Damper(
			Vector2 initialValue,
			float frequency = 1,
			float dampingCoefficient = 1,
			float initialResponse = 2
		)
		{
			_frequency = frequency;
			_dampingCoefficient = dampingCoefficient;
			_initialResponse = initialResponse;

			// Compute constants.
			ComputeConstants();

			// Init variables.
			_oldValue = initialValue;
			_y = initialValue;
			_yd = Vector2.Zero;
		}


		private Vector2Damper(
			float frequency, float dampingCoefficient, float initialResponse,
			TimeKeeper time,
			Vector2 oldValue,
			Vector2 y, Vector2 yd,
			float k1, float k2, float k3
		)
		{
			_frequency = frequency;
			_dampingCoefficient = dampingCoefficient;
			_initialResponse = initialResponse;
			Time = time;
			_oldValue = oldValue;
			_y = y;
			_yd = yd;
			_k1 = k1;
			_k2 = k2;
			_k3 = k3;
		}


		public Vector2 Update(Vector2 value)
		{
			float num = (float)Time.Time();
			var speed = Vector2.Zero;
			if (num != 0)
			{
				speed = (value - _oldValue) / num;
			}
			_oldValue = value;
			return Update(value, speed);
		}


		public Vector2 Update(Vector2 value, Vector2 speed)
		{
			if (_dirtyConstants)
			{
				ComputeConstants();
				_dirtyConstants = false;
			}

			var dt = (float)Time.Time();

			// Clamping k2 to guarantee stability without jitter.
			var k2Stable = Math.Max(Math.Max(_k2, dt * dt / 2 + dt * _k1 / 2), dt * _k1);
			_y = _y + dt * _yd; // Integrating position by velocity.
			_yd = _yd + dt * (value + _k3 * speed - _y - _k1 * _yd) / k2Stable; // Integrating velocity by acceleration.

			return _y;
		}


		/// <summary>
		/// Offsets the internal values of the damper while keeping its motion.
		/// </summary>
		public void Offset(Vector2 offset)
		{
			_y += offset;
			_oldValue += offset;
		}


		private void ComputeConstants()
		{
			_k1 = _dampingCoefficient / (MathHelper.Pi * Frequency);
			_k2 = 1f / ((2 * MathHelper.Pi * Frequency) * (2 * MathHelper.Pi * Frequency));
			_k3 = _initialResponse * _dampingCoefficient / (2 * MathHelper.Pi * Frequency);
		}


		public Vector2Damper Clone()
		{
			return new Vector2Damper(
				_frequency, _dampingCoefficient, _initialResponse,
				Time,
				_oldValue,
				_y, _yd,
				_k1, _k2, _k3
			);
		}
	}
}
