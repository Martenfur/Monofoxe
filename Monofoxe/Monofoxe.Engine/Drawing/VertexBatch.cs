using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;

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


		/// <summary>
		/// Disables rendering for everything that's outside of rectangle.
		/// NOTE: To enable scissoring, enable scissor test in Rasterizer.
		/// </summary>
		public Rectangle ScissorRectangle
		{
			set
			{
				if (value != _scissorRectangle)
				{
					FlushBatch();
					_scissorRectangle = value;
				}
			}
			get => _scissorRectangle;
		}
		private Rectangle _scissorRectangle;

		#endregion


		public GraphicsDevice GraphicsDevice { get; private set; }

		public bool NeedsFlush => _vertexPoolCount > 0 && _indexPoolCount > 0;

		private short[] _indexPool;
		private int _indexPoolCount = 0;
		private const int _indexPoolCapacity = short.MaxValue * 8;

		private VertexPositionColorTexture[] _vertexPool;
		private short _vertexPoolCount = 0;
		private const short _vertexPoolCapacity = short.MaxValue;

		private EffectPass _defaultEffectPass;

		public Matrix World
		{
			get => _world;
			set
			{
				if (_world != value)
				{
					FlushBatch();
					_world = value;
				}
			}
		}
		Matrix _world;

		public Matrix View
		{
			get => _view;
			set
			{
				if (_view != value)
				{
					FlushBatch();
					_view = value;
				}
			}
		}
		Matrix _view;

		public Matrix Projection
		{
			get => _projection;
			set
			{
				if (_projection != value)
				{
					FlushBatch();
					_projection = value;
				}
			}
		}
		Matrix _projection;



		public bool WorldStackEmpty => _worldStack.Count == 0;
		private Stack<Matrix> _worldStack = new Stack<Matrix>();

		public bool ViewStackEmpty => _viewStack.Count == 0;
		private Stack<Matrix> _viewStack = new Stack<Matrix>();

		public bool ProjectionStackEmpty => _projectionStack.Count == 0;
		private Stack<Matrix> _projectionStack = new Stack<Matrix>();

		/// <summary>
		/// Pixel offset is necessary for proper 2D rendering.
		/// If true, offsets x and y by -0.5.
		/// Should be true for OpenGL and false for DirectX. 
		/// </summary>
		public static bool UsesHalfPixelOffset = false;

		PrimitiveType _primitiveType = PrimitiveType.TriangleList;


		public Effect DefaultEffect
		{
			get => _defaultEffect;
			set
			{
				// TODO: Revisit.
				if (value != _defaultEffect)
				{
					FlushBatch();

					if (value == null)
					{
						_defaultEffect = _alphaBlendEffect;
					}
					else
					{
						_defaultEffect = value;
					}

					if (_texture != null)
					{
						// TODO: Move to the separate method.
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
		private Effect _defaultEffect;


		/// <summary>
		/// Default shader with proper alpha blending. 
		/// Replaces BasicEffect. Applied, when CurrentEffect and DefaulrEffect are null.
		/// </summary>
		private static Effect _alphaBlendEffect;

		public VertexBatch(
			GraphicsDevice graphicsDevice,
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


			if (_alphaBlendEffect == null)
			{
				_alphaBlendEffect = StuffResolver.GetStuff<IAlphaBlendEffectLoader>().Load();
			}
			DefaultEffect = _alphaBlendEffect;
		}



		void ApplyDefaultShader()
		{

			// The default shader is used for the transform matrix.

			_defaultEffect.Parameters["World"].SetValue(_world);
			_defaultEffect.Parameters["View"].SetValue(_view);
			if (UsesHalfPixelOffset)
			{
				_defaultEffect.Parameters["Projection"].SetValue(Matrix.CreateTranslation(-0.5f, -0.5f, 0) * _projection);
			}
			else
			{
				_defaultEffect.Parameters["Projection"].SetValue(_projection);
			}

			// We can use vertex shader from the default effect if the custom effect doesn't have one. 
			// Pixel shader get completely overwritten by the custom effect, though. 
			var gd = GraphicsDevice;
			gd.BlendState = _blendState;
			gd.DepthStencilState = _depthStencilState;
			gd.RasterizerState = _rasterizerState;
			gd.SamplerStates[0] = _samplerState;
			gd.ScissorRectangle = _scissorRectangle;

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

			int primitivesCount;

			if (_primitiveType == PrimitiveType.TriangleList || _primitiveType == PrimitiveType.TriangleStrip)
			{
				primitivesCount = _indexPoolCount / 3;
			}
			else
			{
				primitivesCount = _indexPoolCount / 2;
			}

			if (_effect == null)
			{
				GraphicsDevice.DrawUserIndexedPrimitives(
					_primitiveType,
					_vertexPool,
					0,
					_vertexPoolCount,
					_indexPool,
					0,
					primitivesCount,
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
						_primitiveType,
						_vertexPool,
						0,
						_vertexPoolCount,
						_indexPool,
						0,
						primitivesCount,
						VertexPositionColorTexture.VertexDeclaration
					);

				}

			}

			_vertexPoolCount = 0;
			_indexPoolCount = 0;
		}



		#region Quads.


		public void AddQuad(Vector2 position, Color color)
		{
			SetQuad(
				position.X,
				position.Y,
				_texture.Width,
				_texture.Height,
				color,
				Vector2.Zero,
				Vector2.One,
				0,
				0,
				0,
				0
			);
		}

		public void AddQuad(Vector2 position, RectangleF srcRectangle, Color color)
		{
			Vector2 texCoordTL;
			Vector2 texCoordBR;

			texCoordTL.X = srcRectangle.X / (float)_texture.Width;
			texCoordTL.Y = srcRectangle.Y / (float)_texture.Height;
			texCoordBR.X = (srcRectangle.X + srcRectangle.Width) / (float)_texture.Width;
			texCoordBR.Y = (srcRectangle.Y + srcRectangle.Height) / (float)_texture.Height;

			SetQuad(
				position.X,
				position.Y,
				srcRectangle.Width,
				srcRectangle.Height,
				color,
				texCoordTL,
				texCoordBR,
				0,
				0,
				0,
				0
			);
		}

		public void AddQuad(RectangleF destRectangle, RectangleF srcRectangle, Color color)
		{
			Vector2 texCoordTL;
			Vector2 texCoordBR;

			texCoordTL.X = srcRectangle.X / (float)_texture.Width;
			texCoordTL.Y = srcRectangle.Y / (float)_texture.Height;
			texCoordBR.X = (srcRectangle.X + srcRectangle.Width) / (float)_texture.Width;
			texCoordBR.Y = (srcRectangle.Y + srcRectangle.Height) / (float)_texture.Height;

			SetQuad(
				destRectangle.X,
				destRectangle.Y,
				destRectangle.Width,
				destRectangle.Height,
				color,
				texCoordTL,
				texCoordBR,
				0,
				0,
				0,
				0
			);
		}

		public void AddQuad(
			Vector2 position,
			RectangleF srcRectangle,
			Color color,
			double rotation,
			Vector2 origin,
			Vector2 scale,
			SpriteFlipFlags flipFlags,
			Vector4 zDepth
		)
		{

			origin = origin * scale;

			Vector2 texCoordTL;
			Vector2 texCoordBR;


			var w = srcRectangle.Width * scale.X;
			var h = srcRectangle.Height * scale.Y;
			texCoordTL.X = srcRectangle.X / (float)_texture.Width;
			texCoordTL.Y = srcRectangle.Y / (float)_texture.Height;
			texCoordBR.X = (srcRectangle.X + srcRectangle.Width) / (float)_texture.Width;
			texCoordBR.Y = (srcRectangle.Y + srcRectangle.Height) / (float)_texture.Height;

			if ((flipFlags & SpriteFlipFlags.FlipVertically) != 0)
			{
				var temp = texCoordBR.Y;
				texCoordBR.Y = texCoordTL.Y;
				texCoordTL.Y = temp;
			}
			if ((flipFlags & SpriteFlipFlags.FlipHorizontally) != 0)
			{
				var temp = texCoordBR.X;
				texCoordBR.X = texCoordTL.X;
				texCoordTL.X = temp;
			}

			if (rotation == 0f)
			{
				SetQuad(
					position.X - origin.X,
					position.Y - origin.Y,
					w,
					h,
					color,
					texCoordTL,
					texCoordBR,
					zDepth.X,
					zDepth.Y,
					zDepth.Z,
					zDepth.W
				);
			}
			else
			{
				SetQuad(
					position.X,
					position.Y,
					-origin.X,
					-origin.Y,
					w,
					h,
					(float)Math.Sin(rotation),
					(float)Math.Cos(rotation),
					color,
					texCoordTL,
					texCoordBR,
					zDepth.X,
					zDepth.Y,
					zDepth.Z,
					zDepth.W
				);
			}

		}

		public void AddQuad(
			RectangleF destRectangle,
			RectangleF srcRectangle,
			Color color,
			double rotation,
			Vector2 origin,
			SpriteFlipFlags flipFlags,
			Vector4 zDepth
		)
		{
			Vector2 texCoordTL;
			Vector2 texCoordBR;

			texCoordTL.X = srcRectangle.X / (float)_texture.Width;
			texCoordTL.Y = srcRectangle.Y / (float)_texture.Height;
			texCoordBR.X = (srcRectangle.X + srcRectangle.Width) / (float)_texture.Width;
			texCoordBR.Y = (srcRectangle.Y + srcRectangle.Height) / (float)_texture.Height;

			if (srcRectangle.Width != 0)
			{
				origin.X = origin.X * destRectangle.Width / srcRectangle.Width;
			}
			else
			{
				origin.X = origin.X * destRectangle.Width / srcRectangle.Width;
			}
			if (srcRectangle.Height != 0)
			{
				origin.Y = origin.Y * destRectangle.Height / srcRectangle.Height;
			}
			else
			{
				origin.Y = origin.Y * destRectangle.Height / srcRectangle.Height;
			}



			if ((flipFlags & SpriteFlipFlags.FlipVertically) != 0)
			{
				var temp = texCoordBR.Y;
				texCoordBR.Y = texCoordTL.Y;
				texCoordTL.Y = temp;
			}
			if ((flipFlags & SpriteFlipFlags.FlipHorizontally) != 0)
			{
				var temp = texCoordBR.X;
				texCoordBR.X = texCoordTL.X;
				texCoordTL.X = temp;
			}

			if (rotation == 0f)
			{
				SetQuad(
					destRectangle.X - origin.X,
					destRectangle.Y - origin.Y,
					destRectangle.Width,
					destRectangle.Height,
					color,
					texCoordTL,
					texCoordBR,
					zDepth.X,
					zDepth.Y,
					zDepth.Z,
					zDepth.W
				);
			}
			else
			{
				SetQuad(
					destRectangle.X,
					destRectangle.Y,
					-origin.X,
					-origin.Y,
					destRectangle.Width,
					destRectangle.Height,
					(float)Math.Sin(rotation),
					(float)Math.Cos(rotation),
					color,
					texCoordTL,
					texCoordBR,
					zDepth.X,
					zDepth.Y,
					zDepth.Z,
					zDepth.W
				);
			}

		}




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
			float depthTL,
			float depthTR,
			float depthBL,
			float depthBR
		)
		{

			FlushIfOverflow(4, 6);
			SetPrimitiveType(PrimitiveType.TriangleList);

			SetQuadIndices();


			fixed (VertexPositionColorTexture* vertexPtr = _vertexPool)
			{
				SetVertex(
					vertexPtr,
					x + dx * cos - dy * sin,
					y + dx * sin + dy * cos,
					depthTL,
					color,
					texCoordTL.X,
					texCoordTL.Y
				);

				SetVertex(
					vertexPtr,
					x + (dx + w) * cos - dy * sin,
					y + (dx + w) * sin + dy * cos,
					depthTR,
					color,
					texCoordBR.X,
					texCoordTL.Y
				);

				SetVertex(
					vertexPtr,
					x + dx * cos - (dy + h) * sin,
					y + dx * sin + (dy + h) * cos,
					depthBL,
					color,
					texCoordTL.X,
					texCoordBR.Y
				);

				SetVertex(
					vertexPtr,
					x + (dx + w) * cos - (dy + h) * sin,
					y + (dx + w) * sin + (dy + h) * cos,
					depthBR,
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
			float depthTL,
			float depthTR,
			float depthBL,
			float depthBR
		)
		{
			FlushIfOverflow(4, 6);
			SetPrimitiveType(PrimitiveType.TriangleList);

			SetQuadIndices();

			fixed (VertexPositionColorTexture* vertexPtr = _vertexPool)
			{
				SetVertex(vertexPtr, x, y, depthTL, color, texCoordTL.X, texCoordTL.Y);
				SetVertex(vertexPtr, x + w, y, depthTR, color, texCoordBR.X, texCoordTL.Y);
				SetVertex(vertexPtr, x, y + h, depthBL, color, texCoordTL.X, texCoordBR.Y);
				SetVertex(vertexPtr, x + w, y + h, depthBR, color, texCoordBR.X, texCoordBR.Y);
			}
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

		#endregion



		#region Primitives.

		public unsafe void AddPrimitive(PrimitiveType primitiveType, VertexPositionColorTexture[] vertices, short[] indices)
		{
			FlushIfOverflow(vertices.Length, indices.Length);
			SetPrimitiveType(primitiveType);

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


		private void SetPrimitiveType(PrimitiveType primitiveType)
		{
			if (_primitiveType != primitiveType)
			{
				FlushBatch();
				_primitiveType = primitiveType;
			}
		}

		#endregion



		#region Matrices. 

		/// <summary>
		/// Sets new transform matrix and pushes old matrix into the stack.
		/// </summary>
		public void PushWorldMatrix(Matrix matrix)
		{
			_worldStack.Push(_world);
			World = matrix;
		}

		/// <summary>
		/// Pushes the transform matrix into the stack without modifying current matrix.
		/// </summary>
		public void PushWorldMatrix() =>
			_worldStack.Push(_world);

		/// <summary>
		/// Pops a transform matrix from a stack and overrides current matrix.
		/// </summary>
		public void PopWorldMatrix()
		{
			if (_worldStack.Count == 0)
			{
				throw new InvalidOperationException("World matrix stack is empty! Did you forgot to set a matrix somewhere?");
			}
			World = _worldStack.Pop();
		}


		/// <summary>
		/// Sets new transform matrix and pushes old matrix into the stack.
		/// </summary>
		public void PushViewMatrix(Matrix matrix)
		{
			_viewStack.Push(_view);
			View = matrix;
		}

		/// <summary>
		/// Pushes the transform matrix into the stack without modifying current matrix.
		/// </summary>
		public void PushViewMatrix() =>
			_viewStack.Push(_view);

		/// <summary>
		/// Pops a transform matrix from a stack and overrides current matrix.
		/// </summary>
		public void PopViewMatrix()
		{
			if (_viewStack.Count == 0)
			{
				throw new InvalidOperationException("View matrix stack is empty! Did you forgot to set a matrix somewhere?");
			}
			View = _viewStack.Pop();
		}


		/// <summary>
		/// Sets new transform matrix and pushes old matrix into the stack.
		/// </summary>
		public void PushProjectionMatrix(Matrix matrix)
		{
			_projectionStack.Push(_projection);
			Projection = matrix;
		}

		/// <summary>
		/// Pushes the transform matrix into the stack without modifying current matrix.
		/// </summary>
		public void PushProjectionMatrix() =>
			_projectionStack.Push(_projection);

		/// <summary>
		/// Pops a transform matrix from a stack and overrides current matrix.
		/// </summary>
		public void PopProjectionMatrix()
		{
			if (_projectionStack.Count == 0)
			{
				throw new InvalidOperationException("World matrix stack is empty! Did you forgot to set a matrix somewhere?");
			}
			Projection = _projectionStack.Pop();
		}

		#endregion

	}
}

