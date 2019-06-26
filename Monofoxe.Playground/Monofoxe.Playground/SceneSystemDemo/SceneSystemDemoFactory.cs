using Monofoxe.Engine.SceneSystem;
using Monofoxe.Playground.Interface;
using System;

namespace Monofoxe.Playground.SceneSystemDemo
{
	public class SceneSystemDemoFactory : SceneFactory
	{
		public override string Description => "Press ";

		public override void CreateScene()
		{
			Scene = SceneMgr.CreateScene("Input");
			var layer = Scene.CreateLayer("Input");
			new SceneSystemDemo(layer);
		}

		public override void DestroyScene()
		{
			SceneMgr.DestroyScene(Scene);
		}
	}
}
