using Microsoft.Xna.Framework;
using Monofoxe.Engine;
using Monofoxe.Engine.CoroutineSystem;
using Monofoxe.Engine.Drawing;
using Monofoxe.Engine.EC;
using Monofoxe.Engine.Resources;
using Monofoxe.Engine.SceneSystem;
using System.Collections;
using System.Text;
using System.Text.RegularExpressions;

namespace Monofoxe.Playground.InputDemo
{
	public class InputDemo : Entity
	{

		Color _mainColor = Color.White;
		Color _secondaryColor = Color.Violet;

		double _animation = 0;
		double _animationSpeed = 0.25;

		StringBuilder _keyboardInput = new StringBuilder();
		int _keyboardInputMaxLength = 32;

		public const Buttons KeyboardTestButton = Buttons.A;
		public const Buttons GamepadTestButton = Buttons.GamepadA;
		public const Buttons MouseTestButton = Buttons.MouseLeft;


		public InputDemo(Layer layer) : base(layer)
		{
			StartCoroutine(Testo());
		}

		IEnumerator Testo()
		{
			for (var i = 0; i < 50; i += 1)
			{
				_mainColor = Color.Black;
				yield return null;
				_mainColor = Color.Green;
				yield return null;
				_mainColor = Color.Yellow;
				yield return null;
				_mainColor = Color.AliceBlue;
			}

			yield return new ConditionTest();
			
			yield return DeepTesto();

			yield return new WaitForSeconds(3);
			_mainColor = Color.Red;
			yield return new WaitForSeconds(3);
			_mainColor = Color.Blue;
		}

		IEnumerator DeepTesto()
		{
			_mainColor = Color.Transparent;
			yield return new WaitForSeconds(3);
		}

		public override void Update()
		{
			base.Update();

			// Leaving only letters and digits.
			_keyboardInput.Append(Regex.Replace(Input.KeyboardString, @"[^A-Za-z0-9]+", string.Empty));

			// Backspace.
			if (Input.KeyboardKey == Microsoft.Xna.Framework.Input.Keys.Back)
			{
				if (_keyboardInput.Length >= 2)
				{
					_keyboardInput.Remove(_keyboardInput.Length - 2, 2);
				}
				else
				{
					_keyboardInput.Clear();
				}
			}

			// Limiting string length.
			if (_keyboardInput.Length > _keyboardInputMaxLength)
			{
				var overflow = _keyboardInput.Length - _keyboardInputMaxLength;
				_keyboardInput.Remove(0, overflow);
			}

			// Time to get your Rumble Pak (tm), kidz!
			Input.GamepadSetVibration(
				0,
				Input.GamepadGetLeftTrigger(0),
				Input.GamepadGetRightTrigger(0)
			);

		}

		public override void Draw()
		{
			base.Draw();

			var startingPosition = new Vector2(64, 64);
			var position = startingPosition;
			var spacing = 100;

			GraphicsMgr.CurrentColor = _mainColor;

			// This position only accounts for screen transformation.
			// When the camera will move, it will offset.
			CircleShape.Draw(Input.ScreenMousePosition, 8, true);

			// You can also get mouse position from any camera.
			// This method can be used in Update, when no camera is active.
			CircleShape.Draw(GraphicsMgr.CurrentCamera.GetRelativeMousePosition(), 12, true);


			Text.CurrentFont = ResourceHub.GetResource<IFont>("Fonts", "Arial");

			Text.Draw("Keyboard input: " + _keyboardInput.ToString(), position);


			// Gamepad, mouse and keyboard buttons are using the same method. 
			position += Vector2.UnitY * 64;
			CircleShape.Draw(position, 16, Input.CheckButton(KeyboardTestButton));
			position += Vector2.UnitX * 64;
			CircleShape.Draw(position, 16, Input.CheckButton(GamepadTestButton));
			position += Vector2.UnitX * 64;
			CircleShape.Draw(position, 16, Input.CheckButton(MouseTestButton));


			position = new Vector2(200, 200);

			if (Input.GamepadConnected(0))
			{
				Text.Draw("Gamepad is connected!", position);
			}
			else
			{
				Text.Draw("Gamepad is not connected.", position);
			}


			// Sticks.
			position += Vector2.UnitY * 96;
			CircleShape.Draw(position, 64, true);
			CircleShape.Draw(position + Input.GamepadGetLeftStick(0) * 64 * new Vector2(1, -1), 16, false);
			position += Vector2.UnitX * (128 + 64);
			CircleShape.Draw(position, 64, true);
			CircleShape.Draw(position + Input.GamepadGetRightStick(0) * 64 * new Vector2(1, -1), 16, false);

			// Triggers.
			position -= Vector2.UnitX * (64 + 16);
			RectangleShape.DrawBySize(position + Vector2.UnitY * Input.GamepadGetRightTrigger(0) * 64, Vector2.One * 8, false);
			LineShape.Draw(position, position + Vector2.UnitY * 64);
			position -= Vector2.UnitX * 32;
			RectangleShape.DrawBySize(position + Vector2.UnitY * Input.GamepadGetLeftTrigger(0) * 64, Vector2.One * 8, false);
			LineShape.Draw(position, position + Vector2.UnitY * 64);

		}

		public override void Destroy()
		{
			Input.GamepadSetVibration(0, 0, 0);
		}


	}
}
