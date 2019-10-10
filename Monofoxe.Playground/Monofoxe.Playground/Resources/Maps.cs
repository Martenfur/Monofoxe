using Microsoft.Xna.Framework.Content;
using Monofoxe.Engine;
using Monofoxe.Engine.Resources;
using Monofoxe.Tiled.MapStructure;


namespace Resources
{
	public class Maps : ResourceBox<TiledMap>
	{
		private static ContentManager _content;
		
		public override string Name => "Maps";

		public override void Load()
		{
			Loaded = true;
			_content = new ContentManager(GameMgr.Game.Services);
			_content.RootDirectory = AssetMgr.ContentDir + '/' + AssetMgr.MapsDir;

			AddResource("Test", _content.Load<TiledMap>("test"));
		}

		public override void Unload()
		{
			_content.Unload();
		}

	}
}
