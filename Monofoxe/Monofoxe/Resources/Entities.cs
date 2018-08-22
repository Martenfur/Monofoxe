
using Monofoxe.Engine.ECS;
using Microsoft.Xna.Framework;
using Monofoxe;
using Monofoxe.ECSTest.Components;

namespace Resources
{
	public static class Entities
	{
		
		/*
		// Ok, if you really want to go bananas -- uncomment this.
		// But after some thining I've realized that all this stuff
		// is quite useless. Maybe I'm wrong, I dunno. :S

		static Dictionary<string, Func<Entity>> _funcs = new Dictionary<string, Func<Entity>>();

		public static void Init()
		{
			_funcs.Add("test", __createSomething);
			
		}
		
		public static Entity CreateEntity(string tag) =>
			_funcs[tag]();
		*/
		
		public static Entity CreateSomething()
		{
			var entity = new Entity("testie");
			entity.AddComponent(new TestComponent(Vector2.Zero));

			return entity;
		}

		public static Entity CreateBall()
		{
			var entity = new Entity("ball");
			entity.AddComponent(new CMovement());
			entity.AddComponent(new CCollision());

			return entity;
		}

	}
}
