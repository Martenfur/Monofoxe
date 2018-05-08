using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using System;
using System.Linq;
using System.Diagnostics;
using System.Collections.Generic;
using System.Text;

namespace Monofoxe.Engine
{
	/// <summary>
	/// Mouse buttons.
	/// </summary>
	public enum MouseButtons
	{
		Left,
		Right,
		Middle
	}

	/// <summary>
	/// Gamepad buttons, including triggers.
	/// </summary>
	public enum GamepadButtons
	{
		Left,
		Right,
		Up,
		Down,
		A,
		B,
		X,
		Y,
		LT, // Left trigger.
		RT, // Right trigger.
		LB, // Left button.
		RB, // Right button.
		LS, // Left stick.
		RS, // Right stick.
		Start,
		Select
	}


	public static class Input
	{
		// Mouse.
		/// <summary>
		/// Cursor position on screen.
		/// </summary>
		public static Vector2 ScreenMousePos {get; private set;}
		
		/// <summary>
		/// Cursor position in the world. Depends on current camera.
		/// </summary>
		public static Vector2 MousePos {get; private set;}


		private static List<MouseButtons> _mouseButtons, _previousMouseButtons;
		
		/// <summary>
		/// Scrollwheel value. Can be -1, 0 or 1.
		/// </summary>
		public static int MouseWheelVal {get; private set;}
		private static int _mouseWheelAdditionPrev;
		// Mouse.


		// Keyboard.
		/// <summary>
		/// Stores all chars typed in previous step.  
		/// </summary>
		public static string KeyboardString {get; private set;} // Comment?
		
		/// <summary>
		/// Stores last pressed key in current step. If no keys were pressed, resets to Keys.None.
		/// </summary>
		public static Keys KeyboardKey {get; private set;} = Keys.None;

		/// <summary>
		/// Stores last pressed key. Doesn't reset.
		/// </summary>
		public static Keys KeyboardLastKey {get; private set;} = Keys.None;

		/// <summary>
		/// Stores last typed char. Doesn't reset.
		/// </summary>
		public static char KeyboardLastChar {get; private set;} = ' ';

		private static StringBuilder _keyboardBuffer = new StringBuilder();
		private static Keys _keyboardLastKeyBuffer;		
		private static Keys[] _currentKeys, _previousKeys;
		// Keyboard.
		

		// Gamepad.
		/// <summary>
		/// If pressure value is below deadzone, GamepadCheck() won't detect trigger press.
		/// </summary>
		public static double GamepadTriggersDeadzone = 0.5;
		
		/// <summary>
		/// Type of stick deadzone.
		/// </summary>
		public static GamePadDeadZone GamepadDeadzoneType = GamePadDeadZone.Circular;

		private static int _maxGamepadCount = 2;

		/// <summary>
		/// Amount of gamepads which are proccessed by input.
		/// If you don't want to use gamepad input, set value to 0.
		/// </summary>
		public static int MaxGamepadCount
		{
			get => _maxGamepadCount;
			set
			{
				_maxGamepadCount = Math.Min(GamePad.MaximumGamePadCount, value);
				GamepadClear(); // Well, yeah, it's a clearing function, but also can be used to re-init arrays.
			}
		}

		private static List<GamepadButtons>[] _gamepadButtons = new List<GamepadButtons>[_maxGamepadCount], 
		                          _previousGamepadButtons = new List<GamepadButtons>[_maxGamepadCount];
		private static GamePadState[] _gamepadState = new GamePadState[_maxGamepadCount],
		                              _previousGamepadState = new GamePadState[_maxGamepadCount];
		// Gamepad.
		

		public static void Update()
		{
			#region Mouse
			MouseState mouseState = Mouse.GetState();
			
			ScreenMousePos = new Vector2(mouseState.X, mouseState.Y);

			_previousMouseButtons = _mouseButtons;
			_mouseButtons = new List<MouseButtons>();
			
			if (mouseState.LeftButton == ButtonState.Pressed)
			{
				_mouseButtons.Add(MouseButtons.Left);
			}
			if (mouseState.RightButton == ButtonState.Pressed)
			{
				_mouseButtons.Add(MouseButtons.Right);
			}
			if (mouseState.MiddleButton == ButtonState.Pressed)
			{
				_mouseButtons.Add(MouseButtons.Middle);
			}


			/*
			For some weird reason, ScrollWheelValue accumulates all the scroll inputs.
			And does it asynchroniously. So, to get usable 1\-1 value, we need to calculate 
			sign of scroll value delta (raw delta has some big weird value and depends on fps).
			Thank you, XNA devs. You made me write even more code! ^0^
			*/
			MouseWheelVal = Math.Sign(_mouseWheelAdditionPrev - mouseState.ScrollWheelValue);
			_mouseWheelAdditionPrev = mouseState.ScrollWheelValue;
			#endregion Mouse



			#region Keyboard
			KeyboardString = _keyboardBuffer.ToString();
			_keyboardBuffer.Clear();

			if (KeyboardString.Length > 0)
			{
				KeyboardLastChar = KeyboardString[KeyboardString.Length - 1];
			}

			KeyboardLastKey = _keyboardLastKeyBuffer;

			_previousKeys = _currentKeys;
			_currentKeys = Keyboard.GetState().GetPressedKeys();
			
			if (_currentKeys.Length > 0)
			{
				KeyboardKey = _currentKeys.Last();
			}
			else
			{
				KeyboardKey = Keys.None;
			}
			#endregion Keyboard


			
			#region Gamepad
			for(var i = 0; i < MaxGamepadCount; i += 1)
			{
				_previousGamepadState[i] = _gamepadState[i];
				 
				_gamepadState[i] = GamePad.GetState(i,GamepadDeadzoneType);

				_previousGamepadButtons[i] = _gamepadButtons[i];
				_gamepadButtons[i] = new List<GamepadButtons>();
			
				if (_gamepadState[i].DPad.Left == ButtonState.Pressed)
				{
					_gamepadButtons[i].Add(GamepadButtons.Left);
				}
				if (_gamepadState[i].DPad.Right == ButtonState.Pressed)
				{
					_gamepadButtons[i].Add(GamepadButtons.Right);
				}
				if (_gamepadState[i].DPad.Up == ButtonState.Pressed)
				{
					_gamepadButtons[i].Add(GamepadButtons.Up);
				}
				if (_gamepadState[i].DPad.Down == ButtonState.Pressed)
				{
					_gamepadButtons[i].Add(GamepadButtons.Down);
				}
	
				if (_gamepadState[i].Buttons.A == ButtonState.Pressed)
				{
					_gamepadButtons[i].Add(GamepadButtons.A);
				}
				if (_gamepadState[i].Buttons.B == ButtonState.Pressed)
				{
					_gamepadButtons[i].Add(GamepadButtons.B);
				}
				if (_gamepadState[i].Buttons.X == ButtonState.Pressed)
				{
					_gamepadButtons[i].Add(GamepadButtons.X);
				}
				if (_gamepadState[i].Buttons.Y == ButtonState.Pressed)
				{
					_gamepadButtons[i].Add(GamepadButtons.Y);
				}
	
				if (_gamepadState[i].Buttons.LeftShoulder == ButtonState.Pressed)
				{
					_gamepadButtons[i].Add(GamepadButtons.LB);
				}
				if (_gamepadState[i].Buttons.RightShoulder == ButtonState.Pressed)
				{
					_gamepadButtons[i].Add(GamepadButtons.RB);
				}
				
				if (_gamepadState[i].Triggers.Left > GamepadTriggersDeadzone)
				{
					_gamepadButtons[i].Add(GamepadButtons.LT);
				}
				if (_gamepadState[i].Triggers.Right > GamepadTriggersDeadzone)
				{
					_gamepadButtons[i].Add(GamepadButtons.RT);
				}

				if (_gamepadState[i].Buttons.LeftStick == ButtonState.Pressed)
				{
					_gamepadButtons[i].Add(GamepadButtons.LS);
				}
				if (_gamepadState[i].Buttons.RightStick == ButtonState.Pressed)
				{
					_gamepadButtons[i].Add(GamepadButtons.RS);
				}

				if (_gamepadState[i].Buttons.Start == ButtonState.Pressed)
				{
					_gamepadButtons[i].Add(GamepadButtons.Start);
				}
				if (_gamepadState[i].Buttons.Back == ButtonState.Pressed)
				{
					_gamepadButtons[i].Add(GamepadButtons.Select);
				}
			}
			#endregion Gamepad
			
		}



		#region mouse

		/// <summary>
		/// Checks if mouse button is down in current step.
		/// </summary>
		/// <param name="button">Button to check.</param>
		/// <returns>Returns if button is down.</returns>
		public static bool MouseCheck(MouseButtons button) => 
			_mouseButtons.Contains(button);


		/// <summary>
		/// Checks if mouse button is pressed.
		/// </summary>
		/// <param name="button">Button to check.</param>
		/// <returns>Returns if button is pressed.</returns>
		public static bool MouseCheckPress(MouseButtons button) => 
			(_mouseButtons.Contains(button) && !_previousMouseButtons.Contains(button));


		/// <summary>
		/// Checks if mouse button is released.
		/// </summary>
		/// <param name="button">Button to check.</param>
		/// <returns>Returns if button is released.</returns>
		public static bool MouseCheckRelease(MouseButtons button) => 
			(!_mouseButtons.Contains(button) && _previousMouseButtons.Contains(button));


		/// <summary>
		/// Clears mouse input.
		/// </summary>
		public static void MouseClear()
		{
			_mouseButtons = null;
			_previousMouseButtons = null;
		}


		/// <summary>
		/// Updates mouse position in the world.
		/// </summary>
		public static void UpdateMouseWorldPosition()
		{
			Matrix m = Matrix.Invert(DrawCntrl.CurrentTransformMatrix);
			
			Vector3 buffer = Vector3.Transform(new Vector3(ScreenMousePos.X, ScreenMousePos.Y, 0), m);
			MousePos = new Vector2(buffer.X, buffer.Y) - DrawCntrl.CurrentCamera.PortPos;
		}


		#endregion mouse



		#region keyboard

		/// <summary>
		/// Checks if keyboard key is down in current step.
		/// </summary>
		/// <param name="key">Key to check.</param>
		public static bool KeyboardCheck(Keys key) => 
			_currentKeys.Contains(key);


		/// <summary>
		/// Checks if keyboard key is pressed.
		/// </summary>
		/// <param name="key">Key to check.</param>
		public static bool KeyboardCheckPress(Keys key) => 
			(_currentKeys.Contains(key) && !_previousKeys.Contains(key));


		/// <summary>
		/// Checks if keyboard key is released.
		/// </summary>
		/// <param name="key">Key to check.</param>
		public static bool KeyboardCheckRelease(Keys key) => 
			(!_currentKeys.Contains(key) && _previousKeys.Contains(key));


		/// <summary>
		/// Checks if any keyboard key in down in current step.
		/// </summary>
		/// <returns></returns>
		public static bool KeyboardCheckAnyKey => 
			_currentKeys.Length > 0;


		/// <summary>
		/// Checks if any keyboard key in pressed.
		/// </summary>
		/// <returns></returns>
		public static bool KeyboardCheckAnyKeyPress => 
			(_currentKeys.Length > 0 && _previousKeys.Length == 0);


		/// <summary>
		/// Checks if any keyboard key in released.
		/// </summary>
		/// <returns></returns>
		public static bool KeyboardCheckAnyKeyRelease() => 
			(_currentKeys.Length == 0 && _previousKeys.Length > 0);


		/// <summary>
		/// Clears keyboard input.
		/// </summary>
		public static void KeyboardClear()
		{
			_currentKeys = null;
			_previousKeys = null;
		}


		/// <summary>
		/// Text input event. Occurs when any key is pressed.
		/// Works asynchroniously.
		/// Assigned to Window.TextInput of Game1 class.
		/// </summary>
		/// <param name="sender">Sender.</param>
		/// <param name="e">Event arguments, including current pressed key.</param>
		public static void TextInput(object sender, TextInputEventArgs e)
		{
			_keyboardBuffer.Append(e.Character);
			_keyboardLastKeyBuffer = e.Key;
		}

		#endregion keyboard



		#region gamepad

		/// <summary>
		/// Checks if gamepad with given inex is connected.
		/// </summary>
		/// <param name="index">Index of gamepad.</param>
		/// <returns></returns>
		public static bool GamepadConnected(int index)
		{
			if (index < _gamepadState.Length)
			{
				return _gamepadState[index].IsConnected;
			}
			return false;
		}

		/// <summary>
		/// Checks if gamepad button is down in current step.
		/// </summary>
		/// <param name="index">Index of gamepad.</param>
		/// <param name="button">Button to check.</param>
		/// <returns>Returns if button is down.</returns>
		public static bool GamepadCheck(int index, GamepadButtons button)
		{
			if (index < _gamepadButtons.Length)
			{
				return (_gamepadButtons[index].Contains(button));
			}
			return false;
		}

		
		/// <summary>
		/// Checks if gamepad button is pressed.
		/// </summary>
		/// <param name="index">Index of gamepad.</param>
		/// <param name="button">Button to check.</param>
		/// <returns>Returns if button is pressed.</returns>
		public static bool GamepadCheckPress(int index, GamepadButtons button)
		{
			if (index < _gamepadButtons.Length)
			{
				return (_gamepadButtons[index].Contains(button) && !_previousGamepadButtons[index].Contains(button));
			}
			return false;
		}

		
		/// <summary>
		/// Checks if gamepad button is released.
		/// </summary>
		/// <param name="index">Index of gamepad.</param>
		/// <param name="button">Button to check.</param>
		/// <returns>Returns if button is released.</returns>
		public static bool GamepadCheckRelease(int index, GamepadButtons button)
		{
			if (index < _gamepadButtons.Length)
			{
				return (!_gamepadButtons[index].Contains(button) && _previousGamepadButtons[index].Contains(button));
			}
			return false;
		}


		/// <summary>
		/// Returns vector of left thumb stick.
		/// </summary>
		/// <param name="index">Index of gamepad.</param>
		public static Vector2 GamepadGetLeftStick(int index)
		{
			if (index < _gamepadState.Length)
			{
				return _gamepadState[index].ThumbSticks.Left;
			}
			return Vector2.Zero;
		}

		
		/// <summary>
		/// Returns vector of right thumb stick.
		/// </summary>
		/// <param name="index">Index of gamepad.</param>
		public static Vector2 GamepadGetRightStick(int index)
		{
			if (index < _gamepadState.Length)
			{
				return _gamepadState[index].ThumbSticks.Right;
			}
			return Vector2.Zero;
		}

		
		/// <summary>
		/// Returns value of pressure on left trigger.
		/// NOTE: If you don't need exact pressure value, use GamepadCheck* functions.
		/// </summary>
		/// <param name="index">Index of gamepad.</param>
		/// <returns>Returns value of pressure (0..1) on left trigger.</returns>
		public static float GamepadGetLeftTrigger(int index)
		{
			if (index < _gamepadState.Length)
			{
				return _gamepadState[index].Triggers.Left;
			}
			return 0f;
		}


		/// <summary>
		/// Returns value of pressure on right trigger.
		/// NOTE: If you don't need exact pressure value, use GamepadCheck* functions.
		/// </summary>
		/// <param name="index">Index of gamepad.</param>
		/// <returns>Returns value of pressure (0..1) on right trigger.</returns>
		public static float GamepadGetRightTrigger(int index)
		{
			if (index < _gamepadState.Length)
			{
				return _gamepadState[index].Triggers.Right;
			}
			return 0f;
		}

		/// <summary>
		/// Sets vibration to the given gamepad.
		/// </summary>
		/// <param name="index">Index of gamepad.</param>
		/// <param name="leftMotor">Vibration intensity for left motor (0 to 1).</param>
		/// <param name="rightMotor">Vibration intensity for right motor (0 to 1).</param>
		public static void GamepadSetVibration(int index, float leftMotor, float rightMotor)
			=> GamePad.SetVibration(index, leftMotor, rightMotor);

		/// <summary>
		/// Clears gamepad input, including triggers and thumb sticks.
		/// </summary>
		public static void GamepadClear()
		{
			_gamepadButtons = new List<GamepadButtons>[_maxGamepadCount];
			_previousGamepadButtons = new List<GamepadButtons>[_maxGamepadCount];
			_gamepadState = new GamePadState[_maxGamepadCount];
			_previousGamepadState = new GamePadState[_maxGamepadCount];
		}

		#endregion gamepad



		/// <summary>
		/// Clears mouse, keyboard and gamepad input.
		/// </summary>
		public static void IOClear()
		{
			MouseClear();
			KeyboardClear();
			GamepadClear();
		}
	}
}
