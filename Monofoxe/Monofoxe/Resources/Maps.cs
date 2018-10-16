using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Resources.Sprites;
using Monofoxe.Engine.Drawing;
using Monofoxe.Engine;
using MonoGame.Extended.Tiled;
using System;

namespace Resources
{
	public static class Maps
	{
		private static ContentManager _content;
		
		public static TiledMap Test;
		
		public static void Load()
		{
			_content = new ContentManager(GameMgr.Game.Services);
			_content.RootDirectory = AssetMgr.ContentDir + '/' + "Maps";
			
			Test = _content.Load<TiledMap>("test");
			var tile = Test.TileLayers[0].Tiles[0];
			
			var tileset = Test.GetTilesetByTileGlobalIdentifier(tile.GlobalIdentifier);

			Console.WriteLine(tileset.Name + " " + tile.GlobalIdentifier);
		}

		public static void Unload()
		{
			_content.Unload();
		}

	}
}
