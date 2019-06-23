using Monofoxe.Engine.SceneSystem;
using Monofoxe.Engine.ECS;
using Monofoxe.Engine;
using Monofoxe.Engine.Utils;
using Monofoxe.Playground.Interface;
using Microsoft.Xna.Framework;


namespace Monofoxe.Playground.ECSDemo
{
	public class ECSDemoFactory : SceneFactory
	{
		public override string Description => "WASD - move player.";

		public static RandomExt Random = new RandomExt();

		public override void CreateScene()
		{
			Scene = SceneMgr.CreateScene("Entity Component System");
			var layer = Scene.CreateLayer("Entity Component System");
			
			for(var i = 0; i < 20; i += 1)
			{
				var bot = Entity.CreateFromTemplate(layer, "Bot");
				var position = bot.GetComponent<PositionComponent>();
				position.Position = new Vector2(Random.Next(100, 700), Random.Next(100, 500));
			}

			new Player(layer, new Vector2(400, 300));
			
		}

		public override void DestroyScene()
		{
			SceneMgr.DestroyScene(Scene);
		}
	}
}
