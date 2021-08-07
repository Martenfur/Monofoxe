using Monofoxe.Engine.SceneSystem;
using Monofoxe.Playground.Interface;

namespace Monofoxe.Playground.CoroutinesDemo
{
  public class CoroutinesDemoFactory : SceneFactory
	{
		public override string Description => ""; 
			
		public override void CreateScene()
		{
			Scene = SceneMgr.CreateScene("Coroutines");
			var layer = Scene.CreateLayer("Coroutines");
			new CoroutinesDemo(layer);
		}

		public override void DestroyScene()
		{
			SceneMgr.DestroyScene(Scene);
		}
	}
}
