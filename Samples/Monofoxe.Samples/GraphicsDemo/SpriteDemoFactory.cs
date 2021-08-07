using Monofoxe.Engine.SceneSystem;
using Monofoxe.Samples.Interface;

namespace Monofoxe.Samples.GraphicsDemo
{
	public class SpriteDemoFactory : SceneFactory
	{
		public override string Description => "";

		public override void CreateScene()
		{
			Scene = SceneMgr.CreateScene("Sprites");
			var layer = Scene.CreateLayer("Sprites");
			new SpriteDemo(layer);
		}

		public override void DestroyScene()
		{
			SceneMgr.DestroyScene(Scene);
		}
	}
}
