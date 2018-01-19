using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
//using System.Windows.Forms;
using System.Diagnostics;
using System;
using Monofoxe.Engine;

namespace Monofoxe
{
	/// <summary>
	/// This is the main type for your game.
	/// </summary>
	public class Game1 : Game
	{
		public GraphicsDeviceManager graphics;
		public static Texture2D tex;
		public static SpriteBatch spriteBatch;
		public static SpriteBatch spriteBatch1;
		
		
		public static RenderTarget2D surf;

		public static Matrix Transform;

		public Game1()
		{
			graphics = new GraphicsDeviceManager(this);
			Content.RootDirectory = "Content";
		}

		/// <summary>
		/// Allows the game to perform any initialization it needs to before starting to run.
		/// This is where it can query for any required services and load any non-graphic
		/// related content.  Calling base.Initialize will enumerate through any components
		/// and initialize them as well.
		/// </summary>
		protected override void Initialize()
		{
			// TODO: Add your initialization logic here
			
			GameCntrl.MyGame = this;
			
			GameCntrl.MaxGameSpeed = 60.0;

			Debug.Write(GameCntrl.MaxGameSpeed);
			
			Window.TextInput += Input.TextInput;

			
			surf = new RenderTarget2D(GraphicsDevice, 64, 64);

			GraphicsDevice.SetRenderTarget(surf); 
			GraphicsDevice.Clear(Color.AntiqueWhite);
			GraphicsDevice.SetRenderTarget(null);
	

			base.Initialize();
		}

		/// <summary>
		/// LoadContent will be called once per game and is the place to load
		/// all of your content.
		/// </summary>
		protected override void LoadContent()
		{
			spriteBatch = new SpriteBatch(GraphicsDevice);
			spriteBatch1 = new SpriteBatch(GraphicsDevice);
		
			tex = Content.Load<Texture2D>("derp");
		
			DrawCntrl.Init(GraphicsDevice, spriteBatch);

			new TestObj();			
			new GameObj();	
		
		}

		/// <summary>
		/// UnloadContent will be called once per game and is the place to unload
		/// game-specific content.
		/// </summary>
		protected override void UnloadContent()
		{
			// TODO: Unload any non ContentManager content here
		}

		/// <summary>
		/// Allows the game to run logic such as updating the world,
		/// checking for collisions, gathering input, and playing audio.
		/// </summary>
		/// <param name="gameTime">Provides a snapshot of timing values.</param>
		protected override void Update(GameTime gameTime)
		{
			if (Keyboard.GetState().IsKeyDown(Keys.Escape))
			{Exit();}
			
			Input.Update();
			GameCntrl.Update(gameTime);
			
			Objects.Update(gameTime);
			
			base.Update(gameTime);
		}

		/// <summary>
		/// This is called when the game should draw itself.
		/// </summary>
		/// <param name="gameTime">Provides a snapshot of timing values.</param>
		protected override void Draw(GameTime gameTime)
		{
			GraphicsDevice.Clear(Color.CornflowerBlue);
			
			Objects.Draw();

			base.Draw(gameTime);
		}
	}
}
