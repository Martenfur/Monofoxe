using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Monofoxe.Engine.SceneSystem;
using Monofoxe.Engine.Cameras;

namespace Monofoxe.Engine.Drawing
{

	public static class GraphicsMgr
	{
		/// <summary>
		/// Default sprite batch used to draw sprites, text and surfaces.
		/// </summary>
		//internal static SpriteBatch _batch {get; private set;}

		public static VertexBatch VertexBatch;

		public static GraphicsDevice Device {get; private set;}
		
		/// <summary>
		/// Currently enabled camera.
		/// </summary>
		public static Camera CurrentCamera {get; private set;}

		public static Matrix CurrentWorld {get; private set;}
		/// <summary>
		/// Current view. Used to offset, rotate and scale graphics.
		/// </summary>
		public static Matrix CurrentView {get; private set;}
		public static Matrix CurrentProjection {get; private set;}

		private static Stack<Matrix> _transformMatrixStack = new Stack<Matrix>();

		
		/// <summary>
		/// Current drawing color. Affects shapes, sprites, text and primitives.
		/// </summary>
		public static Color CurrentColor = Color.White;
		
		
		/// <summary>
		/// Amount of draw calls per frame.
		/// TODO: Remove.
		/// </summary>
		public static int __drawcalls {get; private set;}
		
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
				VertexBatch.FlushBatch(); // TODO: Add scissor rectangle support.
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
				VertexBatch.RasterizerState = value; 
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
				VertexBatch.SamplerState = value; 
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
				VertexBatch.BlendState = value; 
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
				VertexBatch.Effect = value;
				_currentEffect = value;
			}
			get => _currentEffect;
		}
		private static Effect _currentEffect;
		
		/// <summary>
		/// Default shader with proper alpha blending. 
		/// Replaces BasicEffect. Applied, when CurrentEffect is null.
		/// </summary>
		public static Effect _defaultEffect;
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
		
			//_batch = new SpriteBatch(Device);


			_content = new ContentManager(GameMgr.Game.Services);
			_content.RootDirectory = AssetMgr.ContentDir + '/' + AssetMgr.EffectsDir;
			_defaultEffect = _content.Load<Effect>(_defaultEffectName);
			
			CircleShape.CircleVerticesCount = 16;
			
			_cameraRasterizerState = new RasterizerState
			{
				CullMode = CullMode.None,
				ScissorTestEnable = false,
				FillMode = FillMode.Solid
			};
			
			CurrentWorld = Matrix.CreateTranslation(Vector3.Zero);

			VertexBatch = new VertexBatch(Device, _defaultEffect);
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
					float canvasRatio = windowManager.CanvasWidth / (float)windowManager.CanvasHeight;

					if (canvasRatio > backbufferRatio)
					{
						ratio = windowManager.PreferredBackBufferWidth / (float)windowManager.CanvasWidth;
						offsetY = (windowManager.PreferredBackBufferHeight - (windowManager.CanvasHeight * ratio)) / 2f;
					}
					else
					{
						ratio = windowManager.PreferredBackBufferHeight / (float)windowManager.CanvasHeight;
						offsetX = (windowManager.PreferredBackBufferWidth - (windowManager.CanvasWidth * ratio)) / 2f;
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
					CurrentView = camera.TransformMatrix;
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
			CurrentView = CanvasMatrix;
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
			VertexBatch.FlushBatch();

			VertexBatch.FlushBatch();
			_rasterizer = oldRasterizerState;
			_blendState = oldBlendState;
			// Drawing camera surfaces.

			
			// Drawing GUI stuff.
			
			SceneMgr.CallDrawGUIEvents();

			
			VertexBatch.FlushBatch();
			// Drawing GUI stuff.

			
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


		
		#region Matrices.

		/// <summary>
		/// Sets new transform matrix.
		/// </summary>
		public static void SetTransformMatrix(Matrix matrix)
		{
			VertexBatch.FlushBatch();
			_transformMatrixStack.Push(CurrentView);
			CurrentView = matrix;
		}

		/// <summary>
		/// Sets new transform matrix multiplied by current transform matrix.
		/// </summary>
		public static void AddTransformMatrix(Matrix matrix)
		{
			VertexBatch.FlushBatch();
			_transformMatrixStack.Push(CurrentView);
			CurrentView = matrix * CurrentView;
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

			VertexBatch.FlushBatch(); 
			CurrentView = _transformMatrixStack.Pop();
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