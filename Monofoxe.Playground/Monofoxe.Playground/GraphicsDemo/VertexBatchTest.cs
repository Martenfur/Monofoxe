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

		TriangleFanPrimitive _primitive;

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

			_primitive = new TriangleFanPrimitive();
			_primitive.Vertices.Add(new Vertex(new Vector3(0, 0, 0), Color.White, Vector2.Zero));
			_primitive.Vertices.Add(new Vertex(new Vector3(32, 0, -1), Color.White, Vector2.Zero));
			_primitive.Vertices.Add(new Vertex(new Vector3(32, 32, 0), Color.White, Vector2.Zero));
			_primitive.Vertices.Add(new Vertex(new Vector3(0, 32, -1), Color.White, Vector2.Zero));
			
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

			GraphicsMgr.CurrentColor = Color.Pink;


			// You can extract raw texture from the frames. Note that you will get the whole texture atlas.
			var texture = _monofoxeSprite[0].Texture;
			
			var sw = new Stopwatch();

			_vbatch.World = GraphicsMgr.VertexBatch.World;
			_vbatch.View = GraphicsMgr.VertexBatch.View;
			_vbatch.Projection = GraphicsMgr.VertexBatch.Projection;


			sw.Start();
			if (!flag)
			{

				_vbatch.Texture = texture;
				for (var x = 0; x < 0; x += 1)
				{
					for (var y = 0; y < 0; y += 1)
					{
						_vbatch.AddQuad(position + new Vector2(x, y), GraphicsMgr.CurrentColor);

					}
				}
				position += Vector2.UnitX * 250;

				var bounds = texture.Bounds.ToRectangleF();
				var halfBounds = new RectangleF(0 ,0, bounds.Width/ 2, bounds.Height / 2);

				_vbatch.AddQuad(position, GraphicsMgr.CurrentColor);
				position += Vector2.UnitX * 100;
				_vbatch.AddQuad(position, halfBounds, GraphicsMgr.CurrentColor);
				position += Vector2.UnitX * 100;

				_vbatch.AddQuad(
					new RectangleF(position, new Vector2(300, 100)), 
					halfBounds, 
					GraphicsMgr.CurrentColor
				); // Bugged.

				position += Vector2.UnitY * 150;
				_vbatch.AddQuad(
					position, 
					halfBounds, 
					GraphicsMgr.CurrentColor, 
					GameMgr.ElapsedTimeTotal * 10f * 0, 
					new Vector2(texture.Width / 4, texture.Height / 4), 
					Vector2.One * 2, 
					SpriteFlipFlags.FlipHorizontally, 
					Vector4.Zero
				);

				position += Vector2.UnitY * 150;
				_vbatch.AddQuad(
					new RectangleF(position, new Vector2(200, 100)),
					halfBounds,
					GraphicsMgr.CurrentColor,
					GameMgr.ElapsedTimeTotal * 10f * 0,
					new Vector2(texture.Width / 4, texture.Height / 4),
					SpriteFlipFlags.FlipHorizontally | SpriteFlipFlags.FlipVertically,
					Vector4.Zero
				);


				_vbatch.Texture = null;
				_vbatch.AddPrimitive(PrimitiveType.LineList, _vertices, _indices);

				_vbatch.AddPrimitive(PrimitiveType.TriangleList, _vertices, _indices);


				_vbatch.Texture = texture;
				//_vbatch.DrawQuad(position, Vector2.Zero, Vector2.One * 200, Color.White, 0, Vector2.Zero, SpriteFlipFlags.None, 0);

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
				GraphicsMgr.VertexBatch.View // Passig current transform matrix to match the camera.
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
