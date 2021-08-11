using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Monofoxe.Engine;
using Monofoxe.Engine.EC;
using Monofoxe.Engine.Resources;
using Monofoxe.Engine.SceneSystem;
using Monofoxe.Samples.Misc;
using System;

namespace Monofoxe.Samples.Demos
{
	public class SceneSystemDemo : Entity
	{
		public static readonly string Description = "WASD - move player."
			+ Environment.NewLine
			+ ToggleEnabledButton + " - toggle background layer Update events."
			+ Environment.NewLine
			+ ToggleVisibilityButton + " - toggle background layer Draw events.";

		public const Buttons ToggleVisibilityButton = Buttons.N;
		public const Buttons ToggleEnabledButton = Buttons.M;

		Scene _testScene;

		public SceneSystemDemo(Layer layer) : base(layer)
		{
			// Creating new scene.
			_testScene = SceneMgr.CreateScene("SceneDemoDummy");
			var mainLayer = _testScene.CreateLayer("main");
			var backgroundLayer = _testScene.CreateLayer("background");

			// Update and Draw events will be executed for this layer first.
			// This can be counter-intuitive, but this will put the layer on the back.
			// Because it is being drawn first, everything else will be drawn on top of it.
			backgroundLayer.Priority = 999;
			
			// Applying a shader to the thingy.
			backgroundLayer.PostprocessorEffects.Add(ResourceHub.GetResource<Effect>("Effects", "Seizure"));

			
			// See ECDemo to learn how those work.
			new Player(mainLayer, new Vector2(400, 300));

			// Player will not draw lines to these bots, because they are on a different layer.
			for (var i = 0; i < 10; i += 1)
			{
				var bot = new Bot(backgroundLayer);
				var position = bot.GetComponent<PositionComponent>();
				position.Position = new Vector2(GameController.Random.Next(100, 700), GameController.Random.Next(100, 500));
			}

			// Player will draw lines to these bots, because they are on the same layer.
			for (var i = 0; i < 5; i += 1)
			{
				var bot = new Bot(mainLayer);
				var position = bot.GetComponent<PositionComponent>();
				position.Position = new Vector2(GameController.Random.Next(100, 700), GameController.Random.Next(100, 500));
			}
			
		}

		public override void Update()
		{
			base.Update();

			if (Input.CheckButtonPress(ToggleVisibilityButton))
			{
				// This will turn off Draw events for bot's entity and all of its components.
				foreach (var bot in _testScene["background"].GetEntityList<Bot>())
				{
					bot.Visible = !bot.Visible;
				}
			}

			if (Input.CheckButtonPress(ToggleEnabledButton))
			{
				// This will turn off Update events for bot's entity and all of its components.
				foreach (var bot in _testScene["background"].GetEntityList<Bot>())
				{
					bot.Enabled = !bot.Enabled;
				}
			}
		}
		

		public override void Destroy()
		{
			base.Destroy();
			SceneMgr.DestroyScene(_testScene);
		}


	}
}
