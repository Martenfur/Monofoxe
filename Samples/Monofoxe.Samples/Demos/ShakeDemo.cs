using System;
using Microsoft.Xna.Framework;
using Monofoxe.Engine;
using Monofoxe.Engine.Drawing;
using Monofoxe.Engine.EC;
using Monofoxe.Engine.Resources;
using Monofoxe.Engine.SceneSystem;
using Monofoxe.Engine.Shake;
using Monofoxe.Engine.Shake.Presets;
using Monofoxe.Engine.Utils;

namespace Monofoxe.Samples.Demos
{
	public class ShakeDemo : Entity
	{
		public static readonly string Description = "A to doubt." +
			Environment.NewLine +
			"S to eat." +
			Environment.NewLine +
			"D to steal his dainty.";

		public const Buttons DoubtButton = Buttons.A;
		public const Buttons EatButton = Buttons.S;
		public const Buttons StealButton = Buttons.D;


		private Shaker _shaker;

		private IShake _currentShake;


		private Sprite _cat;
		private Vector2 _catPosition;

		private Sprite _dainty;
		private Vector2 _daintyPosition;

		private bool _stoleDainty;

		public ShakeDemo(Layer layer) : base(layer)
		{
			_shaker = new Shaker(layer);

			_cat = ResourceHub.GetResource<Sprite>("DefaultSprites", "AutismCat");
			_catPosition = GameController.MainCamera.Size / 2 - _cat.Size / 2;

			_dainty = ResourceHub.GetResource<Sprite>("DefaultSprites", "AutismCatDish");
			_daintyPosition = GameController.MainCamera.Size / 2 + Vector2.UnitY * 100;
		}

		public override void Update()
		{
			if (Input.CheckButton(DoubtButton))
			{
				_currentShake = _shaker.Shake(
					new ShortShakePreset()
					{
						PositionStrength = 0.7f,
						BounceCount = 30
					}
				);

				_stoleDainty = false;
			}
			else if (Input.CheckButton(EatButton))
			{
				_currentShake = (BounceShake)_shaker.Shake
				(
					new DirectionalShakePreset()
					{
						Direction = Vector2.UnitY,
						Frequency = 8,
						PositionStrength = 10
					}
				);

				_stoleDainty = false;
			}
			else if (Input.CheckButton(StealButton))
			{
				_currentShake = (PerlinShake)_shaker.Shake
				(
					new ExplosionShakePreset()
					{
						Duration = 2,
						PositionStrength = 500
					}
				);

				_stoleDainty = true;
			}


			Vector2 displacement = default;

			if (_currentShake != null)
			{
				displacement = _currentShake.CurrentDisplacement.Position;
			}

			_catPosition = GameController.MainCamera.Size / 2 - _cat.Size - Vector2.UnitY * 70 + 
				displacement;
		}

		public override void Draw()
		{
			_cat.Draw(_catPosition, 0, Vector2.One * 2, Angle.Right, Color.White);

			if (!_stoleDainty)
			{
				_dainty.Draw(_daintyPosition, 0, Vector2.One * 2, Angle.Right, Color.White);
			}
		}
	}
}
