using System.Diagnostics;
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
		public int ScreenW => GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Width;

		/// <summary>
		/// Height of the screen.
		/// </summary>
		public int ScreenH => GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Height;

		/// <summary>
		/// Size of the screen.
		/// </summary>
		public Vector2 ScreenSize => new Vector2(ScreenW, ScreenH);

		/// <summary>
		/// Window width.
		/// </summary>
		public int CanvasW
		{
			get => _canvasW;
			set
			{
				_canvasW = value;
				if (!IsFullScreen)
				{
					PreferredBackBufferWidth = _canvasW;
				}
			}
		}
		private int _canvasW;

		/// <summary>
		/// Window height.
		/// </summary>
		public int CanvasH
		{
			get => _canvasH;
			set
			{
				_canvasH = value;
				if (!IsFullScreen)
				{
					PreferredBackBufferHeight = _canvasH;
				}
			}
		}
		private int _canvasH;
		
		public CanvasMode CanvasMode = CanvasMode.KeepAspectRatio;

		/// <summary>
		/// Window size.
		/// </summary>
		public Vector2 CanvasSize 
		{
			get => new Vector2(CanvasW, CanvasH);
			set 
			{
				CanvasW = (int)value.X;
				CanvasH = (int)value.Y;
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
			_canvasW = PreferredBackBufferWidth;
			_canvasH = PreferredBackBufferHeight;
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
				PreferredBackBufferWidth = _canvasW;
				PreferredBackBufferHeight = _canvasH;
			}
			
			IsFullScreen = fullscreen;
			ApplyChanges();
		}


		/// <summary>
		/// Centers game window on the screen.
		/// </summary>
		public void CenterWindow() => 
			WindowPos = ((ScreenSize - CanvasSize) / 2).ToPoint();
		


	}
}
