using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Monofoxe.Engine.Drawing;

namespace Monofoxe.Engine
{

	public static class Input
	{
		#region Mouse.
	
		/// <summary>
		/// Cursor position on screen.
		/// </summary>
		public static Vector2 ScreenMousePosition {get; private set;} = Vector2.Zero;
		
		/// <summary>
		/// Cursor position in the world. Depends on current camera.
		/// </summary>
		public static Vector2 MousePosition {get; internal set;} = Vector2.Zero;


		private static List<Buttons> _mouseButtons = new List<Buttons>(),
			_previousMouseButtons = new List<Buttons>();
		

		/// <summary>
		/// Scrollwheel value. Can be -1, 0 or 1.
		/// </summary>
		public static int MouseWheelValue {get; private set;}
		private static int _mouseWheelAdditionPrev;

		#endregion Mouse.


		#region Keyboard.

		/// <summary>
		/// Stores all chars typed in previous step.  
		/// </summary>
		public static string KeyboardString {get; private set;}
		
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
		private static Keys _keyboardLastKeyBuffer = Keys.None;		
		private static Keys[] _currentKeys = new Keys[0]; // A little bit of wasted memory, but code gets much simplier.
		private static Keys[] _previousKeys = new Keys[0];

		#endregion Keyboard.
		

		#region Gamepad.

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
				GamepadInit();
			}
		}

		private static List<Buttons>[] _gamepadButtons = new List<Buttons>[_maxGamepadCount], 
			_previousGamepadButtons = new List<Buttons>[_maxGamepadCount];

		private static GamePadState[] _gamepadState = new GamePadState[_maxGamepadCount],
			_previousGamepadState = new GamePadState[_maxGamepadCount];

		#endregion Gamepad.
		

		private const int _keyboardMaxCode = 1000;
		private const int _mouseMaxCode = 2000;
		private const int _gamepadMaxCode = 3000;


		private static bool _mouseCleared, _keyboardCleared, _gamepadCleared;

		internal static void Update()
		{
			_mouseCleared = false;
			_keyboardCleared = false;
			_gamepadCleared = false;


			#region Mouse.

			MouseState mouseState = Mouse.GetState();
			
			var m = Matrix.Invert(GraphicsMgr.CanvasMatrix);
			var buffer = Vector3.Transform(new Vector3(mouseState.X, mouseState.Y, 0), m);
			ScreenMousePosition = new Vector2(buffer.X, buffer.Y);
			
			_previousMouseButtons = _mouseButtons;
			_mouseButtons = new List<Buttons>();
			
			if (mouseState.LeftButton == ButtonState.Pressed)
			{
				_mouseButtons.Add(Buttons.MouseLeft);
			}
			if (mouseState.RightButton == ButtonState.Pressed)
			{
				_mouseButtons.Add(Buttons.MouseRight);
			}
			if (mouseState.MiddleButton == ButtonState.Pressed)
			{
				_mouseButtons.Add(Buttons.MouseMiddle);
			}


			/*
			For some weird reason, ScrollWheelValue accumulates all the scroll inputs.
			And does it asynchroniously. So, to get usable 1\-1 value, we need to calculate 
			sign of scroll value delta (raw delta has some big weird value and depends on fps).
			Thank you, XNA devs. You've made me write even more code! ^0^
			*/
			MouseWheelValue = Math.Sign(_mouseWheelAdditionPrev - mouseState.ScrollWheelValue);
			_mouseWheelAdditionPrev = mouseState.ScrollWheelValue;

			#endregion Mouse.



			#region Keyboard.

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

			#endregion Keyboard.


			
			#region Gamepad.

			for(var i = 0; i < MaxGamepadCount; i += 1)
			{
				_previousGamepadState[i] = _gamepadState[i];
				 
				_gamepadState[i] = GamePad.GetState(i,GamepadDeadzoneType);

				_previousGamepadButtons[i] = _gamepadButtons[i];
				_gamepadButtons[i] = new List<Buttons>();
			
				// AW YISS

				if (_gamepadState[i].DPad.Left == ButtonState.Pressed)
				{
					_gamepadButtons[i].Add(Buttons.GamepadLeft);
				}
				if (_gamepadState[i].DPad.Right == ButtonState.Pressed)
				{
					_gamepadButtons[i].Add(Buttons.GamepadRight);
				}
				if (_gamepadState[i].DPad.Up == ButtonState.Pressed)
				{
					_gamepadButtons[i].Add(Buttons.GamepadUp);
				}
				if (_gamepadState[i].DPad.Down == ButtonState.Pressed)
				{
					_gamepadButtons[i].Add(Buttons.GamepadDown);
				}
	
				if (_gamepadState[i].Buttons.A == ButtonState.Pressed)
				{
					_gamepadButtons[i].Add(Buttons.GamepadA);
				}
				if (_gamepadState[i].Buttons.B == ButtonState.Pressed)
				{
					_gamepadButtons[i].Add(Buttons.GamepadB);
				}
				if (_gamepadState[i].Buttons.X == ButtonState.Pressed)
				{
					_gamepadButtons[i].Add(Buttons.GamepadX);
				}
				if (_gamepadState[i].Buttons.Y == ButtonState.Pressed)
				{
					_gamepadButtons[i].Add(Buttons.GamepadY);
				}
	
				if (_gamepadState[i].Buttons.LeftShoulder == ButtonState.Pressed)
				{
					_gamepadButtons[i].Add(Buttons.GamepadLB);
				}
				if (_gamepadState[i].Buttons.RightShoulder == ButtonState.Pressed)
				{
					_gamepadButtons[i].Add(Buttons.GamepadRB);
				}
				
				if (_gamepadState[i].Triggers.Left > GamepadTriggersDeadzone)
				{
					_gamepadButtons[i].Add(Buttons.GamepadLT);
				}
				if (_gamepadState[i].Triggers.Right > GamepadTriggersDeadzone)
				{
					_gamepadButtons[i].Add(Buttons.GamepadRT);
				}

				if (_gamepadState[i].Buttons.LeftStick == ButtonState.Pressed)
				{
					_gamepadButtons[i].Add(Buttons.GamepadLS);
				}
				if (_gamepadState[i].Buttons.RightStick == ButtonState.Pressed)
				{
					_gamepadButtons[i].Add(Buttons.GamepadRS);
				}

				if (_gamepadState[i].Buttons.Start == ButtonState.Pressed)
				{
					_gamepadButtons[i].Add(Buttons.GamepadStart);
				}
				if (_gamepadState[i].Buttons.Back == ButtonState.Pressed)
				{
					_gamepadButtons[i].Add(Buttons.Select);
				}
			}

			#endregion Gamepad.
			
		}


		#region Button checks.

		/// <summary>
		/// Checks if mouse, keyboard or gamepad button is down.
		/// </summary>
		/// <param name="button">Button to check.</param>
		/// <param name="index">Device index. Used for gamepad only.</param>
		public static bool CheckButton(Buttons button, int index = 0)
		{
			var buttonCode = (int)button;

			var result = 
				index < _gamepadButtons.Length   && !_gamepadCleared  && _gamepadButtons[0].Contains(button)
				|| buttonCode < _keyboardMaxCode && !_keyboardCleared && _currentKeys.Contains((Keys)button)
				|| buttonCode < _mouseMaxCode    && !_mouseCleared    && _mouseButtons.Contains(button);
			
			return result;
		}


		/// <summary>
		/// Checks if mouse, keyboard or gamepad button is pressed.
		/// </summary>
		/// <param name="button">Button to check.</param>
		/// <param name="index">Device index. Used for gamepad only.</param>
		public static bool CheckButtonPress(Buttons button, int index = 0)
		{
			var buttonCode = (int)button;

			var result = 
				index < _gamepadButtons.Length   && !_gamepadCleared  && _gamepadButtons[0].Contains(button) && _previousGamepadButtons[0].Contains(button)
				|| buttonCode < _keyboardMaxCode && !_keyboardCleared && _currentKeys.Contains((Keys)button) && !_previousKeys.Contains((Keys)button)
				|| buttonCode < _mouseMaxCode    && !_mouseCleared    && _mouseButtons.Contains(button)      && !_previousMouseButtons.Contains(button);
			
			return result;
		}


		/// <summary>
		/// Checks if mouse, keyboard or gamepad button is released.
		/// </summary>
		/// <param name="button">Button to check.</param>
		/// <param name="index">Device index. Used for gamepad only.</param>
		public static bool CheckButtonRelease(Buttons button, int index = 0)
		{
			var buttonCode = (int)button;

			var result = 
				index < _gamepadButtons.Length   && !_gamepadCleared  && !_gamepadButtons[0].Contains(button) && _previousGamepadButtons[0].Contains(button)
				|| buttonCode < _keyboardMaxCode && !_keyboardCleared && !_currentKeys.Contains((Keys)button) && _previousKeys.Contains((Keys)button)
				|| buttonCode < _mouseMaxCode    && !_mouseCleared    && !_mouseButtons.Contains(button)      && _previousMouseButtons.Contains(button);
			
			return result;
		}

		#endregion Button checks.
		


		#region Mouse.

		/// <summary>
		/// Clears mouse input.
		/// </summary>
		public static void ClearMouseInput() =>
			_mouseCleared = true;
		
		#endregion Mouse.



		#region Keyboard.

		/// <summary>
		/// Checks if any keyboard key in down in current step.
		/// </summary>
		public static bool KeyboardCheckAnyKey() => 
			!_keyboardCleared && _currentKeys.Length > 0;
		

		/// <summary>
		/// Checks if any keyboard key in pressed.
		/// </summary>
		public static bool KeyboardCheckAnyKeyPress() => 
			!_keyboardCleared && _currentKeys.Length > 0 && _previousKeys.Length == 0;


		/// <summary>
		/// Checks if any keyboard key in released.
		/// </summary>
		public static bool KeyboardCheckAnyKeyRelease() => 
			!_keyboardCleared && _currentKeys.Length == 0 && _previousKeys.Length > 0;


		/// <summary>
		/// Clears keyboard input.
		/// </summary>
		public static void ClearKeyboardInput() =>
			_keyboardCleared = true;


		/// <summary>
		/// Text input event. Occurs when any key is pressed.
		/// Works asynchroniously.
		/// Assigned to Window.TextInput of Game1 class.
		/// </summary>
		public static void TextInput(object sender, TextInputEventArgs e)
		{
			_keyboardBuffer.Append(e.Character);
			_keyboardLastKeyBuffer = e.Key;
		}

		#endregion Keyboard.



		#region Gamepad.

		internal static void GamepadInit()
		{
			// Creating a bunch of dummy objects just to get rid of null ref exception.
			_gamepadButtons = new List<Buttons>[_maxGamepadCount];
			for(var i = 0; i < _gamepadButtons.Length; i += 1)
			{
				_gamepadButtons[i] = new List<Buttons>();
			}
			_previousGamepadButtons = _gamepadButtons;

			_gamepadState = new GamePadState[_maxGamepadCount];
			for(var i = 0; i < _gamepadState.Length; i += 1)
			{
				_gamepadState[i] = new GamePadState();
			}
			_previousGamepadState = _gamepadState;			
		}


		/// <summary>
		/// Checks if gamepad with given inex is connected.
		/// </summary>
		public static bool GamepadConnected(int index)
		{
			if (!_gamepadCleared && index < _gamepadState.Length)
			{
				return _gamepadState[index].IsConnected;
			}
			return false;
		}


		/// <summary>
		/// Returns vector of left thumb stick.
		/// </summary>
		public static Vector2 GamepadGetLeftStick(int index)
		{
			if (!_gamepadCleared && index < _gamepadState.Length)
			{
				return _gamepadState[index].ThumbSticks.Left;
			}
			return Vector2.Zero;
		}

		
		/// <summary>
		/// Returns vector of right thumb stick.
		/// </summary>
		public static Vector2 GamepadGetRightStick(int index)
		{
			if (!_gamepadCleared && index < _gamepadState.Length)
			{
				return _gamepadState[index].ThumbSticks.Right;
			}
			return Vector2.Zero;
		}

		
		/// <summary>
		/// Returns value of pressure on left trigger.
		/// NOTE: If you don't need exact pressure value, use GamepadCheck* functions.
		/// </summary>
		public static float GamepadGetLeftTrigger(int index)
		{
			if (!_gamepadCleared && index < _gamepadState.Length)
			{
				return _gamepadState[index].Triggers.Left;
			}
			return 0;
		}


		/// <summary>
		/// Returns value of pressure on right trigger.
		/// NOTE: If you don't need exact pressure value, use GamepadCheck* functions.
		/// </summary>
		public static float GamepadGetRightTrigger(int index)
		{
			if (!_gamepadCleared && index < _gamepadState.Length)
			{
				return _gamepadState[index].Triggers.Right;
			}
			return 0;
		}

		/// <summary>
		/// Sets vibration to the given gamepad.
		/// </summary>
		public static void GamepadSetVibration(int index, float leftMotor, float rightMotor) => 
			GamePad.SetVibration(index, leftMotor, rightMotor);
		
		/// <summary>
		/// Clears gamepad input, including triggers and thumb sticks.
		/// </summary>
		public static void ClearGamepadInput() =>
			_gamepadCleared = true;
		
		#endregion Gamepad.

		

		/// <summary>
		/// Clears mouse, keyboard and gamepad input.
		/// </summary>
		public static void ClearInput()
		{
			ClearMouseInput();
			ClearKeyboardInput();
			ClearGamepadInput();
		}
	}
}
