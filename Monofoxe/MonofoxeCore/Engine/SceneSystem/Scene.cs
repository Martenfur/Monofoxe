using System;
using System.Collections.Generic;

namespace Monofoxe.Engine.SceneSystem
{
	public class Scene
	{
		public readonly string Name;
		

		public IReadOnlyCollection<Layer> Layers => _layers;
		private List<Layer> _layers = new List<Layer>();
		

		public Scene(string name)
		{
			Name = name;
			
		}


		internal void Destroy()
		{
			foreach(var layer in _layers)
			{
				LayerMgr.DestroyLayer(layer);
			}
			_layers.Clear();
		}





	}
}
