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
using Monofoxe.Engine.Resources;

namespace Monofoxe.Playground.GraphicsDemo
{
	public class SpriteDemo : Entity
	{
		
		Color _mainColor = Color.White;
		Color _secondaryColor = Color.Violet;

		double _animation = 0;
		double _animationSpeed = 0.25;

		SpriteBatch _batch;

		Surface _surface;

		Effect _seizure;

		Sprite _monofoxeSprite;
		Sprite _fireSprite;

		public SpriteDemo(Layer layer) : base(layer)
		{
			_monofoxeSprite = ResourceHub.GetResource<Sprite>("DefaultSprites", "Monofoxe");
			_fireSprite = ResourceHub.GetResource<Sprite>("DefaultSprites", "Fire");

			_batch = new SpriteBatch(GraphicsMgr.Device);
			_seizure = ResourceHub.GetResource<Effect>("Effects", "Seizure");
			InitSurface();
		}

		public override void Update()
		{
			base.Update();

			// Basic animation code.
			_animation += TimeKeeper.Global.Time(_animationSpeed);
			if (_animation > 1)
			{
				_animation -= 1;
			}
		}

		public override void Draw()
		{
			base.Draw();

			var startingPosition = new Vector2(100, 100);
			var position = startingPosition;
			var spacing = 100;

			GraphicsMgr.CurrentColor = Color.White; // Sprites are affected by current color too.
			
			// Sprites can't have static methods. So we are pulling them from sprite group.
			_monofoxeSprite.Draw(position);

			position += Vector2.UnitX * spacing * 2;

			// Setting a shader for the sprite.
			_seizure.SetWorldViewProjection(
				GraphicsMgr.CurrentWorld, 
				GraphicsMgr.CurrentView, 
				GraphicsMgr.CurrentProjection
			);

			GraphicsMgr.CurrentEffect = _seizure;
			// If you want to animate the sprite, you must pass a value from 0 to 1 to it.
			_fireSprite.Draw(position, _animation);
			GraphicsMgr.CurrentEffect = null;

			position += Vector2.UnitX * spacing;

			// You can also access sprite's frame array, if you want to draw a specific frame.
			_fireSprite[2].Draw(position, _fireSprite.Origin);

			position += Vector2.UnitX * spacing;

			// You can scale, rotate srites and set custom origin point.

			_fireSprite.Draw(
				position,
				0.4f, 
				new Vector2(_fireSprite.Width, _fireSprite.Height) / 2, 
				new Vector2(1, 2) * (float)Math.Sin(_animation * Math.PI * 2 * 2), 
				new Angle(360 * _animation), 
				Color.Red // Overrides CurrentColor.
			);


			position += Vector2.UnitX * spacing;

			// You also can draw only a part of the sprite.
			_monofoxeSprite.Draw(
				new Rectangle((int)(position.X), (int)(position.Y), 64, 64),
				0,
				new Rectangle(64, 64, 64, 64),
				Angle.Right,
				Color.White
			);


			position += Vector2.UnitY * spacing * 1.5f;
			position.X = 0;


			// You can extract raw texture from the frames. Note that you will get the whole texture atlas.
			var texture = _monofoxeSprite[0].Texture;
			var texturePosition = _monofoxeSprite[0].TexturePosition; // This will give you texture's position on the atlas.

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
				GraphicsMgr.CurrentView // Passig current transform matrix to match the camera.
			);
			_batch.Draw(texture, position, GraphicsMgr.CurrentColor);
			_batch.End();
			
			// After you're done, you can draw anything you like without switching graphics mode again.
			RectangleShape.Draw(position, position + new Vector2(texture.Width, texture.Height), true);


			position += Vector2.UnitX * 512;

			_surface.Draw(position);

			position += new Vector2(16, 150);

			GraphicsMgr.CurrentColor = Color.White;
			Text.CurrentFont = ResourceHub.GetResource<IFont>("Fonts", "Arial");
			Text.Draw("This text is drawn using default" + Environment.NewLine + "Monogame spritefont.", position);
			position += Vector2.UnitY * 48;
			Text.CurrentFont = ResourceHub.GetResource<IFont>("Fonts", "FancyFont");
			Text.Draw("This text is drawn using custom" + Environment.NewLine + "font made from a sprite.", position);

		}

		/// <summary>
		/// Creates a new surface and draws test stuff on it.
		/// </summary>
		void InitSurface()
		{
			_surface = new Surface(128, 128);

			GraphicsMgr.SetSurfaceTarget(_surface);

			GraphicsMgr.Device.Clear(_secondaryColor);

			GraphicsMgr.CurrentColor = _mainColor;
			CircleShape.Draw(new Vector2(64, 64), 64, false);

			GraphicsMgr.ResetSurfaceTarget();
		}


		public override void Destroy()
		{
			_surface.Dispose(); // Don't forget to dispose surfaces.
		}


	}
}
