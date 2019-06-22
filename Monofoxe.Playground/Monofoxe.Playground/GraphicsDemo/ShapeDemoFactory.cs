using Monofoxe.Engine.SceneSystem;
using Monofoxe.Playground.Interface;

namespace Monofoxe.Playground.GraphicsDemo
{
	public class ShapeDemoFactory : SceneFactory
	{
		public override string Description => "";

		public override void CreateScene()
		{
			Scene = SceneMgr.CreateScene("Shapes");
			var layer = Scene.CreateLayer("Shapes");
			new ShapeDemo(layer);
		}

		public override void DestroyScene()
		{
			SceneMgr.DestroyScene(Scene);
		}
	}
}
