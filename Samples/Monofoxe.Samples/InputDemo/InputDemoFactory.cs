using Monofoxe.Engine.SceneSystem;
using Monofoxe.Samples.Interface;
using System;

namespace Monofoxe.Samples.InputDemo
{
	public class InputDemoFactory : SceneFactory
	{
		public override string Description => "Press " 
			+ InputDemo.KeyboardTestButton 
			+ "/" + InputDemo.GamepadTestButton 
			+ "/" + InputDemo.MouseTestButton 
			+ " to test input methods."
			+ Environment.NewLine
			+ "Type something to see keyboard input." 
			+ Environment.NewLine 
			+ "Connect the gamepad and move sticks to engage rumble.";

		public override void CreateScene()
		{
			Scene = SceneMgr.CreateScene("Input");
			var layer = Scene.CreateLayer("Input");
			new InputDemo(layer);
		}

		public override void DestroyScene()
		{
			SceneMgr.DestroyScene(Scene);
		}
	}
}
