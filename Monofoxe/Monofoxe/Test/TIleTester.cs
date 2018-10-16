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
using Monofoxe.Utils.Tilemaps;


namespace Monofoxe.Test
{
	public class TileTester : Entity
	{
	
		Tilemap _tilemap;

		public TileTester(Layer layer) : base(layer)
		{
			Resources.Maps.Load();

			_tilemap = MapLoader.LoadMap(Resources.Maps.Test);

		}

		public override void Update()
		{
		}

		public override void Draw()
		{
			for(var y = 0; y < 100; y += 1)	
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
			}
		}

	}
}
