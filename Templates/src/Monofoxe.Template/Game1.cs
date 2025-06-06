﻿using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Monofoxe.Engine;
using Monofoxe.Engine.Drawing;
using Monofoxe.Engine.Resources;
using Monofoxe.Resources;
using Monofoxe.Template.Resources;
using Monofoxe.Tiled;
using Monofoxe.Tiled.MapStructure;

namespace Monofoxe.Template
{
	/// <summary>
	/// This is the main type for your game.
	/// </summary>
	public class Game1 : Game
	{
		public Game1()
		{
			Content.RootDirectory = ResourceInfoMgr.ContentDir;
			GameMgr.Init(this);

			if (GameMgr.CurrentPlatform == Platform.Android)
			{
				GameMgr.WindowManager.SetFullScreen(true); // Has to be exactly here, apparently.
			}

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
			TiledEntityFactoryPool.InitFactoryPool();

			new GameController();
		}

		/// <summary>
		/// LoadContent will be called once per game and is the place to load
		/// all of your content.
		/// </summary>
		protected override void LoadContent()
		{
			GraphicsMgr.Init(GraphicsDevice);

			new SpriteGroupResourceBox("DefaultSprites", "Graphics/Default");
			new DirectoryResourceBox<Effect>("Effects", "Effects");
			new DirectoryResourceBox<TiledMap>("Maps", "Maps");
			new Fonts();
		}

		/// <summary>
		/// UnloadContent will be called once per game and is the place to unload
		/// game-specific content.
		/// </summary>
		protected override void UnloadContent()
		{
			ResourceHub.UnloadAll();
		}

		/// <summary>
		/// Allows the game to run logic such as updating the world,
		/// checking for collisions, gathering input, and playing audio.
		/// </summary>
		/// <param name="gameTime">Provides a snapshot of timing values.</param>
		protected override void Update(GameTime gameTime)
		{
			GameMgr.Update(gameTime);

			base.Update(gameTime);
		}

		/// <summary>
		/// This is called when the game should draw itself.
		/// </summary>
		/// <param name="gameTime">Provides a snapshot of timing values.</param>
		protected override void Draw(GameTime gameTime)
		{
			GameMgr.Draw(gameTime);

			base.Draw(gameTime);
		}
	}
}
