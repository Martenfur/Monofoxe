using Monofoxe.Engine.SceneSystem;
using Monofoxe.Playground.Interface;
using System;

namespace Monofoxe.Playground.UtilsDemo
{
	public class UtilsDemoFactory : SceneFactory
	{
		public override string Description => ""; 
			
		public override void CreateScene()
		{
			Scene = SceneMgr.CreateScene("Utilities");
			var layer = Scene.CreateLayer("Utilities");
			new UtilsDemo(layer);
		}

		public override void DestroyScene()
		{
			SceneMgr.DestroyScene(Scene);
		}
	}
}
