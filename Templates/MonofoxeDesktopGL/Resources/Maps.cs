using Microsoft.Xna.Framework.Content;
using Monofoxe.Engine;
using Monofoxe.Tiled;


namespace Resources
{
	public static class Maps
	{
		private static ContentManager _content;
		
		
		public static void Load()
		{
			_content = new ContentManager(GameMgr.Game.Services);
			_content.RootDirectory = AssetMgr.ContentDir + '/' + AssetMgr.MapsDir;
			
		}

		public static void Unload()
		{
			_content.Unload();
		}

	}
}
