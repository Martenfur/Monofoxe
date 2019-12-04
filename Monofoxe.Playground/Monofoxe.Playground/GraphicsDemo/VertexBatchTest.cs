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
using System.Diagnostics;

namespace Monofoxe.Playground.GraphicsDemo
{
	public class VertexBatchTest : Entity
	{


		VertexBatch _vbatch;
		SpriteBatch _batch;

		Sprite _monofoxeSprite;
		Sprite _tex;

		public VertexBatchTest(Layer layer) : base(layer)
		{
			_monofoxeSprite = ResourceHub.GetResource<Sprite>("DefaultSprites", "AutismCat");
			_tex = ResourceHub.GetResource<Sprite>("DefaultSprites", "Monofoxe");
			_vbatch = new VertexBatch(GraphicsMgr.Device);
			_batch = new SpriteBatch(GraphicsMgr.Device);

		}

		public override void Update()
		{
			base.Update();
			if (Input.CheckButtonPress(Buttons.A))
			{
				flag = !flag;
			}
		}

		bool flag = false;
		public override void Draw()
		{
			base.Draw();

			var startingPosition = new Vector2(100, 100);
			var position = startingPosition;

			GraphicsMgr.CurrentColor = Color.White; // Sprites are affected by current color too.



			// You can extract raw texture from the frames. Note that you will get the whole texture atlas.
			var texture = _monofoxeSprite[0].Texture;
			var texture1 = _tex[0].Texture;

			// But how are we gonna draw it? Monofoxe can't draw textures by itself.
			// We can use default Monogame's SpriteBatch for this.

			// But beforehand we must reset Monofoxe's graphics pipeline.
			// This method draws all batched graphics and resets internal graphics pipeline mode. 
			GraphicsMgr.SwitchGraphicsMode(GraphicsMode.None);

			// After it, you can draw anything you like using any method.
			var sw = new Stopwatch();

			sw.Start();
			if (flag)
			{

				_vbatch.Begin( // If you don't want to create new SpriteBatch, you can use GraphicsMgr.Batch instead.
					null,
					SamplerState.PointWrap,
					null,
					null,
					null 
				);
				for (var x = 0; x < 100; x += 1)
				{
					for (var y = 0; y < 100; y += 1)
					{
						_vbatch.Draw(texture, position + new Vector2(x, y), GraphicsMgr.CurrentColor);
						//_vbatch.Draw(texture1, position + new Vector2(x, y), GraphicsMgr.CurrentColor);

					}
				}
				_vbatch.End();

			}
			else
			{
				_batch.Begin( // If you don't want to create new SpriteBatch, you can use GraphicsMgr.Batch instead.
				SpriteSortMode.Deferred,
				null,
				SamplerState.PointWrap,
				null,
				null,
				null,//GraphicsMgr, 
				GraphicsMgr.CurrentView // Passig current transform matrix to match the camera.
			);
				for (var x = 0; x < 100; x += 1)
				{
					for (var y = 0; y < 100; y += 1)
					{
						_batch.Draw(texture, position + new Vector2(x + 32, y + 32), GraphicsMgr.CurrentColor);
						//_batch.Draw(texture1, position + new Vector2(x + 32, y + 32), GraphicsMgr.CurrentColor);
					}
				}
				_batch.End();

			}

			sw.Stop();
			Text.CurrentFont = ResourceHub.GetResource<IFont>("Fonts", "Arial");
			Text.Draw(sw.Elapsed.ToString(), new Vector2(32, 32));
		}



		public override void Destroy()
		{
		}


	}
}
