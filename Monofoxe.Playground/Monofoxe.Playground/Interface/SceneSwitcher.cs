using Monofoxe.Engine.ECS;
using Monofoxe.Engine.SceneSystem;
using System.Collections.Generic;
using Monofoxe.Playground.GraphicsDemo;

namespace Monofoxe.Playground.Interface
{
	public static class SceneSwitcher
	{
		public static List<SceneFactory> Factories = new List<SceneFactory>
		{
			new GraphicsDemoFactory(),

		};

		public static int CurrentSceneID {get; private set;} = 0;

		public static Scene CurrentScene => Factories[CurrentSceneID].Scene;

		public static SceneFactory CurrentFactory => Factories[CurrentSceneID];

		public static void NextScene()
		{
			Factories[CurrentSceneID].DestroyScene();

			CurrentSceneID += 1;
			if (CurrentSceneID >= Factories.Count)
			{
				CurrentSceneID = 0;
			}

			Factories[CurrentSceneID].CreateScene();
		}


		public static void PreviousScene()
		{
			Factories[CurrentSceneID].DestroyScene();

			CurrentSceneID -= 1;
			if (CurrentSceneID < 0)
			{
				CurrentSceneID = Factories.Count - 1;
			}

			Factories[CurrentSceneID].CreateScene();
		}


		public static void RestartScene() =>
			Factories[CurrentSceneID].RestartScene();

	}
}
