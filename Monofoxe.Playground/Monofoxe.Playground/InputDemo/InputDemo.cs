﻿using System;
using System.Collections.Generic;
using System.Text;
using Monofoxe.Engine.ECS;
using Monofoxe.Engine.Drawing;
using Monofoxe.Engine.Utils;
using Monofoxe.Engine;
using Monofoxe.Engine.SceneSystem;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Resources.Sprites;
using System.Text.RegularExpressions;

namespace Monofoxe.Playground.GraphicsDemo
{
	public class InputDemo : Entity
	{
		
		Color _mainColor = Color.White;
		Color _secondaryColor = Color.Violet;

		double _animation = 0;
		double _animationSpeed = 0.25;
		
		StringBuilder _keyboardInput = new StringBuilder();
		int _keyboardInputMaxLength = 32;

		public InputDemo(Layer layer) : base(layer)
		{
		}

		public override void Update()
		{
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
			var startingPosition = new Vector2(64, 64);
			var position = startingPosition;
			var spacing = 100;

			GraphicsMgr.CurrentColor = _mainColor;
			
			// This position accounts for current camera transform matrix.
			// Visually it will be at the pointer's position when camera will move.
			CircleShape.Draw(Input.MousePosition, 4, false);

			// This position only accounts for screen transformation.
			// When the camera will move, it will offset.
			CircleShape.Draw(Input.ScreenMousePosition, 8, true);

			// You can also get mouse position from any camera.
			// This method can be used in Update, when no camera is active.
			CircleShape.Draw(GraphicsMgr.CurrentCamera.GetRelativeMousePosition(), 12, true);


			Text.CurrentFont = Resources.Fonts.Arial;

			Text.Draw("Keyboard input: " + _keyboardInput.ToString(), position);

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
