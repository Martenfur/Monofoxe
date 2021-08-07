using Monofoxe.Engine.SceneSystem;
using Monofoxe.Samples.Interface;
using System;

namespace Monofoxe.Samples.SceneSystemDemo
{
	public class SceneSystemDemoFactory : SceneFactory
	{
		public override string Description => "WASD - move player."
			+ Environment.NewLine
			+ SceneSystemDemo.ToggleEnabledButton + " - toggle background layer Update events."
			+ Environment.NewLine
			+ SceneSystemDemo.ToggleVisibilityButton + " - toggle background layer Draw events.";

		public override void CreateScene()
		{
			Scene = SceneMgr.CreateScene("Scene system");
			var layer = Scene.CreateLayer("Scene system");
			new SceneSystemDemo(layer);
		}

		public override void DestroyScene()
		{
			SceneMgr.DestroyScene(Scene);
		}
	}
}
