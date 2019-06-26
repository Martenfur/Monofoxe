using System;
using System.Collections.Generic;
using System.Text;
using Monofoxe.Engine.ECS;
using Monofoxe.Engine.Drawing;
using Monofoxe.Engine.Utils;
using Monofoxe.Engine;
using Monofoxe.Engine.SceneSystem;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Resources.Sprites;
using System.Text.RegularExpressions;

namespace Monofoxe.Playground.SceneSystemDemo
{
	public class SceneSystemDemo : Entity
	{
		
		Color _mainColor = Color.White;
		Color _secondaryColor = Color.Violet;

		double _animation = 0;
		double _animationSpeed = 0.25;
		
		StringBuilder _keyboardInput = new StringBuilder();
		int _keyboardInputMaxLength = 32;

		public const Buttons KeyboardTestButton = Buttons.A;
		public const Buttons GamepadTestButton = Buttons.GamepadA;
		public const Buttons MouseTestButton = Buttons.MouseLeft;


		public SceneSystemDemo(Layer layer) : base(layer)
		{
		}

		public override void Update()
		{
			
		}

		public override void Draw()
		{
			var startingPosition = new Vector2(64, 64);
			var position = startingPosition;
			var spacing = 100;

		
		}

		public override void Destroy()
		{
		}


	}
}
