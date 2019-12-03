using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Monofoxe.Engine.Drawing
{
	/// <summary>
	/// Helper class for drawing text strings and sprites in one or more optimized batches.
	/// </summary>
	public class VertexBatch : IDisposable
	{
		#region Private Fields
		
		BlendState _blendState;
		SamplerState _samplerState;
		DepthStencilState _depthStencilState;
		RasterizerState _rasterizerState;
		Effect _effect;
		Texture2D _texture;
		bool _beginCalled;

		
		Rectangle _tempRect = new Rectangle(0, 0, 0, 0);
		Vector2 _texCoordTL = new Vector2(0, 0);
		Vector2 _texCoordBR = new Vector2(0, 0);
		#endregion

		#region Stuff.

		bool disposed;

		// The GraphicsDevice property should only be accessed in Dispose(bool) if the disposing
		// parameter is true. If disposing is false, the GraphicsDevice may or may not be
		// disposed yet.
		GraphicsDevice graphicsDevice;

		public GraphicsDevice GraphicsDevice
		{
			get
			{
				return graphicsDevice;
			}

			internal set
			{
				Debug.Assert(value != null);

				if (graphicsDevice == value)
					return;

				graphicsDevice = value;

			}
		}

		public bool IsDisposed
		{
			get
			{
				return disposed;
			}
		}

		#endregion
		
		
		public VertexBatch(GraphicsDevice graphicsDevice)
		{
			if (graphicsDevice == null)
			{
				throw new ArgumentNullException("graphicsDevice");
			}

			GraphicsDevice = graphicsDevice;

			
			
			_beginCalled = false;
			
			_indexPool = new short[_indexPoolCapacity];
			_vertexPool = new VertexPositionColorTexture[_vertexPoolCapacity];
		}

		/// <summary>
		/// Begins a new sprite and text batch with the specified render state.
		/// </summary>
		public void Begin(
			Texture2D texture,
			BlendState blendState = null,
			SamplerState samplerState = null,
			DepthStencilState depthStencilState = null,
			RasterizerState rasterizerState = null,
			Effect effect = null,
			Matrix? transformMatrix = null
		)
		{
			if (_beginCalled)
			{
				throw new InvalidOperationException("Begin cannot be called again until End has been successfully called.");
			}

			// defaults
			_blendState = blendState ?? BlendState.AlphaBlend;
			_samplerState = samplerState ?? SamplerState.LinearClamp;
			_depthStencilState = depthStencilState ?? DepthStencilState.None;
			_rasterizerState = rasterizerState ?? RasterizerState.CullCounterClockwise;
			_effect = effect;
			_texture = texture;
			_beginCalled = true;
		}

		/// <summary>
		/// Flushes all batched text and sprites to the screen.
		/// </summary>
		public void End()
		{
			if (!_beginCalled)
			{
				throw new InvalidOperationException("Begin must be called before calling End.");
			}

			_beginCalled = false;

			var gd = GraphicsDevice;
			gd.BlendState = _blendState;
			gd.DepthStencilState = _depthStencilState;
			gd.RasterizerState = _rasterizerState;
			gd.SamplerStates[0] = _samplerState;

			DrawBatch(_effect, _texture);
		}
		

		void CheckValid()
		{
			if (!_beginCalled)
			{
				throw new InvalidOperationException("Draw was called, but Begin has not yet been called. Begin must be called successfully before you can call Draw.");
			}
		}


		#region Batcher stuff.
		
		/// <summary>
		/// Vertex index array. The values in this array never change.
		/// </summary>
		private short[] _indexPool;
		private int _indexPoolCount = 0;
		private const int _indexPoolCapacity = short.MaxValue * 6;

		private VertexPositionColorTexture[] _vertexPool;
		private int _vertexPoolCount = 0;
		private const int _vertexPoolCapacity = short.MaxValue;
		

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
			
			var passes = effect.CurrentTechnique.Passes;
			foreach (var pass in passes)
			{
				pass.Apply();

				// Whatever happens in pass.Apply, make sure the texture being drawn
				// ends up in Textures[0].
				GraphicsDevice.Textures[0] = texture;

				GraphicsDevice.DrawUserIndexedPrimitives(
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

		#endregion


		public void Draw(Texture2D texture, Vector2 position, Color color)
		{
			CheckValid();

			Set(
				position.X,
				position.Y,
				texture.Width,
				texture.Height,
				color,
				Vector2.Zero,
				Vector2.One,
				0
			);

		}

		/*
		public void Draw(
			Texture2D texture,
			Vector2 position,
			Rectangle? sourceRectangle,
			Color color,
			float rotation,
			Vector2 origin,
			Vector2 scale,
			SpriteEffects effects,
			float layerDepth
		)
		{
			CheckValid(texture);

			var item = _batcher.CreateBatchItem();
			item.Texture = texture;


			origin = origin * scale;

			float w, h;
			if (sourceRectangle.HasValue)
			{
				var srcRect = sourceRectangle.GetValueOrDefault();
				w = srcRect.Width * scale.X;
				h = srcRect.Height * scale.Y;
				_texCoordTL.X = srcRect.X / (float)texture.Width;
				_texCoordTL.Y = srcRect.Y / (float)texture.Height;
				_texCoordBR.X = (srcRect.X + srcRect.Width) / (float)texture.Width;
				_texCoordBR.Y = (srcRect.Y + srcRect.Height) / (float)texture.Height;
			}
			else
			{
				w = texture.Width * scale.X;
				h = texture.Height * scale.Y;
				_texCoordTL = Vector2.Zero;
				_texCoordBR = Vector2.One;
			}

			if ((effects & SpriteEffects.FlipVertically) != 0)
			{
				var temp = _texCoordBR.Y;
				_texCoordBR.Y = _texCoordTL.Y;
				_texCoordTL.Y = temp;
			}
			if ((effects & SpriteEffects.FlipHorizontally) != 0)
			{
				var temp = _texCoordBR.X;
				_texCoordBR.X = _texCoordTL.X;
				_texCoordTL.X = temp;
			}

			if (rotation == 0f)
			{
				item.Set(
					position.X - origin.X,
					position.Y - origin.Y,
					w,
					h,
					color,
					_texCoordTL,
					_texCoordBR,
					layerDepth
				);
			}
			else
			{
				item.Set(
					position.X,
					position.Y,
					-origin.X,
					-origin.Y,
					w,
					h,
					(float)Math.Sin(rotation),
					(float)Math.Cos(rotation),
					color,
					_texCoordTL,
					_texCoordBR,
					layerDepth
				);
			}

		}

		public void Draw(
			Texture2D texture,
			Vector2 position,
			Rectangle? sourceRectangle,
			Color color,
			float rotation,
			Vector2 origin,
			float scale,
			SpriteEffects effects,
			float layerDepth
		)
		{
			var scaleVec = new Vector2(scale, scale);
			Draw(texture, position, sourceRectangle, color, rotation, origin, scaleVec, effects, layerDepth);
		}

		public void Draw(Texture2D texture,
			Rectangle destinationRectangle,
			Rectangle? sourceRectangle,
			Color color,
			float rotation,
			Vector2 origin,
			SpriteEffects effects,
			float layerDepth
		)
		{
			CheckValid(texture);

			var item = _batcher.CreateBatchItem();
			item.Texture = texture;

			if (sourceRectangle.HasValue)
			{
				var srcRect = sourceRectangle.GetValueOrDefault();
				_texCoordTL.X = srcRect.X / (float)texture.Width;
				_texCoordTL.Y = srcRect.Y / (float)texture.Height;
				_texCoordBR.X = (srcRect.X + srcRect.Width) / (float)texture.Width;
				_texCoordBR.Y = (srcRect.Y + srcRect.Height) / (float)texture.Height;

				if (srcRect.Width != 0)
					origin.X = origin.X * (float)destinationRectangle.Width / (float)srcRect.Width;
				else
					origin.X = origin.X * (float)destinationRectangle.Width / (float)texture.Width;
				if (srcRect.Height != 0)
					origin.Y = origin.Y * (float)destinationRectangle.Height / (float)srcRect.Height;
				else
					origin.Y = origin.Y * (float)destinationRectangle.Height / (float)texture.Height;
			}
			else
			{
				_texCoordTL = Vector2.Zero;
				_texCoordBR = Vector2.One;

				origin.X = origin.X * (float)destinationRectangle.Width / (float)texture.Width;
				origin.Y = origin.Y * (float)destinationRectangle.Height / (float)texture.Height;
			}

			if ((effects & SpriteEffects.FlipVertically) != 0)
			{
				var temp = _texCoordBR.Y;
				_texCoordBR.Y = _texCoordTL.Y;
				_texCoordTL.Y = temp;
			}
			if ((effects & SpriteEffects.FlipHorizontally) != 0)
			{
				var temp = _texCoordBR.X;
				_texCoordBR.X = _texCoordTL.X;
				_texCoordTL.X = temp;
			}

			if (rotation == 0f)
			{
				item.Set(
					destinationRectangle.X - origin.X,
					destinationRectangle.Y - origin.Y,
					destinationRectangle.Width,
					destinationRectangle.Height,
					color,
					_texCoordTL,
					_texCoordBR,
					layerDepth
				);
			}
			else
			{
				item.Set(
					destinationRectangle.X,
					destinationRectangle.Y,
					-origin.X,
					-origin.Y,
					destinationRectangle.Width,
					destinationRectangle.Height,
					(float)Math.Sin(rotation),
					(float)Math.Cos(rotation),
					color,
					_texCoordTL,
					_texCoordBR,
					layerDepth
				);
			}

		}

		
		public void Draw(Texture2D texture, Vector2 position, Rectangle? sourceRectangle, Color color)
		{
			CheckValid(texture);

			var item = _batcher.CreateBatchItem();
			item.Texture = texture;

			Vector2 size;

			if (sourceRectangle.HasValue)
			{
				var srcRect = sourceRectangle.GetValueOrDefault();
				size = new Vector2(srcRect.Width, srcRect.Height);
				_texCoordTL.X = srcRect.X / (float)texture.Width;
				_texCoordTL.Y = srcRect.Y / (float)texture.Height;
				_texCoordBR.X = (srcRect.X + srcRect.Width) / (float)texture.Width;
				_texCoordBR.Y = (srcRect.Y + srcRect.Height) / (float)texture.Height;
			}
			else
			{
				size = new Vector2(texture.Width, texture.Height);
				_texCoordTL = Vector2.Zero;
				_texCoordBR = Vector2.One;
			}

			item.Set(
				position.X,
				position.Y,
				size.X,
				size.Y,
				color,
				_texCoordTL,
				_texCoordBR,
				0
			);

		}

		public void Draw(Texture2D texture, Rectangle destinationRectangle, Rectangle? sourceRectangle, Color color)
		{
			CheckValid(texture);
			
			var item = _batcher.CreateBatchItem();
			item.Texture = texture;


			if (sourceRectangle.HasValue)
			{
				var srcRect = sourceRectangle.GetValueOrDefault();
				_texCoordTL.X = srcRect.X / (float)texture.Width;
				_texCoordTL.Y = srcRect.Y / (float)texture.Height;
				_texCoordBR.X = (srcRect.X + srcRect.Width) / (float)texture.Width;
				_texCoordBR.Y = (srcRect.Y + srcRect.Height) / (float)texture.Height;
			}
			else
			{
				_texCoordTL = Vector2.Zero;
				_texCoordBR = Vector2.One;
			}

			item.Set(
				destinationRectangle.X,
				destinationRectangle.Y,
				destinationRectangle.Width,
				destinationRectangle.Height,
				color,
				_texCoordTL,
				_texCoordBR,
				0
			);

		}

		public void Draw(Texture2D texture, Rectangle destinationRectangle, Color color)
		{
			CheckValid(texture);

			var item = _batcher.CreateBatchItem();
			item.Texture = texture;

			item.Set(
				destinationRectangle.X,
				destinationRectangle.Y,
				destinationRectangle.Width,
				destinationRectangle.Height,
				color,
				Vector2.Zero,
				Vector2.One,
				0
			);

		}
		*/
		/// <summary>
		/// Immediately releases the unmanaged resources used by this object.
		/// </summary>
		public void Dispose()
		{
			graphicsDevice = null;
			disposed = true;

			GC.SuppressFinalize(this);
		}
	}
}

