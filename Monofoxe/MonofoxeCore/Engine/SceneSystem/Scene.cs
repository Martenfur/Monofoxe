using System;
using System.Collections.Generic;

namespace Monofoxe.Engine.SceneSystem
{
	public class Scene
	{
		public readonly string Name;

		List<Layer> _layers = new List<Layer>();

		public Scene(string name)
		{
			Name = name;
			
		}





	}
}
