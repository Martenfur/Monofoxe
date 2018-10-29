using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Resources.Sprites;
using Monofoxe.Engine.Drawing;
using Monofoxe.Engine;
//using MonoGame.Extended.Tiled;
using Monofoxe.Tiled.MapStructure;
using Monofoxe.Tiled.ContentReaders;

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
			
			//Monofoxe.Tiled.MapLoader.TestLoadMap(_content);
			Test = _content.Load<TiledMap>("test");
			//Console.WriteLine("Mapwidth: " + Test.Width);
			//var tile = Test.TileLayers[0].Tiles[0];
			
			//var tileset = Test.GetTilesetByTileGlobalIdentifier(tile.GlobalIdentifier);
			
		}

		public static void Unload()
		{
			_content.Unload();
		}

	}
}
