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
		
		public static Effect Effect;
		public static Effect BW;
		
		public static void Load()
		{
			_content = new ContentManager(GameMgr.Game.Services);
			_content.RootDirectory = AssetMgr.ContentDir + '/' + AssetMgr.EffectsDir;
			
			Effect = _content.Load<Effect>("effect");
			BW = _content.Load<Effect>("BW");
		}

		public static void Unload()
		{
			_content.Unload();
		}

	}
}