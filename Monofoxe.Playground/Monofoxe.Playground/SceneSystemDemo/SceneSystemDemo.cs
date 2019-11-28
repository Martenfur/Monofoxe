using System;
using System.Collections.Generic;
using System.Text;
using Monofoxe.Engine.EC;
using Monofoxe.Engine.Drawing;
using Monofoxe.Engine.Utils;
using Monofoxe.Engine;
using Monofoxe.Engine.SceneSystem;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Resources.Sprites;
using System.Text.RegularExpressions;
using Monofoxe.Playground.ECDemo;
using Monofoxe.Engine.Resources;

namespace Monofoxe.Playground.SceneSystemDemo
{
	public class SceneSystemDemo : Entity
	{


		public const Buttons ToggleVisibilityButton = Buttons.N;
		public const Buttons ToggleEnabledButton = Buttons.M;

		Scene _testScene;

		public SceneSystemDemo(Layer layer) : base(layer)
		{
			// Creating new scene.
			_testScene = SceneMgr.CreateScene("SceneDemoDummy");
			var mainLayer = _testScene.CreateLayer("main");
			var backgroungLayer = _testScene.CreateLayer("background");

			// Update and Draw events will be executed for this layer first.
			// This can be counter-intuitive, but this will put the layer on the back.
			// Because it is being drawn first, everything else will be drawn on top of it.
			backgroungLayer.Priority = 999;
			
			// Applying a shader to the thingy.
			backgroungLayer.PostprocessorEffects.Add(ResourceHub.GetResource<Effect>("Effects", "Seizure"));

			
			// See ECDemo to learn how those work.
			new Player(mainLayer, new Vector2(400, 300));

			// Player will not draw lines to these bots, because they are on a different layer.
			for (var i = 0; i < 10; i += 1)
			{
				var bot = CreateFromTemplate(backgroungLayer, "Bot");
				var position = bot.GetComponent<PositionComponent>();
				position.Position = new Vector2(ECDemoFactory.Random.Next(100, 700), ECDemoFactory.Random.Next(100, 500));
			}

			// Player will draw lines to these bots, because they are on the same layer.
			for (var i = 0; i < 5; i += 1)
			{
				var bot = CreateFromTemplate(mainLayer, "Bot");
				var position = bot.GetComponent<PositionComponent>();
				position.Position = new Vector2(ECDemoFactory.Random.Next(100, 700), ECDemoFactory.Random.Next(100, 500));
			}
			
		}

		public override void Update()
		{
			if (Input.CheckButtonPress(ToggleVisibilityButton))
			{
				// This will turn off Draw events for bot's entity and all of its components.
				foreach (Entity bot in _testScene["background"].GetEntityListByComponent<BotComponent>())
				{
					bot.Visible = !bot.Visible;
				}
			}

			if (Input.CheckButtonPress(ToggleEnabledButton))
			{
				// This will turn off Update events for bot's entity and all of its components.
				foreach (Entity bot in _testScene["background"].GetEntityListByComponent<BotComponent>())
				{
					bot.Enabled = !bot.Enabled;
				}
			}
		}
		

		public override void Destroy()
		{
			SceneMgr.DestroyScene(_testScene);
		}


	}
}
