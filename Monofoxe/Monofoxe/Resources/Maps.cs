using Microsoft.Xna.Framework.Content;
using Monofoxe.Engine;
using Monofoxe.Tiled;
using Monofoxe.Tiled.MapStructure;


namespace Resources
{
	public static class Maps
	{
		private static ContentManager _content;
		
		public static MapBuilder Test;
		
		public static void Load()
		{
			_content = new ContentManager(GameMgr.Game.Services);
			_content.RootDirectory = AssetMgr.ContentDir + '/' + AssetMgr.MapsDir;
			
			Test = new MapBuilder(_content.Load<TiledMap>("test"));
		}

		public static void Unload()
		{
			_content.Unload();
		}

	}
}
