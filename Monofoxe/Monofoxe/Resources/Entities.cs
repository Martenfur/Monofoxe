using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Monofoxe.Engine;
using Monofoxe.Engine.ECS;
using Microsoft.Xna.Framework;
using Monofoxe;

namespace Resources
{
	public static class Entities
	{
		
		static Dictionary<string, Func<Entity>> _funcs = new Dictionary<string, Func<Entity>>();


		public static void Init()
		{
			_funcs.Add("testie", _test);
		}

		public static Entity CreateEntity(string tag) =>
			_funcs[tag]();
		
		
		static Func<Entity> _test = CreateSomething;
		private static Entity CreateSomething()
		{
			var entity = new Entity("testie");
			entity.AddComponent(new TestComponent(Vector2.Zero));

			return entity;
		}
	}
}
