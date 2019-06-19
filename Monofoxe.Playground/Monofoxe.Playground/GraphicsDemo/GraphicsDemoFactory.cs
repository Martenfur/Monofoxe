using System;
using System.Collections.Generic;
using System.Text;
using Monofoxe.Playground.Interface;
using Monofoxe.Engine.SceneSystem;

namespace Monofoxe.Playground.GraphicsDemo
{
	public class GraphicsDemoFactory : SceneFactory
	{
		public override void CreateScene()
		{
			Scene = SceneMgr.CreateScene("GraphicsDemo");
			var layer = Scene.CreateLayer("GraphicsDemo");
			new ShapeDemo(layer);
		}

		public override void DestroyScene()
		{
			SceneMgr.DestroyScene(Scene);
		}
	}
}
