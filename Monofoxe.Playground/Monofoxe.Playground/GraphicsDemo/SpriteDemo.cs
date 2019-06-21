﻿using System;
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


namespace Monofoxe.Playground.GraphicsDemo
{
	public class SpriteDemo : Entity
	{
		
		Color _mainColor = Color.White;
		Color _secondaryColor = Color.Violet;

		double _animation = 0;
		double _animationSpeed = 0.25;

		SpriteBatch _batch;

		public SpriteDemo(Layer layer) : base(layer)
		{
			_batch = new SpriteBatch(GraphicsMgr.Device);
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
				new Vector2(1, 2) * (float)Math.Sin(_animation * Math.PI * 2 * 2), 
				(float)(359 * _animation), 
				Color.Red // Overrides CurrentColor.
			);


			position += Vector2.UnitX * spacing;

			// You also can draw only a part of the sprite.
			Default.Monofoxe.Draw(
				0,
				new Rectangle((int)(position.X), (int)(position.Y), 64, 64),
				new Rectangle(64, 64, 64, 64),
				0,
				Color.White
			);


			position += Vector2.UnitY * spacing * 1.5f;
			position.X = 0;


			// You can extract raw texture from the frames. Note that you will get the whole texture atlas.
			var texture = Default.Monofoxe[0].Texture;
			var texturePosition = Default.Monofoxe[0].TexturePosition; // This will give you texture's position on the atlas.

			// But how are we gonna draw it? Monofoxe can't draw textures by itself.
			// We can use default Monogame's SpriteBatch for this.

			// But beforehand we must reset Monofoxe's graphics pipeline.
			// This method draws all batched graphics and resets internal graphics pipeline mode. 
			GraphicsMgr.SwitchGraphicsMode(GraphicsMode.None); 

			// After it, you can draw anything you like using any method.

			_batch.Begin( // If you don't want to create new SpriteBatch, you can use GraphicsMgr.Batch instead.
				SpriteSortMode.Deferred, 
				null, 
				null, 
				null, 
				null, 
				null, 
				GraphicsMgr.CurrentTransformMatrix // Passig current transform matrix to match the camera.
			);
			_batch.Draw(texture, position, GraphicsMgr.CurrentColor);
			_batch.End();
			
			// After you're done, you can draw anything you like without switching graphics mode again.
			RectangleShape.Draw(position, position + new Vector2(texture.Width, texture.Height), true);



		}

	}
}
