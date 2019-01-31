using Microsoft.Xna.Framework;
using Monofoxe.Engine;
using Monofoxe.Engine.ECS;
using Monofoxe.FMODAudio;
using Monofoxe.Tiled;

namespace Monofoxe.Demo
{
	/// <summary>
	/// This is the main type for your game.
	/// </summary>
	public class Game1 : Game
	{
		public Game1()
		{
			Content.RootDirectory = AssetMgr.ContentDir;
			GameMgr.Init(this);
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
			
			AudioMgr.Init();
			MapMgr.Init();

			//new Test();
		}

		/// <summary>
		/// LoadContent will be called once per game and is the place to load
		/// all of your content.
		/// </summary>
		protected override void LoadContent()
		{
			DrawMgr.Init(GraphicsDevice);
			EntityMgr.LoadEntityTemplates();
			//189
			//114
			Resources.Sprites.SpritesDefault.Load();	
			Resources.Fonts.Load();
			Resources.Effects.Load();
			Resources.Maps.Load();
			
		}

		/// <summary>
		/// UnloadContent will be called once per game and is the place to unload
		/// game-specific content.
		/// </summary>
		protected override void UnloadContent()
		{
			AudioMgr.Unload();
		}

		/// <summary>
		/// Allows the game to run logic such as updating the world,
		/// checking for collisions, gathering input, and playing audio.
		/// </summary>
		/// <param name="gameTime">Provides a snapshot of timing values.</param>
		protected override void Update(GameTime gameTime)
		{
			GameMgr.Update(gameTime);
			AudioMgr.Update();
			
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
