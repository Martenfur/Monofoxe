using Monofoxe.Engine;
using Monofoxe.Engine.EC;
using Monofoxe.Engine.SceneSystem;

namespace Monofoxe.Samples.ECDemo
{
	public class ECDemo : Entity
	{
		
		public const Buttons ToggleVisibilityButton = Buttons.N;
		public const Buttons ToggleEnabledButton = Buttons.M;
		

		public ECDemo(Layer layer) : base(layer)
		{
		}

		public override void Update()
		{
			base.Update();

			if (Input.CheckButtonPress(ToggleVisibilityButton))
			{
				// This will turn off Draw events for bot's entity and all of its components.
				foreach (var bot in Layer.GetEntityList<Bot>())
				{
					bot.Visible = !bot.Visible;
				}
			}

			if (Input.CheckButtonPress(ToggleEnabledButton))
			{
				// This will turn off Update events for bot's entity and all of its components.
				foreach(var bot in Layer.GetEntityList<Bot>())
				{
					bot.Enabled = !bot.Enabled;
				}
			}

		}
		

	}
}
