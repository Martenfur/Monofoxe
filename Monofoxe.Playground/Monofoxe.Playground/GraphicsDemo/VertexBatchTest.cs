using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Monofoxe.Engine;
using Monofoxe.Engine.Drawing;
using Monofoxe.Engine.EC;
using Monofoxe.Engine.Resources;
using Monofoxe.Engine.SceneSystem;
using System.Diagnostics;

namespace Monofoxe.Playground.GraphicsDemo
{
	public class VertexBatchTest : Entity
	{


		VertexBatch _vbatch;

		Sprite _monofoxeSprite;
		
		VertexPositionColorTexture[] _triangleListVertices;
		short[] _triangleListIndices;

		VertexPositionColorTexture[] _lineListVertices;
		short[] _lineListIndices;

		public VertexBatchTest(Layer layer) : base(layer)
		{
			_monofoxeSprite = ResourceHub.GetResource<Sprite>("DefaultSprites", "AutismCat");
			_vbatch = new VertexBatch(
				GraphicsMgr.Device,
				null,
				SamplerState.PointWrap,
				null,
				null
			);
			
			_triangleListVertices = new VertexPositionColorTexture[5];

			_triangleListVertices[0] = new VertexPositionColorTexture(new Vector3(100, 100, 0), Color.White, new Vector2(0, 0));
			_triangleListVertices[1] = new VertexPositionColorTexture(new Vector3(100 + 32, 100, 0), Color.Red, new Vector2(1, 0));
			_triangleListVertices[2] = new VertexPositionColorTexture(new Vector3(100 + 32, 100 + 32, 0), Color.White, new Vector2(1, 1));
			_triangleListVertices[3] = new VertexPositionColorTexture(new Vector3(100 + 32, 100 + 64, 0), Color.White, Vector2.Zero);
			_triangleListVertices[4] = new VertexPositionColorTexture(new Vector3(100 - 32, 100 + 64, 0), Color.White, Vector2.Zero);

			_triangleListIndices = new short[]
			{
				0, 1, 2,
				0, 2, 3,
				0, 3, 4,
			};

			_lineListVertices = new VertexPositionColorTexture[5];

			_lineListVertices[0] = new VertexPositionColorTexture(new Vector3(10, 10, 0), Color.White, new Vector2(0, 0));
			_lineListVertices[1] = new VertexPositionColorTexture(new Vector3(10 + 32, 10, 0), Color.Red, new Vector2(1, 0));
			_lineListVertices[2] = new VertexPositionColorTexture(new Vector3(00 + 32, 10 + 32, 0), Color.White, new Vector2(1, 1));
			_lineListVertices[3] = new VertexPositionColorTexture(new Vector3(00 + 32, 10 + 64, 0), Color.White, Vector2.Zero);

			_lineListIndices = new short[]
			{
				0, 1,
				1, 2,
				2, 3,
				3, 0
			};


		}

		public override void Update()
		{
			base.Update();

		}

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
			var halfBounds = new RectangleF(0, 0, bounds.Width / 2, bounds.Height / 2);

			_vbatch.AddQuad(position, GraphicsMgr.CurrentColor);
			position += Vector2.UnitX * 100;
			_vbatch.AddQuad(position, halfBounds, GraphicsMgr.CurrentColor);
			position += Vector2.UnitX * 100;

			_vbatch.AddQuad(
				new RectangleF(position, new Vector2(300, 100)),
				halfBounds,
				GraphicsMgr.CurrentColor
			);

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
			_vbatch.AddPrimitive(PrimitiveType.LineList, _lineListVertices, _lineListIndices);

			_vbatch.AddPrimitive(PrimitiveType.TriangleList, _triangleListVertices, _triangleListIndices);


			_vbatch.Texture = texture;

			_vbatch.FlushBatch();


		}



		public override void Destroy()
		{
		}


	}
}
