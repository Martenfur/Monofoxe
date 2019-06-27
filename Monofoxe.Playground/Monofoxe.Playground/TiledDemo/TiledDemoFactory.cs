using Monofoxe.Engine.SceneSystem;
using Monofoxe.Playground.Interface;
using System;

namespace Monofoxe.Playground.TiledDemo
{
	public class TiledDemoFactory : SceneFactory
	{
		public override string Description => TiledDemo.BuildCustomMapBuilderButton + " - build map with custom map builder." 
		+ Environment.NewLine
		+ TiledDemo.BuildDefaultMapBuilderButton + " - build map with default map builder."
		+ Environment.NewLine
		+ TiledDemo.DestroyMapButton + " - destroy currently loaded map.";

		public override void CreateScene()
		{
			Scene = SceneMgr.CreateScene("Tiled");
			var layer = Scene.CreateLayer("Tiled");
			new TiledDemo(layer);
		}

		public override void DestroyScene()
		{
			SceneMgr.DestroyScene(Scene);
		}
	}
}
