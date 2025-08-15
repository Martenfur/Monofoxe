using Microsoft.Xna.Framework;
using Monofoxe.Engine;
using Monofoxe.Engine.EC;
using Monofoxe.Engine.SceneSystem;
using Monofoxe.Samples.Misc;
using System;

namespace Monofoxe.Samples.Demos
{
	public class ECDemo : Entity
	{
		public static readonly string Description = "WASD - move player."
			+ Environment.NewLine
			+ ToggleEnabledButton + " - toggle bots Update events."
			+ Environment.NewLine
			+ ToggleVisibilityButton + " - toggle bots Draw events.";

		public const Buttons ToggleVisibilityButton = Buttons.N;
		public const Buttons ToggleEnabledButton = Buttons.M;

		public ECDemo(Layer layer) : base(layer)
		{
			for (var i = 0; i < 20; i += 1)
			{
				var bot = new Bot(layer);
				var position = bot.GetComponent<PositionComponent>();
				position.Position = new Vector2(GameController.Random.Next(100, 700), GameController.Random.Next(100, 500));
			}

			var player = new Player(layer, new Vector2(400, 300));

			// Now player will be drawn below all the bots, even though he was created last.
			// Reordering puts him on the top of entity list.
			layer.ReorderEntityToTop(player);
			//layer.ReorderEntityToBottom(player); // Will have no effect.
			//layer.ReorderEntity(player, 2); // Player will be updated and drawn only after two entities above him.

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
