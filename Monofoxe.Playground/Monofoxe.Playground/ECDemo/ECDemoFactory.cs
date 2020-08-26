using Microsoft.Xna.Framework;
using Monofoxe.Engine.EC;
using Monofoxe.Engine.SceneSystem;
using Monofoxe.Engine.Utils;
using Monofoxe.Playground.Interface;
using System;

namespace Monofoxe.Playground.ECDemo
{
	public class ECDemoFactory : SceneFactory
	{
		public override string Description => "WASD - move player." 
			+ Environment.NewLine 
			+ ECDemo.ToggleEnabledButton + " - toggle bots Update events."
			+ Environment.NewLine
			+ ECDemo.ToggleVisibilityButton + " - toggle bots Draw events.";

		public static RandomExt Random = new RandomExt();

		public override void CreateScene()
		{
			Scene = SceneMgr.CreateScene("Entity Component");
			var layer = Scene.CreateLayer("Entity Component");
			
			for(var i = 0; i < 20; i += 1)
			{
				var bot = new Bot(layer);
				var position = bot.GetComponent<PositionComponent>();
				position.Position = new Vector2(Random.Next(100, 700), Random.Next(100, 500));
			}

			var player = new Player(layer, new Vector2(400, 300));

			// Now player will be drawn below all the bots, even though he was created last.
			// Reaordering puts him on the top of entity list.
			layer.ReorderEntityToTop(player);
			//layer.ReorderEntityToBottom(player); // Will have no effect.
			//layer.ReorderEntity(player, 2); // Player will be updated and drawn only after two entities above him.


			new ECDemo(layer);
			
		}

		public override void DestroyScene()
		{
			SceneMgr.DestroyScene(Scene);
		}
	}
}
