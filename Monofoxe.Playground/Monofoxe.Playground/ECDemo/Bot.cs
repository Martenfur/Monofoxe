using Microsoft.Xna.Framework;
using Monofoxe.Engine.Drawing;
using Monofoxe.Engine.EC;
using Monofoxe.Engine.Resources;
using Monofoxe.Engine.SceneSystem;
using Monofoxe.Engine.Utils;

namespace Monofoxe.Playground.ECDemo
{
	/// <summary>
	/// Basic position component. 
	/// </summary>
	public class Bot : Entity
	{
		public float TurningSpeed = 60;

		private readonly ActorComponent _actor;

		public Bot(Layer layer) : base(layer)
		{
			var botSprite = ResourceHub.GetResource<Sprite>("DefaultSprites", "Bot");

			AddComponent(new PositionComponent(Vector2.Zero));
			_actor = AddComponent(new ActorComponent(botSprite));

			// It is recommended to reuse random objects.
			TurningSpeed = ECDemoFactory.Random.Next(120, 240);

		}


		public override void Update()
		{
			base.Update();
			_actor.Move = true;
			_actor.Direction += TimeKeeper.Global.Time(TurningSpeed); // ni-ni-ni-ni-ni-ni-ni-ni-ni-ni-ni-ni-ni-ni
		}
	}
}
