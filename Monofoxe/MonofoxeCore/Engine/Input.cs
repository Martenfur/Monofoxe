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
	/// Combined enums from keyboard, gamepad and mouse.
	/// I really don't like the idea of 3 separate sets of functions for each input method.
	/// Standard Xna.Input.Keys can be casted into Buttons.
	/// </summary>
	public enum Buttons
	{
		#region Keyboard.
		None = 0,
		Back = 8,
		Tab = 9,
		Enter = 13,
		Pause = 19,
		CapsLock = 20,
		Kana = 21,
		Kanji = 25,
		Escape = 27,
		ImeConvert = 28,
		ImeNoConvert = 29,
		Space = 32,
		PageUp = 33,
		PageDown = 34,
		End = 35,
		Home = 36,
		Left = 37,
		Up = 38,
		Right = 39,
		Down = 40,
		Select = 41,
		Print = 42,
		Execute = 43,
		PrintScreen = 44,
		Insert = 45,
		Delete = 46,
		Help = 47,
		D0 = 48,
		D1 = 49,
		D2 = 50,
		D3 = 51,
		D4 = 52,
		D5 = 53,
		D6 = 54,
		D7 = 55,
		D8 = 56,
		D9 = 57,
		A = 65,
		B = 66,
		C = 67,
		D = 68,
		E = 69,
		F = 70,
		G = 71,
		H = 72,
		I = 73,
		J = 74,
		K = 75,
		L = 76,
		M = 77,
		N = 78,
		O = 79,
		P = 80,
		Q = 81,
		R = 82,
		S = 83,
		T = 84,
		U = 85,
		V = 86,
		W = 87,
		X = 88,
		Y = 89,
		Z = 90,
		LeftWindows = 91,
		RightWindows = 92,
		Apps = 93,
		Sleep = 95,
		NumPad0 = 96,
		NumPad1 = 97,
		NumPad2 = 98,
		NumPad3 = 99,
		NumPad4 = 100,
		NumPad5 = 101,
		NumPad6 = 102,
		NumPad7 = 103,
		NumPad8 = 104,
		NumPad9 = 105,
		Multiply = 106,
		Add = 107,
		Separator = 108,
		Subtract = 109,
		Decimal = 110,
		Divide = 111,
		F1 = 112,
		F2 = 113,
		F3 = 114,
		F4 = 115,
		F5 = 116,
		F6 = 117,
		F7 = 118,
		F8 = 119,
		F9 = 120,
		F10 = 121,
		F11 = 122,
		F12 = 123,
		F13 = 124,
		F14 = 125,
		F15 = 126,
		F16 = 127,
		F17 = 128,
		F18 = 129,
		F19 = 130,
		F20 = 131,
		F21 = 132,
		F22 = 133,
		F23 = 134,
		F24 = 135,
		NumLock = 144,
		Scroll = 145,
		LeftShift = 160,
		RightShift = 161,
		LeftControl = 162,
		RightControl = 163,
		LeftAlt = 164,
		RightAlt = 165,
		BrowserBack = 166,
		BrowserForward = 167,
		BrowserRefresh = 168,
		BrowserStop = 169,
		BrowserSearch = 170,
		BrowserFavorites = 171,
		BrowserHome = 172,
		VolumeMute = 173,
		VolumeDown = 174,
		VolumeUp = 175,
		MediaNextTrack = 176,
		MediaPreviousTrack = 177,
		MediaStop = 178,
		MediaPlayPause = 179,
		LaunchMail = 180,
		SelectMedia = 181,
		LaunchApplication1 = 182,
		LaunchApplication2 = 183,
		OemSemicolon = 186,
		OemPlus = 187,
		OemComma = 188,
		OemMinus = 189,
		OemPeriod = 190,
		OemQuestion = 191,
		OemTilde = 192,
		ChatPadGreen = 202,
		ChatPadOrange = 203,
		OemOpenBrackets = 219,
		OemPipe = 220,
		OemCloseBrackets = 221,
		OemQuotes = 222,
		Oem8 = 223,
		OemBackslash = 226,
		ProcessKey = 229,
		OemCopy = 242,
		OemAuto = 243,
		OemEnlW = 244,
		Attn = 246,
		Crsel = 247,
		Exsel = 248,
		EraseEof = 249,
		Play = 250,
		Zoom = 251,
		Pa1 = 253,
		OemClear = 254,
		#endregion Keyboard.


		#region Mouse.
		MouseLeft = 1000,
		MouseRight = 1001,
		MouseMiddle = 1002,
		#endregion Mouse.


		#region Gamepad.
		GpLeft = 2000,
		GpRight = 2001,
		GpUp = 2002,
		GpDown = 2003,
		GpA = 2004,
		GpB = 2005,
		GpX = 2006,
		GpY = 2007,
		GpLT = 2008, // Left trigger.
		GpRT = 2009, // Right trigger.
		GpLB = 2010, // Left button.
		GpRB = 2011, // Right button.
		GpLS = 2012, // Left stick.
		GpRS = 2013, // Right stick.
		GpStart = 2014,
		GpSelect = 2015,
		#endregion Gamepad.
	}


	public static class Input
	{
		#region Mouse.
	
		/// <summary>
		/// Cursor position on screen.
		/// </summary>
		public static Vector2 ScreenMousePos {get; private set;} = Vector2.Zero;
		
		/// <summary>
		/// Cursor position in the world. Depends on current camera.
		/// </summary>
		public static Vector2 MousePos {get; private set;} = Vector2.Zero;


		private static List<Buttons> _mouseButtons = new List<Buttons>(),
			_previousMouseButtons = new List<Buttons>();
		

		/// <summary>
		/// Scrollwheel value. Can be -1, 0 or 1.
		/// </summary>
		public static int MouseWheelVal {get; private set;}
		private static int _mouseWheelAdditionPrev;

		#endregion Mouse.


		#region Keyboard.

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
			
			var m = Matrix.Invert(DrawCntrl.CanvasMatrix);
			var buffer = Vector3.Transform(new Vector3(mouseState.X, mouseState.Y, 0), m);
			ScreenMousePos = new Vector2(buffer.X, buffer.Y);
			
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
			Thank you, XNA devs. You made me write even more code! ^0^
			*/
			MouseWheelVal = Math.Sign(_mouseWheelAdditionPrev - mouseState.ScrollWheelValue);
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
			
				if (_gamepadState[i].DPad.Left == ButtonState.Pressed)
				{
					_gamepadButtons[i].Add(Buttons.GpLeft);
				}
				if (_gamepadState[i].DPad.Right == ButtonState.Pressed)
				{
					_gamepadButtons[i].Add(Buttons.GpRight);
				}
				if (_gamepadState[i].DPad.Up == ButtonState.Pressed)
				{
					_gamepadButtons[i].Add(Buttons.GpUp);
				}
				if (_gamepadState[i].DPad.Down == ButtonState.Pressed)
				{
					_gamepadButtons[i].Add(Buttons.GpDown);
				}
	
				if (_gamepadState[i].Buttons.A == ButtonState.Pressed)
				{
					_gamepadButtons[i].Add(Buttons.GpA);
				}
				if (_gamepadState[i].Buttons.B == ButtonState.Pressed)
				{
					_gamepadButtons[i].Add(Buttons.GpB);
				}
				if (_gamepadState[i].Buttons.X == ButtonState.Pressed)
				{
					_gamepadButtons[i].Add(Buttons.GpX);
				}
				if (_gamepadState[i].Buttons.Y == ButtonState.Pressed)
				{
					_gamepadButtons[i].Add(Buttons.GpY);
				}
	
				if (_gamepadState[i].Buttons.LeftShoulder == ButtonState.Pressed)
				{
					_gamepadButtons[i].Add(Buttons.GpLB);
				}
				if (_gamepadState[i].Buttons.RightShoulder == ButtonState.Pressed)
				{
					_gamepadButtons[i].Add(Buttons.GpRB);
				}
				
				if (_gamepadState[i].Triggers.Left > GamepadTriggersDeadzone)
				{
					_gamepadButtons[i].Add(Buttons.GpLT);
				}
				if (_gamepadState[i].Triggers.Right > GamepadTriggersDeadzone)
				{
					_gamepadButtons[i].Add(Buttons.GpRT);
				}

				if (_gamepadState[i].Buttons.LeftStick == ButtonState.Pressed)
				{
					_gamepadButtons[i].Add(Buttons.GpLS);
				}
				if (_gamepadState[i].Buttons.RightStick == ButtonState.Pressed)
				{
					_gamepadButtons[i].Add(Buttons.GpRS);
				}

				if (_gamepadState[i].Buttons.Start == ButtonState.Pressed)
				{
					_gamepadButtons[i].Add(Buttons.GpStart);
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
		public static void MouseClear() =>
			_mouseCleared = true;


		/// <summary>
		/// Updates mouse position in the world.
		/// </summary>
		public static void UpdateMouseWorldPosition()
		{
			var m = Matrix.Invert(DrawCntrl.CurrentTransformMatrix);
			var buffer = Vector3.Transform(new Vector3(ScreenMousePos.X, ScreenMousePos.Y, 0), m);
			MousePos = new Vector2(buffer.X, buffer.Y) - DrawCntrl.CurrentCamera.PortPos;
		}

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
		public static void KeyboardClear() =>
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
		public static void GamepadClear() =>
			_gamepadCleared = true;
		#endregion Gamepad.



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
