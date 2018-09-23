using System;
using System.Collections.Generic;
using System.Text;
using Monofoxe.Engine.Drawing;
using Monofoxe.Engine.ECS;

namespace Monofoxe.Engine
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
