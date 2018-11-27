using Monofoxe.Engine;
using Monofoxe.Engine.ECS;
using Monofoxe.Engine.SceneSystem;
using Monofoxe.Engine.Utils.Tilemaps;
using Resources.Sprites;
using Resources;

namespace Monofoxe.Test
{
	public class TileTester : Entity
	{
		
		public TileTester(Layer layer) : base(layer)
		{
		
			Maps.Test.Load();
			
			//Console.WriteLine(_map.Tilesets[0].Textures == null);
			//frame = new Frame(_map.Tilesets[0].Textures[0], new Rectangle(0, 0, 128, 128), Vector2.Zero, 128, 128);

			//_scene = MapLoader.LoadMap(Resources.Maps.Test);

		}

		public override void Update()
		{
			//var tilemap = Maps.Test.MapScene.FindEntity("basicTilemap");
			
			if (Input.CheckButtonPress(Buttons.L))
			{
				Maps.Test.Unload();
			}
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
