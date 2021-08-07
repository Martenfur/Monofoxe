using Monofoxe.Engine.SceneSystem;
using Monofoxe.Samples.Interface;

namespace Monofoxe.Samples.GraphicsDemo
{
	public class PrimitiveDemoFactory : SceneFactory
	{
		public override string Description => PrimitiveDemo.ToggleWireframeButton + " - toggle wireframe.";
		
		public override void CreateScene()
		{
			Scene = SceneMgr.CreateScene("Primitives");
			var layer = Scene.CreateLayer("Primitives");
			
			new PrimitiveDemo(layer);
		}

		public override void DestroyScene()
		{
			SceneMgr.DestroyScene(Scene);
		}
	}
}
