using Monofoxe.Engine.SceneSystem;

namespace Monofoxe.Playground.Interface
{
	public abstract class SceneFactory
	{
		public Scene Scene;

		public abstract string Description {get;}

		public abstract void CreateScene();
		public abstract void DestroyScene();

		public void RestartScene()
		{
			DestroyScene();
			CreateScene();
		}



	}
}
