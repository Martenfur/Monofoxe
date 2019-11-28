using Monofoxe.Engine.EC;
using Monofoxe.Engine.Utils;

namespace Monofoxe.Playground.ECDemo
{
	/// <summary>
	/// Basic position component. 
	/// </summary>
	public class BotComponent : Component
	{
		public float TurningSpeed = 60;

		public BotComponent()
		{

		}

		public override void Initialize()
		{
		}

		public override void FixedUpdate()
		{
		}

		public override void Update()
		{
			var actor = Owner.GetComponent<ActorComponent>();
			actor.Move = true;
			actor.Direction += TimeKeeper.Global.Time(TurningSpeed); // ni-ni-ni-ni-ni-ni-ni-ni-ni-ni-ni-ni-ni-ni
		}

		public override void Draw()
		{
		}

		
		public override void Destroy()
		{
		}
	}
}
