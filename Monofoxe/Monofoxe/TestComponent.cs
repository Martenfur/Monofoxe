using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Monofoxe.Engine;
using Monofoxe.Engine.ECS;
using Microsoft.Xna.Framework;

namespace Monofoxe
{
	public class TestComponent : IComponent
	{
		public string Tag {get;} = "test";
		public Entity Owner {get; set;}
		
		public Vector2 Position;

		public TestComponent(Vector2 pos)
		{
			Position = pos;
		}

	}
}
