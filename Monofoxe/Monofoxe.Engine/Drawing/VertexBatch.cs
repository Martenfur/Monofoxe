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
		readonly VertexBatcher _batcher;

		BlendState _blendState;
		SamplerState _samplerState;
		DepthStencilState _depthStencilState;
		RasterizerState _rasterizerState;
		Effect _effect;
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

		

		public VertexBatch(GraphicsDevice graphicsDevice) : this(graphicsDevice, 0)
		{
		}

		public VertexBatch(GraphicsDevice graphicsDevice, int capacity)
		{
			if (graphicsDevice == null)
			{
				throw new ArgumentNullException("graphicsDevice");
			}

			this.GraphicsDevice = graphicsDevice;

			
			_batcher = new VertexBatcher(graphicsDevice, capacity);

			_beginCalled = false;
		}

		/// <summary>
		/// Begins a new sprite and text batch with the specified render state.
		/// </summary>
		public void Begin
		(
				 BlendState blendState = null,
				 SamplerState samplerState = null,
				 DepthStencilState depthStencilState = null,
				 RasterizerState rasterizerState = null,
				 Effect effect = null,
				 Matrix? transformMatrix = null
		)
		{
			if (_beginCalled)
				throw new InvalidOperationException("Begin cannot be called again until End has been successfully called.");

			// defaults
			_blendState = blendState ?? BlendState.AlphaBlend;
			_samplerState = samplerState ?? SamplerState.LinearClamp;
			_depthStencilState = depthStencilState ?? DepthStencilState.None;
			_rasterizerState = rasterizerState ?? RasterizerState.CullCounterClockwise;
			_effect = effect;
			
			_beginCalled = true;
		}

		/// <summary>
		/// Flushes all batched text and sprites to the screen.
		/// </summary>
		public void End()
		{
			if (!_beginCalled)
				throw new InvalidOperationException("Begin must be called before calling End.");

			_beginCalled = false;

			Setup();

			_batcher.DrawBatch(_effect);
		}

		void Setup()
		{
			var gd = GraphicsDevice;
			gd.BlendState = _blendState;
			gd.DepthStencilState = _depthStencilState;
			gd.RasterizerState = _rasterizerState;
			gd.SamplerStates[0] = _samplerState;

			//_spritePass.Apply();
		}

		void CheckValid(Texture2D texture)
		{
			if (texture == null)
				throw new ArgumentNullException("texture");
			if (!_beginCalled)
				throw new InvalidOperationException("Draw was called, but Begin has not yet been called. Begin must be called successfully before you can call Draw.");
		}

		/// <summary>
		/// Submit a sprite for drawing in the current batch.
		/// </summary>
		public void Draw(Texture2D texture,
				Vector2 position,
				Rectangle? sourceRectangle,
				Color color,
				float rotation,
				Vector2 origin,
				Vector2 scale,
				SpriteEffects effects,
								float layerDepth)
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
				item.Set(position.X - origin.X,
								position.Y - origin.Y,
								w,
								h,
								color,
								_texCoordTL,
								_texCoordBR,
								layerDepth);
			}
			else
			{
				item.Set(position.X,
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
								layerDepth);
			}

			FlushIfNeeded();
		}

		/// <summary>
		/// Submit a sprite for drawing in the current batch.
		/// </summary>
		public void Draw(Texture2D texture,
				Vector2 position,
				Rectangle? sourceRectangle,
				Color color,
				float rotation,
				Vector2 origin,
				float scale,
				SpriteEffects effects,
								float layerDepth)
		{
			var scaleVec = new Vector2(scale, scale);
			Draw(texture, position, sourceRectangle, color, rotation, origin, scaleVec, effects, layerDepth);
		}

		/// <summary>
		/// Submit a sprite for drawing in the current batch.
		/// </summary>
		public void Draw(Texture2D texture,
			Rectangle destinationRectangle,
			Rectangle? sourceRectangle,
			Color color,
			float rotation,
			Vector2 origin,
			SpriteEffects effects,
						float layerDepth)
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
				item.Set(destinationRectangle.X - origin.X,
								destinationRectangle.Y - origin.Y,
								destinationRectangle.Width,
								destinationRectangle.Height,
								color,
								_texCoordTL,
								_texCoordBR,
								layerDepth);
			}
			else
			{
				item.Set(destinationRectangle.X,
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
								layerDepth);
			}

			FlushIfNeeded();
		}

		// Mark the end of a draw operation for Immediate SpriteSortMode.
		internal void FlushIfNeeded()
		{
			//	if (_sortMode == SpriteSortMode.Immediate)
			//	{
			//		_batcher.DrawBatch(_sortMode, _effect);
			//}
		}

		/// <summary>
		/// Submit a sprite for drawing in the current batch.
		/// </summary>
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

			item.Set(position.X,
							 position.Y,
							 size.X,
							 size.Y,
							 color,
							 _texCoordTL,
							 _texCoordBR,
							 0);

			FlushIfNeeded();
		}

		/// <summary>
		/// Submit a sprite for drawing in the current batch.
		/// </summary>
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

			item.Set(destinationRectangle.X,
							 destinationRectangle.Y,
							 destinationRectangle.Width,
							 destinationRectangle.Height,
							 color,
							 _texCoordTL,
							 _texCoordBR,
							 0);

			FlushIfNeeded();
		}

		/// <summary>
		/// Submit a sprite for drawing in the current batch.
		/// </summary>
		public void Draw(Texture2D texture, Vector2 position, Color color)
		{
			CheckValid(texture);

			var item = _batcher.CreateBatchItem();
			item.Texture = texture;

			item.Set(position.X,
							 position.Y,
							 texture.Width,
							 texture.Height,
							 color,
							 Vector2.Zero,
							 Vector2.One,
							 0);

			FlushIfNeeded();
		}

		/// <summary>
		/// Submit a sprite for drawing in the current batch.
		/// </summary>
		public void Draw(Texture2D texture, Rectangle destinationRectangle, Color color)
		{
			CheckValid(texture);

			var item = _batcher.CreateBatchItem();
			item.Texture = texture;

			item.Set(destinationRectangle.X,
							 destinationRectangle.Y,
							 destinationRectangle.Width,
							 destinationRectangle.Height,
							 color,
							 Vector2.Zero,
							 Vector2.One,
							 0);

			FlushIfNeeded();
		}

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

