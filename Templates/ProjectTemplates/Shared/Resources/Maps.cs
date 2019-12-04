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
			if (Loaded)
			{
				return;
			}
			Loaded = true;

			_content = new ContentManager(GameMgr.Game.Services);
			_content.RootDirectory = AssetMgr.ContentDir + '/' + AssetMgr.MapsDir;

			// Add your resources here. 
			// Example:
			// AddResource("Test", _content.Load<TiledMap>("test"));
			// You can access the resources via ResourceHub.GetResource<>();
		}

		public override void Unload()
		{
			if (!Loaded)
			{
				return;
			}
			_content.Unload();
		}

	}
}
