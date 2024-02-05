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
		/// Size of the screen.
		/// </summary>
		public Vector2 ScreenSize => new Vector2(GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Width, GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Height);

		
		/// <summary>
		/// Tells how canvas will be drawn on the backbuffer.
		/// </summary>
		public CanvasMode CanvasMode = CanvasMode.KeepAspectRatio;

		/// <summary>
		/// Window size.
		/// </summary>
		public Vector2 CanvasSize 
		{
			get => new Vector2(_canvasWidth, _canvasHeight);
			set 
			{
				_canvasWidth = (int)value.X;
				_canvasHeight = (int)value.Y;
				if (!IsFullScreen)
				{
					PreferredBackBufferWidth = _canvasWidth;
					PreferredBackBufferHeight = _canvasHeight;
				}
			}
		}
		private int _canvasWidth;
		private int _canvasHeight;



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
				PreferredBackBufferWidth = (int)ScreenSize.X;
				PreferredBackBufferHeight = (int)ScreenSize.Y;
			}
			else
			{
				PreferredBackBufferWidth = _canvasWidth;
				PreferredBackBufferHeight = _canvasHeight;
			}
			
			IsFullScreen = fullscreen;
			ApplyChanges();
		}

		
		public new void ToggleFullScreen() =>
			SetFullScreen(!IsFullScreen);


		/// <summary>
		/// Centers game window on the screen.
		/// </summary>
		public void CenterWindow() => 
			WindowPosision = ((ScreenSize - CanvasSize) / 2).ToPoint();
	}
}
