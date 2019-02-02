using Microsoft.Xna.Framework.Content;
using Monofoxe.Engine;
using Monofoxe.Tiled.MapStructure;


namespace Resources
{
	public static class Maps
	{
		private static ContentManager _content;
		
		public static TiledMap Test;
		
		public static void Load()
		{
			_content = new ContentManager(GameMgr.Game.Services);
			_content.RootDirectory = AssetMgr.ContentDir + '/' + AssetMgr.MapsDir;
			
			Test = _content.Load<TiledMap>("test");
			System.Console.WriteLine(Test == null);
		}

		public static void Unload()
		{
			_content.Unload();
		}

	}
}
