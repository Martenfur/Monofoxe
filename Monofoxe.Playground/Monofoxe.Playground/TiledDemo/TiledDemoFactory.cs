using Monofoxe.Engine.SceneSystem;
using Monofoxe.Playground.Interface;
using System;

namespace Monofoxe.Playground.TiledDemo
{
	public class TiledDemoFactory : SceneFactory
	{
		public override string Description => "";

		public override void CreateScene()
		{
			Scene = SceneMgr.CreateScene("Input");
			var layer = Scene.CreateLayer("Input");
			new TiledDemo(layer);
		}

		public override void DestroyScene()
		{
			SceneMgr.DestroyScene(Scene);
		}
	}
}
