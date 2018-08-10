using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Monofoxe.Engine;
using Monofoxe.Engine.ECS;
using Microsoft.Xna.Framework;

namespace Monofoxe.Resources
{
	public static class Entites
	{
		
		
		public static Entity CreateSomething(Vector2 pos)
		{
			var entity = new Entity("testie");
			entity.AddComponent(new TestComponent(pos));

			return entity;
		}
	}
}
