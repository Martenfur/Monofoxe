using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;

// Based on default Monogame's Spritebatch by maintainy bois.
// https://github.com/MonoGame/MonoGame/blob/master/MonoGame.Framework/Graphics/SpriteBatch.cs


namespace Monofoxe.Engine.Drawing
{
	/// <summary>
	/// Helper class for drawing text strings and sprites in one or more optimized batches.
	/// </summary>
	public class VertexBatch
	{

		#region Fields.

		public BlendState BlendState
		{
			get => _blendState;
			set
			{
				if (value != _blendState)
				{
					FlushBatch();
					_blendState = value;
				}
			}
		}
		BlendState _blendState;

		public SamplerState SamplerState
		{
			get => _samplerState;
			set
			{
				if (value != _samplerState)
				{
					FlushBatch();
					_samplerState = value;
				}
			}
		}
		SamplerState _samplerState;

		public DepthStencilState DepthStencilState
		{
			get => _depthStencilState;
			set
			{
				if (value != _depthStencilState)
				{
					FlushBatch();
					_depthStencilState = value;
				}
			}
		}
		DepthStencilState _depthStencilState;

		public RasterizerState RasterizerState
		{
			get => _rasterizerState;
			set
			{
				if (value != _rasterizerState)
				{
					FlushBatch();
					_rasterizerState = value;
				}
			}
		}
		RasterizerState _rasterizerState;
		
		public Effect Effect
		{
			get => _effect;
			set
			{
				if (value != _effect)
				{
					FlushBatch();
					_effect = value;
				}
			}
		}
		Effect _effect;


		public Texture2D Texture 
		{
			get => _texture;
			set
			{
				if (value != _texture)
				{
					FlushBatch();

					_texture = value;

					if (_texture != null)
					{
						_defaultEffect.CurrentTechnique = _defaultEffect.Techniques["TexturePremultiplied"];
					}
					else
					{
						_defaultEffect.CurrentTechnique = _defaultEffect.Techniques["Basic"];
					}
					_defaultEffectPass = _defaultEffect.CurrentTechnique.Passes[0];
				}

			}
		}
		Texture2D _texture;

		#endregion


		public GraphicsDevice GraphicsDevice { get; private set; }

		public bool NeedsFlush => _vertexPoolCount > 0 && _indexPoolCount > 0;



		private short[] _indexPool;
		private int _indexPoolCount = 0;
		private const int _indexPoolCapacity = short.MaxValue * 6; // TODO: Figure out better value.

		private VertexPositionColorTexture[] _vertexPool;
		private short _vertexPoolCount = 0;
		private const short _vertexPoolCapacity = short.MaxValue;

		private Effect _defaultEffect;
		private EffectPass _defaultEffectPass;

		Matrix _world;
		Matrix _view;
		Matrix _projection;

		public VertexBatch(
			GraphicsDevice graphicsDevice, 
			Effect defaultEffect, 
			BlendState blendState = null,
			SamplerState samplerState = null,
			DepthStencilState depthStencilState = null,
			RasterizerState rasterizerState = null
		)
		{
			GraphicsDevice = graphicsDevice ?? throw new ArgumentNullException("graphicsDevice");

			_blendState = blendState ?? BlendState.AlphaBlend;
			_samplerState = samplerState ?? SamplerState.LinearClamp;
			_depthStencilState = depthStencilState ?? DepthStencilState.None;
			_rasterizerState = rasterizerState ?? RasterizerState.CullCounterClockwise;
			
			_indexPool = new short[_indexPoolCapacity];
			_vertexPool = new VertexPositionColorTexture[_vertexPoolCapacity];

			_defaultEffect = defaultEffect;
			_defaultEffectPass = _defaultEffect.CurrentTechnique.Passes[0];
		}

		
		public void SetWorldViewProjection(
			Matrix world,
			Matrix view,
			Matrix projection
		)
		{
			if (
				_world != world
				|| _view != view
				|| _projection != projection
			)
			{
				FlushBatch();
			}

			_world = world;
			_view = view;
			_projection = projection;
		}


		void ApplyDefaultShader()
		{
			var gd = GraphicsDevice;
			gd.BlendState = _blendState;
			gd.DepthStencilState = _depthStencilState;
			gd.RasterizerState = _rasterizerState;
			gd.SamplerStates[0] = _samplerState;

			// The default shader is used for the transfrm matrix.

			_defaultEffect.Parameters["World"].SetValue(_world);
			_defaultEffect.Parameters["View"].SetValue(_view);
			_defaultEffect.Parameters["Projection"].SetValue(_projection);

			// We can use vertex shader from the default effect if the custom effect doesn't have one. 
			// Pixel shader get completely overwritten by the custom effect, though. 
			_defaultEffectPass.Apply();

			GraphicsDevice.Textures[0] = _texture;
		}

		private bool FlushIfOverflow(int newVerticesCount, int newIndicesCount)
		{
			if (
				_vertexPoolCount + newVerticesCount < _vertexPoolCapacity
				&& _indexPoolCount + newIndicesCount < _indexPoolCapacity
			)
			{
				return false;
			}

			FlushBatch();
			return true;
		}

		
		public void FlushBatch()
		{
			if (_vertexPoolCount == 0 || _indexPoolCount == 0)
			{
				return;
			}

			if (_effect != null && _effect.IsDisposed)
				throw new ObjectDisposedException("effect");

			ApplyDefaultShader();

			if (_effect == null)
			{
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
			else
			{

				var passes = _effect.CurrentTechnique.Passes;
				foreach (var pass in passes)
				{
					pass.Apply();

					// Whatever happens in pass.Apply, make sure the texture being drawn
					// ends up in Textures[0].
					GraphicsDevice.Textures[0] = _texture;

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

			_vertexPoolCount = 0;
			_indexPoolCount = 0;
		}

		private unsafe void SetVertex(
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




		public void DrawQuad(Vector2 position, Color color)
		{
			SetQuad(
				position.X,
				position.Y,
				_texture.Width,
				_texture.Height,
				color,
				Vector2.Zero,
				Vector2.One,
				0
			);

		}


		#region Your present.
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
		#endregion

		#region Quads.

		private unsafe void SetQuadIndices()
		{
			fixed (short* poolPtr = _indexPool)
			{
				var indexPtr = poolPtr + _indexPoolCount;

				// 0 - 1
				// | / |
				// 2 - 3

				*(indexPtr + 0) = _vertexPoolCount;
				*(indexPtr + 1) = (short)(_vertexPoolCount + 1);
				*(indexPtr + 2) = (short)(_vertexPoolCount + 2);
				// Second triangle.
				*(indexPtr + 3) = (short)(_vertexPoolCount + 1);
				*(indexPtr + 4) = (short)(_vertexPoolCount + 3);
				*(indexPtr + 5) = (short)(_vertexPoolCount + 2);
			}

			_indexPoolCount += 6;
		}

		private unsafe void SetQuad(
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

			FlushIfOverflow(4, 6);

			SetQuadIndices();


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

		private unsafe void SetQuad(
			float x, float y,
			float w, float h,
			Color color,
			Vector2 texCoordTL,
			Vector2 texCoordBR,
			float depth
		)
		{
			FlushIfOverflow(4, 6);

			SetQuadIndices();

			fixed (VertexPositionColorTexture* vertexPtr = _vertexPool)
			{
				SetVertex(vertexPtr, x, y, depth, color, texCoordTL.X, texCoordTL.Y);
				SetVertex(vertexPtr, x + w, y, depth, color, texCoordBR.X, texCoordTL.Y);
				SetVertex(vertexPtr, x, y + h, depth, color, texCoordTL.X, texCoordBR.Y);
				SetVertex(vertexPtr, x + w, y + h, depth, color, texCoordBR.X, texCoordBR.Y);
			}
		}

		#endregion



		#region Primitives.

		public void DrawPrimitive(VertexPositionColorTexture[] vertices, short[] indices)
		{
			SetPrimitive(vertices, indices);
		}


		private unsafe void SetPrimitive(VertexPositionColorTexture[] vertices, short[] indices)
		{
			FlushIfOverflow(vertices.Length, indices.Length);

			fixed (short* poolPtr = _indexPool, newIndices = indices)
			{
				var newIndicesPtr = newIndices;

				var indicesMax = poolPtr + _indexPoolCount + indices.Length;
				for (
					var indexPtr = poolPtr + _indexPoolCount;
					indexPtr < indicesMax;
					indexPtr += 1, newIndicesPtr += 1
				)
				{
					*indexPtr = (short)(*newIndicesPtr + _vertexPoolCount);
				}
				_indexPoolCount += (short)indices.Length;
			}

			fixed (VertexPositionColorTexture* poolPtr = _vertexPool, newVertices = vertices)
			{
				var newVerticesPtr = newVertices;

				var verticesMax = poolPtr + _vertexPoolCount + vertices.Length;
				for (
					var vertexPtr = poolPtr + _vertexPoolCount;
					vertexPtr < verticesMax;
					vertexPtr += 1, newVerticesPtr += 1
				)
				{
					*vertexPtr = *newVerticesPtr;
				}
				_vertexPoolCount += (short)vertices.Length;
			}
		}


		#endregion

	}
}

