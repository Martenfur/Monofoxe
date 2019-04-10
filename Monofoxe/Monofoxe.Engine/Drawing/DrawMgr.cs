using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Monofoxe.Engine.SceneSystem;
using Monofoxe.Engine.Utils.Cameras;

namespace Monofoxe.Engine.Drawing
{

	public static class GraphicsMgr
	{
		private const int _vertexBufferSize = 320000;	// TODO: Figure out, if this value is actually ok.

		/// <summary>
		/// Default sprite batch used to draw sprites, text and surfaces.
		/// </summary>
		public static SpriteBatch Batch {get; private set;}


		public static GraphicsDevice Device {get; private set;}
		public static GraphicsDeviceManager DeviceManager {get; private set;}
		

		/// <summary>
		/// Currently enabled camera.
		/// </summary>
		public static Camera CurrentCamera {get; private set;}

		/// <summary>
		/// Current transformation matrix. Used to offset, rotate and scale graphics.
		/// </summary>
		public static Matrix CurrentTransformMatrix {get; private set;}
		public static Matrix CurrentProjection {get; private set;}

		private static Stack<Matrix> _transformMatrixStack = new Stack<Matrix>();

		
		/// <summary>
		/// Current drawing color. Affects shapes, sprites, text and primitives.
		/// TODO: Update description.
		/// </summary>
		public static Color CurrentColor = Color.White;

		private static DynamicVertexBuffer _vertexBuffer;
		private static DynamicIndexBuffer _indexBuffer;

		private static List<VertexPositionColorTexture> _vertices = new List<VertexPositionColorTexture>();
		private static List<short> _indices = new List<short>();

		/// <summary>
		/// Amount of draw calls per frame.
		/// </summary>
		public static int __drawcalls {get; private set;}


		/// <summary>
		/// Current graphics mode. Tells, which type of graphics is being drawn right now.
		/// </summary>
		private static GraphicsMode _currentGraphicsMode = GraphicsMode.None;
		private static Texture2D _currentTexture;
		
		/// <summary>
		/// We can set surface targets inside another surfaces.
		/// </summary>
		private static Stack<Surface> _surfaceStack = new Stack<Surface>();
		private static Surface _currentSurface;

		#region Modifiers.

		/// <summary>
		/// Disables rendering for everything that's outside of rectangle.
		/// NOTE: To enable scissoring, enable scissor test in Rasterizer.
		/// </summary>
		public static Rectangle ScissorRectangle
		{
			set
			{
				SwitchGraphicsMode(GraphicsMode.None);
				_scissorRectangle = value;
			}
			get => _scissorRectangle;
		}
		private static Rectangle _scissorRectangle;

		/// <summary>
		/// Rasterizer state. 
		/// NOTE: Do NOT modify object which you'll set. This will lead to errors and unexpected behaviour.
		/// </summary>
		public static RasterizerState Rasterizer
		{
			set
			{
				SwitchGraphicsMode(GraphicsMode.None); 
				_rasterizer = value;
			}
			get => _rasterizer;
		}
		private static RasterizerState _rasterizer;

		/// <summary>
		/// Sampler state. Used for interpolation and texture wrappping.
		/// NOTE: Do NOT modify object which you'll set. This will lead to errors and unexpected behaviour.
		/// </summary>
		public static SamplerState Sampler
		{
			set
			{
				SwitchGraphicsMode(GraphicsMode.None); 
				_sampler = value;
			}
			get => _sampler;
		}
		private static SamplerState _sampler;
		
		/// <summary>
		/// Blend state. Used for color blending.
		/// NOTE: Do NOT modify object which you'll set. This will lead to errors and unexpected behaviour.
		/// </summary>
		public static BlendState BlendState
		{
			set
			{
				SwitchGraphicsMode(GraphicsMode.None); 
				_blendState = value;
			}
			get => _blendState;
		}
		private static BlendState _blendState;


		/// <summary>
		/// Current shader. Set to null to reset to the default shader.
		/// </summary>
		public static Effect CurrentEffect
		{
			set
			{
				SwitchGraphicsMode(GraphicsMode.None);
				_currentEffect = value;
			}
			get => _currentEffect;
		}
		private static Effect _currentEffect;
		
		/// <summary>
		/// Default shader with proper alpha blending. 
		/// Replaces BasicEffect. Applied, when CurrentEffect is null.
		/// </summary>
		private static Effect _defaultEffect;
		private static string _defaultEffectName = "AlphaBlend";

		/// <summary>
		/// Used for drawing cameras.
		/// </summary>
		internal static RasterizerState _cameraRasterizerState;

		#endregion Modifiers.


		/// <summary>
		/// Matrix for offsetting, scaling and rotating canvas contents.
		/// </summary>
		public static Matrix CanvasMatrix = Matrix.CreateTranslation(Vector3.Zero); //We need zero matrix here, or else mouse position will derp out.
		
		
		/// <summary>
		/// Used to load default shader.
		/// </summary>
		private static ContentManager _content;
		
		/// <summary>
		/// Initialization function for draw manager. 
		/// </summary>
		public static void Init(GraphicsDevice device)
		{
			Device = device;			
			Device.DepthStencilState = DepthStencilState.DepthRead;
		
			Batch = new SpriteBatch(Device);

			_content = new ContentManager(GameMgr.Game.Services);
			_content.RootDirectory = AssetMgr.ContentDir + '/' + AssetMgr.EffectsDir;
			_defaultEffect = _content.Load<Effect>(_defaultEffectName);
			_defaultEffect.Parameters["World"].SetValue(Matrix.CreateTranslation(Vector3.Zero));
			

			_vertexBuffer = new DynamicVertexBuffer(Device, typeof(VertexPositionColorTexture), _vertexBufferSize, BufferUsage.WriteOnly);
			_indexBuffer = new DynamicIndexBuffer(Device, IndexElementSize.SixteenBits, _vertexBufferSize, BufferUsage.WriteOnly);
			
			Device.SetVertexBuffer(_vertexBuffer);
			Device.Indices = _indexBuffer;
			
			CircleShape.CircleVerticesCount = 16;
			
			_cameraRasterizerState = new RasterizerState
			{
				CullMode = CullMode.None,
				ScissorTestEnable = false,
				FillMode = FillMode.Solid
			};
		}


		/// <summary>
		/// Performs Draw events for all objects.
		/// </summary>
		public static void Update(GameTime gameTime)
		{
			__drawcalls = 0;
			
			#region Canvas matrix.

			var windowManager = GameMgr.WindowManager;
			if (!windowManager.IsFullScreen || windowManager.CanvasMode == CanvasMode.None)
			{
				CanvasMatrix = Matrix.CreateTranslation(Vector3.Zero);
			}
			if (windowManager.IsFullScreen)
			{
				// Fills the display with canvas.
				if (windowManager.CanvasMode == CanvasMode.Fill)
				{
					CanvasMatrix = Matrix.CreateScale(
					 new Vector3(
							windowManager.PreferredBackBufferWidth / (float)windowManager.CanvasWidth,
							windowManager.PreferredBackBufferHeight / (float)windowManager.CanvasHeight,
							1
						)
					);
				}
				// Fills the display with canvas.
				
				// Scales display to match canvas, but keeps aspect ratio.
				if (windowManager.CanvasMode == CanvasMode.KeepAspectRatio)
				{
					var backbufferSize = new Vector2(
						windowManager.PreferredBackBufferWidth,
						windowManager.PreferredBackBufferHeight
					);
					float ratio,
						offsetX = 0,
						offsetY = 0;

					float backbufferRatio = windowManager.PreferredBackBufferWidth / (float)windowManager.PreferredBackBufferHeight;
					float canvasRatio = windowManager.CanvasWidth / windowManager.CanvasHeight;

					if (canvasRatio > backbufferRatio)
					{
						ratio = windowManager.PreferredBackBufferWidth / (float)windowManager.CanvasWidth;
						offsetY = (windowManager.PreferredBackBufferHeight - (windowManager.CanvasHeight * ratio)) / 2;
					}
					else
					{
						ratio = windowManager.PreferredBackBufferHeight / (float)windowManager.CanvasHeight;
						offsetX = (windowManager.PreferredBackBufferWidth - (windowManager.CanvasWidth * ratio)) / 2;
					}
					
					CanvasMatrix = Matrix.CreateScale(new Vector3(ratio, ratio, 1)) * Matrix.CreateTranslation(new Vector3(offsetX, offsetY, 0));
				}
				// Scales display to match canvas, but keeps aspect ratio.
			}
			
			#endregion Canvas matrix.


			
			#region Main draw events.
			
			foreach(var camera in CameraMgr.Cameras)
			{
				if (camera.Enabled)
				{
					// Updating current transform matrix and camera.
					camera.UpdateTransformMatrix();
					CurrentCamera = camera;
					CurrentTransformMatrix = camera.TransformMatrix;
					CurrentProjection = Matrix.CreateOrthographicOffCenter(0, camera.Size.X, camera.Size.Y, 0, 0, 1);
					// Updating current transform matrix and camera.

					Input.MousePosition = camera.GetRelativeMousePosition();

					SetSurfaceTarget(camera.Surface, camera.TransformMatrix);
					
					if (camera.ClearBackground)
					{
						Device.Clear(camera.BackgroundColor);
					}

					SceneMgr.CallDrawEvents();
					
					ResetSurfaceTarget();
				}
			}
			#endregion Main draw events.


			// Resetting camera, transform matrix and mouse position.
			CurrentCamera = null;
			CurrentTransformMatrix = CanvasMatrix;
			Input.MousePosition = Input.ScreenMousePosition;
			// Resetting camera, transform matrix and mouse position
			

			// Drawing camera surfaces.
			Device.Clear(Color.TransparentBlack);
			
			// We don't need in-game rasterizer to apply to camera surfaces.
			var oldRasterizerState = _rasterizer;
			var oldBlendState = _blendState;

			_rasterizer = _cameraRasterizerState;
			_blendState = BlendState.AlphaBlend;

			foreach(var camera in CameraMgr.Cameras)
			{
				if (camera.Visible && camera.Enabled)
				{
					camera.Render();
				}
			}
			SwitchGraphicsMode(GraphicsMode.None);
			_rasterizer = oldRasterizerState;
			_blendState = oldBlendState;
			// Drawing camera surfaces.

			
			// Drawing GUI stuff.
			_currentGraphicsMode = GraphicsMode.None;
			
			SceneMgr.CallDrawGUIEvents();

			if (_currentGraphicsMode == GraphicsMode.Sprites) // If there's something left in batch or vertex buffer, we should draw it.
			{
				Batch.End();
			}
			DrawVertices();
			// Drawing GUI stuff.


			_currentGraphicsMode = GraphicsMode.None;

			// Safety checks.
			if (_surfaceStack.Count != 0)
			{
				throw new InvalidOperationException("Unbalanced surface stack! Did you forgot to reset a surface somewhere?");
			}

			if (_transformMatrixStack.Count != 0)
			{
				throw new InvalidOperationException("Unbalanced matrix stack! Did you forgot to reset a matrix somewhere?");
			}
			// Safety checks.
		}



		#region Base.

		/// <summary>
		/// Switches graphics mode.
		/// 
		/// Call it before manually using sprite batches or vertex buffers.
		/// </summary>
		public static void SwitchGraphicsMode(GraphicsMode mode, Texture2D texture = null)
		{
			if (mode != _currentGraphicsMode || texture != _currentTexture) // No need to switch to same graphics mode.
			{
				// Ending drawing stuff of previous call.
				if (_currentGraphicsMode != GraphicsMode.None)
				{
					if (_currentGraphicsMode == GraphicsMode.Sprites || _currentGraphicsMode == GraphicsMode.SpritesNonPremultiplied)
					{
						Batch.End();
					}
					else
					{
						DrawVertices();
					}
				}
				// Ending drawing stuff of previous call.

				if (mode == GraphicsMode.Sprites || mode == GraphicsMode.SpritesNonPremultiplied)
				{
					Device.ScissorRectangle = _scissorRectangle;
					
					Effect resultingEffect;

					if (_currentEffect == null)
					{
						resultingEffect = _defaultEffect;
						resultingEffect.Parameters["View"].SetValue(CurrentTransformMatrix);
						resultingEffect.Parameters["Projection"].SetValue(CurrentProjection);
						if (mode == GraphicsMode.Sprites)
						{
							resultingEffect.CurrentTechnique = _defaultEffect.Techniques["TexturePremultiplied"];
						}
						else
						{
							resultingEffect.CurrentTechnique = _defaultEffect.Techniques["TextureNonPremultiplied"];
						}
					}
					else
					{
						resultingEffect = _currentEffect;
					}

					Batch.Begin(SpriteSortMode.Deferred, _blendState, _sampler, null, _rasterizer, resultingEffect, CurrentTransformMatrix);
				}
				_currentGraphicsMode = mode;
				_currentTexture = texture;
			}
		}



		/// <summary>
		/// Adds vertices and indices to global vertex and index list.
		/// If current and suggested graphics modes are different, draws accumulated vertices first.
		/// </summary>
		internal static void AddVertices(GraphicsMode mode, Texture2D texture, List<VertexPositionColorTexture> vertices, short[] indices)
		{
			if (_indices.Count + indices.Length >= _vertexBufferSize)
			{
				DrawVertices(); // If buffer overflows, we need to empty it.
			}

			SwitchGraphicsMode(mode, texture);

			var indicesCopy= new short[indices.Length];
			Array.Copy(indices, indicesCopy, indices.Length); // We must copy an array to prevent modifying original.

			for(var i = 0; i < indices.Length; i += 1)
			{
				indicesCopy[i] += (short)_vertices.Count; // We need to offset each index because of single buffer for everything.
			} 

			_vertices.AddRange(vertices);
			_indices.AddRange(indicesCopy);
		}



		/// <summary>
		/// Draws vertices from vertex buffer and empties it.
		/// </summary>
		private static void DrawVertices()
		{
			Effect resultingEffect;

			if (_currentEffect == null)
			{
				resultingEffect = _defaultEffect;
			}
			else
			{
				resultingEffect = _currentEffect;
			}

			resultingEffect.Parameters["View"].SetValue(CurrentTransformMatrix);
			resultingEffect.Parameters["Projection"].SetValue(CurrentProjection);

			__drawcalls += 1;

			if (_vertices.Count > 0)
			{
				if (_currentTexture != null)
				{
					resultingEffect.Parameters["BasicTexture"].SetValue(_currentTexture);
					resultingEffect.CurrentTechnique = _defaultEffect.Techniques["TexturePremultiplied"];
				}
				else
				{
					resultingEffect.Parameters["BasicTexture"].SetValue((Texture2D)null);
					resultingEffect.CurrentTechnique = _defaultEffect.Techniques["Basic"];
				}

				PrimitiveType type;
				int prCount;

				if (_currentGraphicsMode == GraphicsMode.LinePrimitives)
				{
					type = PrimitiveType.LineList;
					prCount = _indices.Count / 2;
				}
				else
				{
					type = PrimitiveType.TriangleList;
					prCount = _indices.Count / 3;
				}
				
				// Passing primitive data to the buffers.
				_vertexBuffer.SetData(_vertices.ToArray(), 0, _vertices.Count, SetDataOptions.None);
				_indexBuffer.SetData(_indices.ToArray(), 0, _indices.Count);
				// Passing primitive data to the buffers.
				
				if (_rasterizer != null)
				{
					Device.RasterizerState = _rasterizer;
				}

				if (_sampler != null)
				{
					Device.SamplerStates[0] = _sampler;
				}
				
				if (_blendState != null)
				{
					Device.BlendState = _blendState;
				}

				Device.ScissorRectangle = _scissorRectangle;
		
				foreach(var pass in _defaultEffect.CurrentTechnique.Passes)
				{
					pass.Apply();
					Device.DrawIndexedPrimitives(type, 0, 0, prCount);
				}
				
				_vertices.Clear();
				_indices.Clear();
				
			}
		}

		#endregion Base.

		
		
		#region Matrices.

		/// <summary>
		/// Sets new transform matrix.
		/// </summary>
		public static void SetTransformMatrix(Matrix matrix)
		{
			SwitchGraphicsMode(GraphicsMode.None);
			_transformMatrixStack.Push(CurrentTransformMatrix);
			CurrentTransformMatrix = matrix;
		}

		/// <summary>
		/// Sets new transform matrix multiplied by current transform matrix.
		/// </summary>
		public static void AddTransformMatrix(Matrix matrix)
		{
			SwitchGraphicsMode(GraphicsMode.None);
			_transformMatrixStack.Push(CurrentTransformMatrix);
			CurrentTransformMatrix = matrix * CurrentTransformMatrix;
		}

		/// <summary>
		/// Resets to a previous transform matrix.
		/// </summary>
		public static void ResetTransformMatrix()
		{
			if (_transformMatrixStack.Count == 0)
			{
				throw new InvalidOperationException("Matrix stack is empty! Did you forgot to set a matrix somewhere?");
			}

			SwitchGraphicsMode(GraphicsMode.None); 
			CurrentTransformMatrix = _transformMatrixStack.Pop();
		}

		#endregion Matrices.



		


		#region Surfaces.

		/// <summary>
		/// Sets surface as a render target.
		/// </summary>
		public static void SetSurfaceTarget(Surface surf) => 
			SetSurfaceTarget(surf, Matrix.CreateTranslation(Vector3.Zero));

		/// <summary>
		/// Sets surface as a render target.
		/// </summary>
		/// <param name="surf">Target surface.</param>
		/// <param name="matrix">Surface transformation matrix.</param>
		public static void SetSurfaceTarget(Surface surf, Matrix matrix)
		{
			SetTransformMatrix(matrix);

			_surfaceStack.Push(_currentSurface);
			_currentSurface = surf;

			CurrentProjection = Matrix.CreateOrthographicOffCenter(0,	_currentSurface.Width, _currentSurface.Height, 0, 0, 1);

			Device.SetRenderTarget(_currentSurface.RenderTarget);
		}

		/// <summary>
		/// Resets render target to a previous surface.
		/// </summary>
		public static void ResetSurfaceTarget()
		{
			ResetTransformMatrix();

			if (_surfaceStack.Count == 0)
			{
				throw new InvalidOperationException("Surface stack is empty! Did you forgot to set a surface somewhere?");
			}
			_currentSurface = _surfaceStack.Pop();

			if (_currentSurface != null)
			{
				CurrentProjection = Matrix.CreateOrthographicOffCenter(0,	_currentSurface.Width, _currentSurface.Height, 0, 0, 1);
				
				Device.SetRenderTarget(_currentSurface.RenderTarget);
			}
			else
			{
				CurrentProjection = Matrix.CreateOrthographicOffCenter(
					0, 
					GameMgr.WindowManager.PreferredBackBufferWidth, 
					GameMgr.WindowManager.PreferredBackBufferHeight, 
					0,
					0,
					1
				);
				
				Device.SetRenderTarget(null);
			}
		}
		
		#endregion Surfaces.
		
		
	}
}