using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Monofoxe.Engine
{
	/// <summary>
	/// Merging of GraphicsDeviveManager and WindowManager.
	/// </summary>
	public class WindowMgr : GraphicsDeviceManager
	{
		// NOTE: To control VSync, use SynchronizeWithVerticalRetrace.

		#region Window properties.

		/// <summary>
		/// Width of the screen.
		/// </summary>
		public int ScreenWidth => GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Width;

		/// <summary>
		/// Height of the screen.
		/// </summary>
		public int ScreenHeight => GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Height;

		/// <summary>
		/// Size of the screen.
		/// </summary>
		public Vector2 ScreenSize => new Vector2(ScreenWidth, ScreenHeight);

		/// <summary>
		/// Window width.
		/// </summary>
		public int CanvasWidth
		{
			get => _canvasWidth;
			set
			{
				_canvasWidth = value;
				if (!IsFullScreen)
				{
					PreferredBackBufferWidth = _canvasWidth;
				}
			}
		}
		private int _canvasWidth;

		/// <summary>
		/// Window height.
		/// </summary>
		public int CanvasHeight
		{
			get => _canvasHeight;
			set
			{
				_canvasHeight = value;
				if (!IsFullScreen)
				{
					PreferredBackBufferHeight = _canvasHeight;
				}
			}
		}
		private int _canvasHeight;
		
		/// <summary>
		/// Tells how canvas will be drawn on the backbuffer.
		/// </summary>
		public CanvasMode CanvasMode = CanvasMode.KeepAspectRatio;

		/// <summary>
		/// Window size.
		/// </summary>
		public Vector2 CanvasSize 
		{
			get => new Vector2(CanvasWidth, CanvasHeight);
			set 
			{
				CanvasWidth = (int)value.X;
				CanvasHeight = (int)value.Y;
			}
		}



		/// <summary>
		/// Window position.
		/// </summary>
		public Point WindowPosision
		{
			get => Window.Position;
			set => Window.Position = value;
		}

		/// <summary>
		/// Title of the window.
		/// </summary>
		public string WindowTitle
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

		#endregion Window properties.

		/// <summary>
		/// Game window class. 
		/// Most of its features are handled through WindowManager,
		/// but if you want something specific, use this.
		/// </summary>
		public GameWindow Window {get;}

		public WindowMgr(Game game) : base(game)
		{
			Window = game.Window;
			_canvasWidth = PreferredBackBufferWidth;
			_canvasHeight = PreferredBackBufferHeight;
		}

		public void SetFullScreen(bool fullscreen)
		{
			if (fullscreen)
			{
				PreferredBackBufferWidth = ScreenWidth;
				PreferredBackBufferHeight = ScreenHeight;
			}
			else
			{
				PreferredBackBufferWidth = _canvasWidth;
				PreferredBackBufferHeight = _canvasHeight;
			}
			
			IsFullScreen = fullscreen;
			ApplyChanges();
		}


		/// <summary>
		/// Centers game window on the screen.
		/// </summary>
		public void CenterWindow() => 
			WindowPosision = ((ScreenSize - CanvasSize) / 2).ToPoint();
		


	}
}
