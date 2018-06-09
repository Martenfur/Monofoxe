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
		public static Effect effect;

		public Game1()
		{
			Content.RootDirectory = GameCntrl.ContentDir;
			GameCntrl.Init(this);
		}

		/// <summary>
		/// Allows the game to perform any initialization it needs to before starting to run.
		/// This is where it can query for any required services and load any non-graphic
		/// related content.  Calling base.Initialize will enumerate through any components
		/// and initialize them as well.
		/// </summary>
		protected override void Initialize()
		{
			base.Initialize();
			
			new TestObj();
		}

		/// <summary>
		/// LoadContent will be called once per game and is the place to load
		/// all of your content.
		/// </summary>
		protected override void LoadContent()
		{
			GameCntrl.LoadGraphics(Content);	
			Fonts.Load(Content);
			DrawCntrl.Init(GraphicsDevice);
			
			effect = Content.Load<Effect>("Effects/effect");
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
