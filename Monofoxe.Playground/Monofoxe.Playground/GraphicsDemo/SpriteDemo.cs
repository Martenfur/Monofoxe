using System;
using System.Collections.Generic;
using System.Text;
using Monofoxe.Engine.ECS;
using Monofoxe.Engine.Drawing;
using Monofoxe.Engine.Utils;
using Monofoxe.Engine.SceneSystem;
using Microsoft.Xna.Framework;
using Resources.Sprites;


namespace Monofoxe.Playground.GraphicsDemo
{
	public class SpriteDemo : Entity
	{
		
		Color _mainColor = Color.White;
		Color _secondaryColor = Color.Violet;

		double _animation = 0;
		double _animationSpeed = 0.25;


		public SpriteDemo(Layer layer) : base(layer)
		{
		}

		public override void Update()
		{
			// Basic animation code.
			_animation += TimeKeeper.GlobalTime(_animationSpeed);
			if (_animation > 1)
			{
				_animation -= 1;
			}
		}

		public override void Draw()
		{
			var startingPosition = new Vector2(100, 100);
			var position = startingPosition;
			var spacing = 100;

			GraphicsMgr.CurrentColor = Color.White; // Sprites are affected by current color too.

			// Sprites can't have static methods. So we are pulling them from sprite group.
			Default.Monofoxe.Draw(position, Default.Monofoxe.Origin);

			position += Vector2.UnitX * spacing * 2;

			// If you want to animate the sprite, you must pass a value from 0 to 1 to it.
			Default.Fire.Draw(_animation, position, Default.Fire.Origin);

			position += Vector2.UnitX * spacing;

			// You can also access sprite's frame array, if you want to draw a specific frame.
			Default.Fire[2].Draw(position, Default.Fire.Origin);

			position += Vector2.UnitX * spacing;

			// You can scale, rotate srites and set custom origin point.
			Default.Fire.Draw(
				0.4f, 
				position,
				new Vector2(Default.Fire.Width, Default.Fire.Height) / 2, 
				new Vector2(1, 2) * (float)Math.Sin(_animation * Math.PI * 2), 
				(float)(359 * _animation), 
				Color.Red // Overrides CurrentColor.
			);



		}

	}
}
