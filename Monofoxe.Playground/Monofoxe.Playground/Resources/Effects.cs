using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Resources.Sprites;
using Monofoxe.Engine.Drawing;
using Monofoxe.Engine;

namespace Resources
{
	public static class Effects
	{
		private static ContentManager _content;

		public static Effect Grayscale;

		public static void Load()
		{
			_content = new ContentManager(GameMgr.Game.Services);
			_content.RootDirectory = AssetMgr.ContentDir + '/' + AssetMgr.EffectsDir;

			// This is not generated automatically. Sadly, you'll have to add those by hand.
			Grayscale = _content.Load<Effect>("Grayscale");

		}

		public static void Unload()
		{
			_content.Unload();
		}

	}
}