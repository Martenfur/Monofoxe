using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
//using System.Windows.Forms;
using System.Diagnostics;
using System;
using Monofoxe.Engine;
using Monofoxe.Engine.Drawing;
using System.Collections.Generic;
using System.IO;

namespace Monofoxe
{
	/// <summary>
	/// This is the main type for your game.
	/// </summary>
	public class Game1 : Game
	{
		public GraphicsDeviceManager graphics;
		
		public static SpriteFont Def;

		public Game1()
		{
			graphics = new GraphicsDeviceManager(this);
			Content.RootDirectory = GameCntrl.ContentDir;
		}

		/// <summary>
		/// Allows the game to perform any initialization it needs to before starting to run.
		/// This is where it can query for any required services and load any non-graphic
		/// related content.  Calling base.Initialize will enumerate through any components
		/// and initialize them as well.
		/// </summary>
		protected override void Initialize()
		{
			GameCntrl.MyGame = this;
			
			GameCntrl.MaxGameSpeed = 60.0;
			
			Window.TextInput += Input.TextInput;

			base.Initialize();
		}

		/// <summary>
		/// LoadContent will be called once per game and is the place to load
		/// all of your content.
		/// </summary>
		protected override void LoadContent()
		{
			GameCntrl.LoadGraphics(Content);			
			DrawCntrl.Init(GraphicsDevice);
			
			Def = Content.Load<SpriteFont>("def"); 

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
			{
				Exit();
			}

			GameCntrl.Update(gameTime);
			
			base.Update(gameTime);
		}

		/// <summary>
		/// This is called when the game should draw itself.
		/// </summary>
		/// <param name="gameTime">Provides a snapshot of timing values.</param>
		protected override void Draw(GameTime gameTime)
		{
			GameCntrl.UpdateFps(gameTime);
			DrawCntrl.Update(gameTime);

			base.Draw(gameTime);
		}
	}
}
