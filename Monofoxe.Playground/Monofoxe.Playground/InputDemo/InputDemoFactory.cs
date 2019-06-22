using Monofoxe.Engine.SceneSystem;
using Monofoxe.Playground.Interface;
using System;

namespace Monofoxe.Playground.GraphicsDemo
{
	public class InputDemoFactory : SceneFactory
	{
		public override string Description => "Type something to see keyboard input." 
			+ Environment.NewLine 
			+ "Connect the gamepad and move sticks to engage rumble.";

		public override void CreateScene()
		{
			Scene = SceneMgr.CreateScene("Sprites");
			var layer = Scene.CreateLayer("Sprites");
			new InputDemo(layer);
		}

		public override void DestroyScene()
		{
			SceneMgr.DestroyScene(Scene);
		}
	}
}
