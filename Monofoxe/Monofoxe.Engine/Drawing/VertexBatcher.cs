using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;


namespace Monofoxe.Engine.Drawing
{
	/// <summary>
	/// This class handles the queueing of batch items into the GPU by creating the triangle tesselations
	/// that are used to draw the sprite textures. This class supports int.MaxValue number of sprites to be
	/// batched and will process them into short.MaxValue groups (strided by 6 for the number of vertices
	/// sent to the GPU). 
	/// </summary>
	internal class VertexBatcher
	{
		/*
		 * Note that this class is fundamental to high performance for SpriteBatch games. Please exercise
		 * caution when making changes to this class.
		 */

		/// <summary>
		/// Initialization size for the batch item list and queue.
		/// </summary>
		private const int InitialBatchSize = 256;
		/// <summary>
		/// The maximum number of batch items that can be processed per iteration
		/// </summary>
		private const int MaxBatchSize = short.MaxValue / 6; // 6 = 4 vertices unique and 2 shared, per quad

		/// <summary>
		/// Initialization size for the vertex array, in batch units.
		/// </summary>
		private const int InitialVertexArraySize = 256;


		/// <summary>
		/// The target graphics device.
		/// </summary>
		private readonly GraphicsDevice _device;

		/// <summary>
		/// Vertex index array. The values in this array never change.
		/// </summary>
		private short[] _indexPool;
		private int _indexPoolCount = 0;
		private const int _indexPoolCapacity = short.MaxValue * 6;

		private VertexPositionColorTexture[] _vertexPool;
		private int _vertexPoolCount = 0;
		private const int _vertexPoolCapacity = short.MaxValue;


		public VertexBatcher(GraphicsDevice device)
		{
			_device = device;

			_indexPool = new short[_indexPoolCapacity];
			_vertexPool = new VertexPositionColorTexture[_vertexPoolCapacity];


			//EnsureArrayCapacity(capacity);
		}


		/// <summary>
		/// Resize and recreate the missing indices for the index and vertex position color buffers.
		/// </summary>
		private unsafe void EnsureArrayCapacity(int numBatchItems)
		{
			int neededCapacity = 6 * numBatchItems;
			if (_indexPool != null && neededCapacity <= _indexPool.Length)
			{
				// Short circuit out of here because we have enough capacity.
				return;
			}
			short[] newIndex = new short[6 * numBatchItems];
			int start = 0;
			if (_indexPool != null)
			{
				_indexPool.CopyTo(newIndex, 0);
				start = _indexPool.Length / 6;
			}
			fixed (short* indexFixedPtr = newIndex)
			{
				var indexPtr = indexFixedPtr + (start * 6);
				for (var i = start; i < numBatchItems; i++, indexPtr += 6)
				{
					/*
					 *  TL    TR
					 *   0----1 0,1,2,3 = index offsets for vertex indices
					 *   |   /| TL,TR,BL,BR are vertex references in SpriteBatchItem.
					 *   |  / |
					 *   | /  |
					 *   |/   |
					 *   2----3
					 *  BL    BR
					 */
					// Triangle 1
					*(indexPtr + 0) = (short)(i * 4);
					*(indexPtr + 1) = (short)(i * 4 + 1);
					*(indexPtr + 2) = (short)(i * 4 + 2);
					// Triangle 2
					*(indexPtr + 3) = (short)(i * 4 + 1);
					*(indexPtr + 4) = (short)(i * 4 + 3);
					*(indexPtr + 5) = (short)(i * 4 + 2);
				}
			}
			_indexPool = newIndex;

			_vertexPool = new VertexPositionColorTexture[4 * numBatchItems];
		}

		/// <summary>
		/// Sorts the batch items and then groups batch drawing into maximal allowed batch sets that do not
		/// overflow the 16 bit array indices for vertices.
		/// </summary>
		public unsafe void DrawBatch(Effect effect, Texture2D texture)
		{
			if (effect != null && effect.IsDisposed)
				throw new ObjectDisposedException("effect");

			// nothing to do
			if (_vertexPoolCount == 0)
			{
				return;
			}

			FlushVertexArray(effect, texture);

			_vertexPoolCount = 0;
			_indexPoolCount = 0;
		}

		/// <summary>
		/// Sends the triangle list to the graphics device. Here is where the actual drawing starts.
		/// </summary>
		private void FlushVertexArray(Effect effect, Texture texture)
		{
			if (effect == null)
			{
				effect = GraphicsMgr._defaultEffect;

				effect.Parameters["World"].SetValue(GraphicsMgr.CurrentWorld);
				effect.Parameters["View"].SetValue(GraphicsMgr.CurrentView);
				effect.Parameters["Projection"].SetValue(GraphicsMgr.CurrentProjection);

			}
			Console.WriteLine("Drawing shit!");

			var passes = effect.CurrentTechnique.Passes;
			foreach (var pass in passes)
			{
				pass.Apply();

				// Whatever happens in pass.Apply, make sure the texture being drawn
				// ends up in Textures[0].
				_device.Textures[0] = texture;

				_device.DrawUserIndexedPrimitives(
					PrimitiveType.TriangleList,
					_vertexPool,
					0,
					_vertexPoolCount,
					_indexPool,
					0,
					_indexPoolCount / 3,
					VertexPositionColorTexture.VertexDeclaration
				);
			}

		}

		unsafe void SetVertex(
			VertexPositionColorTexture* poolPtr,
			float x, float y, float z,
			Color color,
			float texX, float texY
		)
		{
			var vertexPtr = poolPtr + _vertexPoolCount;

			(*vertexPtr).Position.X = x;
			(*vertexPtr).Position.Y = y;
			(*vertexPtr).Position.Z = z;

			(*vertexPtr).Color = color;
			(*vertexPtr).TextureCoordinate.X = texX;
			(*vertexPtr).TextureCoordinate.Y = texY;

			_vertexPoolCount += 1;

			Console.WriteLine("VERTICES: " + _vertexPoolCount);

		}


		unsafe void SetQuadIndices(short* poolPtr)
		{
			var indexPtr = poolPtr + _indexPoolCount;

			*(indexPtr + 0) = (short)(_vertexPoolCount);
			*(indexPtr + 1) = (short)(_vertexPoolCount + 1);
			*(indexPtr + 2) = (short)(_vertexPoolCount + 2);
			// Second triangle.
			*(indexPtr + 3) = (short)(_vertexPoolCount + 1);
			*(indexPtr + 4) = (short)(_vertexPoolCount + 3);
			*(indexPtr + 5) = (short)(_vertexPoolCount + 2);

			_indexPoolCount += 6;

			Console.WriteLine("INDEXES: " + _indexPoolCount);
			
		}

		public unsafe void Set(
			float x, float y,
			float dx, float dy,
			float w, float h,
			float sin, float cos,
			Color color,
			Vector2 texCoordTL,
			Vector2 texCoordBR,
			float depth
		)
		{
			fixed (short* indexPtr = _indexPool)
			{
				SetQuadIndices(indexPtr);
			}

			fixed (VertexPositionColorTexture* vertexPtr = _vertexPool)
			{
				SetVertex(
					vertexPtr,
					x + dx * cos - dy * sin,
					y + dx * sin + dy * cos,
					depth,
					color,
					texCoordTL.X,
					texCoordTL.Y
				);

				SetVertex(
					vertexPtr,
					x + (dx + w) * cos - dy * sin,
					y + (dx + w) * sin + dy * cos,
					depth,
					color,
					texCoordBR.X,
					texCoordTL.Y
				);

				SetVertex(
					vertexPtr,
					x + dx * cos - (dy + h) * sin,
					y + dx * sin + (dy + h) * cos,
					depth,
					color,
					texCoordTL.X,
					texCoordBR.Y
				);

				SetVertex(
					vertexPtr,
					x + (dx + w) * cos - (dy + h) * sin,
					y + (dx + w) * sin + (dy + h) * cos,
					depth,
					color,
					texCoordBR.X,
					texCoordBR.Y
				);

			}

		}

		public unsafe void Set(
			float x, float y,
			float w, float h,
			Color color,
			Vector2 texCoordTL,
			Vector2 texCoordBR,
			float depth
		)
		{
			fixed (short* indexPtr = _indexPool)
			{
				SetQuadIndices(indexPtr);
			}

			fixed (VertexPositionColorTexture* vertexPtr = _vertexPool)
			{
				SetVertex(
						vertexPtr,
						x,
						y,
						depth,
						color,
						texCoordTL.X,
						texCoordTL.Y
					);

				SetVertex(
					vertexPtr,
					x + w,
					y,
					depth,
					color,
					texCoordBR.X,
					texCoordTL.Y
				);

				SetVertex(
					vertexPtr,
					x,
					y + h,
					depth,
					color,
					texCoordTL.X,
					texCoordBR.Y
				);

				SetVertex(
					vertexPtr,
					x + w,
					y + h,
					depth,
					color,
					texCoordBR.X,
					texCoordBR.Y
				);

			}
		}
	}
}

