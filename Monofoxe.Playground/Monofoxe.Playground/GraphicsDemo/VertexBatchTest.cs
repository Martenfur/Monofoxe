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

		Effect _seizure;
		Effect _grayscale;

		VertexPositionColorTexture[] _vertices;
		short[] _indices;
		public VertexBatchTest(Layer layer) : base(layer)
		{
			_monofoxeSprite = ResourceHub.GetResource<Sprite>("DefaultSprites", "AutismCat");
			_tex = ResourceHub.GetResource<Sprite>("DefaultSprites", "Monofoxe");
			_vbatch = new VertexBatch(
				GraphicsMgr.Device, 
				GraphicsMgr._defaultEffect, 
				null,
				SamplerState.PointWrap,
				null,
				null
			);
			_batch = new SpriteBatch(GraphicsMgr.Device);

			_seizure = ResourceHub.GetResource<Effect>("Effects", "Seizure");
			_grayscale = ResourceHub.GetResource<Effect>("Effects", "Grayscale");

			_vertices = new VertexPositionColorTexture[5];

			_vertices[0] = new VertexPositionColorTexture(new Vector3(100, 100, 0), Color.White, new Vector2(0, 0));
			_vertices[1] = new VertexPositionColorTexture(new Vector3(100 + 32, 100, 0), Color.Red, new Vector2(1, 0));
			_vertices[2] = new VertexPositionColorTexture(new Vector3(100 + 32, 100 + 32, 0), Color.White, new Vector2(1, 1));
			_vertices[3] = new VertexPositionColorTexture(new Vector3(100 + 32, 100 + 64, 0), Color.White, Vector2.Zero);
			_vertices[4] = new VertexPositionColorTexture(new Vector3(100 - 32, 100 + 64, 0), Color.White, Vector2.Zero);

			_indices = new short[]
			{
				0, 1, 2,
				0, 2, 3,
				0, 3, 4,
			};
			// - Primitives need a technique set.
			// - Sprites need it too.
			//
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
			if (!flag)
			{

				_vbatch.SetWorldViewProjection(
					GraphicsMgr.CurrentWorld,
					GraphicsMgr.CurrentView,
					GraphicsMgr.CurrentProjection
				);
				_vbatch.Texture = texture;
				for (var x = 0; x < 10; x += 1)
				{
					for (var y = 0; y < 10; y += 1)
					{
						_vbatch.DrawQuad(position + new Vector2(x, y) * 4, GraphicsMgr.CurrentColor);

					}
				}
				
				_vbatch.Texture = null;
				_vbatch.DrawPrimitive(PrimitiveType.LineList, _vertices, _indices);


				//_vbatch.Texture = texture;
				//_vbatch.DrawQuad(position, position + new Vector2(120, 50), Vector2.Zero, Vector2.One * 200, Color.White, 0, Vector2.Zero, SpriteFlipFlags.None, 0);

				_vbatch.FlushBatch();

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
