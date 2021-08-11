using Monofoxe.Engine.SceneSystem;
using System;

namespace Monofoxe.Samples
{
	public class SceneFactory
	{
		public Scene Scene;
		private Type _type;

		public SceneFactory(Type type, string description = "")
		{
			_type = type;
			Description = description;
		}

		public readonly string Description;

		public void CreateScene()
		{
			Scene = SceneMgr.CreateScene(_type.Name);
			Scene.CreateLayer("default");
			Activator.CreateInstance(_type, Scene["default"]);
		}

		public void DestroyScene() =>
			SceneMgr.DestroyScene(Scene);

		public void RestartScene()
		{
			DestroyScene();
			CreateScene();
		}
	}
}
