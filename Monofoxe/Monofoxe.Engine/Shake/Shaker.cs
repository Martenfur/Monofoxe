/////////////////////////////////////////////////////////////////////////////////////////////
/// Original Unity version made by Ivan Pensionerov https://github.com/gasgiant/Camera-Shake
/// Ported and improved by Minkberry.
/////////////////////////////////////////////////////////////////////////////////////////////
using System.Collections.Generic;
using Monofoxe.Engine.Shake.Presets;
using Monofoxe.Engine.Shake.Utils;
using Microsoft.Xna.Framework;
using Monofoxe.Engine.EC;
using Monofoxe.Engine.SceneSystem;
using Monofoxe.Engine.Utils;

namespace Monofoxe.Engine.Shake
{
	/// <summary>
	/// Camera shaker component registeres new shakes, holds a list of active shakes, and applies them to the camera additively.
	/// </summary>
	public class Shaker : Entity
	{
		private readonly List<IShake> _activeShakes = new List<IShake>();

		public Vector2 ShakePosition { get; private set; }
		public Angle ShakeRotation { get; private set; }


		public float StrengthMultiplier = 1f;

		public Shaker(Layer layer) : base(layer)
		{
		}


		/// <summary>
		/// Creates a shake from preset.
		/// </summary>
		public IShake Shake(ShakePreset preset)
		{
			var shake = preset.CreateShake();
			Shake(shake);
			return shake;
		}


		/// <summary>
		/// Adds a shake to the list of active shakes.
		/// </summary>
		public void Shake(IShake shake)
		{
			shake.Initialize(ShakePosition, Angle.Right.RadiansF);
			_activeShakes.Add(shake);
		}


		public override void Update()
		{
			Displacement cameraDisplacement = Displacement.Zero;
			for (int i = _activeShakes.Count - 1; i >= 0; i--)
			{
				if (_activeShakes[i].IsFinished)
				{
					_activeShakes.RemoveAt(i);
				}
				else
				{
					_activeShakes[i].Update(TimeKeeper.Global, ShakePosition, Angle.Right.RadiansF);
					cameraDisplacement += _activeShakes[i].CurrentDisplacement;
				}
			}
			ShakePosition = StrengthMultiplier * cameraDisplacement.Position;
			ShakeRotation = Angle.FromRadians(StrengthMultiplier * cameraDisplacement.Angle);
		}
	}
}
