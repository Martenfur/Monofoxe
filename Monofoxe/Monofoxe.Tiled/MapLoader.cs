using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MonoGame.Extended.Tiled;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;


namespace Monofoxe.Tiled
{
	public class MapLoader
	{
		public static void LoadMap(TiledMap map)
		{
			//MonoGame.Extended.Tiled.TiledMap;
			TiledMap map = null;
			//map.
			var tile = map.GetTilesetByTileGlobalIdentifier(0);
			//TiledMapTileset
		}
	}
}
