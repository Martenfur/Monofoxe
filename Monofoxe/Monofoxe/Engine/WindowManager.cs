using System.Diagnostics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Monofoxe.Engine
{
	/// <summary>
	/// Merging of GraphicsDeviveManager and WindowManager.
	/// </summary>
	public class WindowManager : GraphicsDeviceManager
	{
		#region Window properties
		/// <summary>
		/// Width of the screen.
		/// </summary>
		public int ScreenW {get => GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Width;}

		/// <summary>
		/// Height of the screen.
		/// </summary>
		public int ScreenH {get => GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Height;}

		/// <summary>
		/// Size of the screen.
		/// </summary>
		public Vector2 ScreenSize {get => new Vector2(ScreenW, ScreenH);}

		/// <summary>
		/// Window width.
		/// </summary>
		public int WindowW
		{
			get => _windowW;
			set
			{
				_windowW = value;
				if (!IsFullScreen)
				{
					PreferredBackBufferWidth = _windowW;
				}
			}
		}
		private int _windowW;

		/// <summary>
		/// Window height.
		/// </summary>
		public int WindowH
		{
			get => _windowH;
			set
			{
				_windowH = value;
				if (!IsFullScreen)
				{
					PreferredBackBufferHeight = _windowH;
				}
			}
		}
		private int _windowH;
		
		/// <summary>
		/// Window size.
		/// </summary>
		public Vector2 WindowSize 
		{
			get => new Vector2(WindowW, WindowH);
			set 
			{
				WindowW = (int)value.X;
				WindowH = (int)value.Y;
			}
		}

		/// <summary>
		/// Window position.
		/// </summary>
		public Point WindowPos
		{
			get => Window.Position;
			set => Window.Position = value;
		}

		/// <summary>
		/// Title of the window.
		/// </summary>
		public string Title
		{
			get => Window.Title;
			set => Window.Title = value;
		}

		/// <summary>
		/// Allowing borders in window.
		/// </summary>
		public bool IsBorderless
		{
			get => Window.IsBorderless;
			set => Window.IsBorderless = value;
		}
		#endregion Window properties

		/// <summary>
		/// Game window class. 
		/// Most of its features are handled through WindowManager,
		/// but if you want something specific, use this.
		/// </summary>
		public GameWindow Window {get;}

		public WindowManager(Game game) : base(game)
		{
			Window = game.Window;
			_windowW = PreferredBackBufferWidth;
			_windowH = PreferredBackBufferHeight;
		}

		public void SetFullScreen(bool fullscreen)
		{
			if (fullscreen)
			{
				PreferredBackBufferWidth = ScreenW;
				PreferredBackBufferHeight = ScreenH;
			}
			else
			{
				PreferredBackBufferWidth = _windowW;
				PreferredBackBufferHeight = _windowH;
			}
			
			IsFullScreen = fullscreen;
			ApplyChanges();
		}


		/// <summary>
		/// Centers game window on the screen.
		/// </summary>
		public void CenterWindow() 
		=> WindowPos = ((ScreenSize - WindowSize) / 2).ToPoint();
		


	}
}
