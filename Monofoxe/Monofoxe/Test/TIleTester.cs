using System;
using System.Collections.Generic;
using Monofoxe.Engine;
using Monofoxe.Utils;
using Microsoft.Xna.Framework.Graphics;
using Monofoxe.Engine.ECS;
using Monofoxe.Engine.Drawing;
using Microsoft.Xna.Framework;
using Resources.Sprites;
using Monofoxe.Engine.SceneSystem;
using Monofoxe.Tiled;
using Monofoxe.Tiled.MapStructure;
using Monofoxe.Utils.Tilemaps;



namespace Monofoxe.Test
{
	public class TileTester : Entity
	{
		
		Scene _scene;
		TiledMap _map;
		Frame frame;

		public TileTester(Layer layer) : base(layer)
		{
			Resources.Maps.Load();

			_map = Resources.Maps.Test;
			MapLoader.LoadMap(_map);

			//Console.WriteLine(_map.Tilesets[0].Textures == null);
			//frame = new Frame(_map.Tilesets[0].Textures[0], new Rectangle(0, 0, 128, 128), Vector2.Zero, 128, 128);

			//_scene = MapLoader.LoadMap(Resources.Maps.Test);

		}

		public override void Update()
		{
		}

		public override void Draw()
		{//DrawMgr.DrawFrame(frame, Vector2.One * 80, Vector2.Zero);
			/*for(var y = 0; y < 100; y += 1)	
			{
				for(var x = 0; x < 100; x += 1)
				{	
					if (!_tilemap.GetTile(x, y).IsBlank)
					DrawMgr.DrawFrame(
						_tilemap.GetTile(x, y).GetFrame(), 
						new Vector2(_tilemap.TileWidth * x, _tilemap.TileHeight * y), 
						Vector2.Zero
					);
				}
			}*/
		}

	}
}
