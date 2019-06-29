using Monofoxe.Engine;
using Monofoxe.Engine.ECS;
using Monofoxe.Engine.SceneSystem;

namespace Monofoxe.Playground.ECSDemo
{
	public class ECSDemo : Entity
	{
		
		public const Buttons ToggleVisibilityButton = Buttons.N;
		public const Buttons ToggleEnabledButton = Buttons.M;
		

		public ECSDemo(Layer layer) : base(layer)
		{
		}

		public override void Update()
		{
			if (Input.CheckButtonPress(ToggleVisibilityButton))
			{
				// This will turn off Draw events for bot's entity and all of its components.
				foreach (Entity bot in Layer.GetEntityListByComponent<BotComponent>())
				{
					bot.Visible = !bot.Visible;
				}
			}

			if (Input.CheckButtonPress(ToggleEnabledButton))
			{
				// This will turn off Update events for bot's entity and all of its components.
				foreach(Entity bot in Layer.GetEntityListByComponent<BotComponent>())
				{
					bot.Enabled = !bot.Enabled;
				}
			}

		}
		

	}
}
