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

		
		/// <summary>
		/// Current drawing color. Affects shapes, sprites, text and primitives.
		/// </summary>
		public static Color CurrentColor = Color.White;
		
		
		/// <summary>
		/// Used for drawing cameras.
		/// </summary>
		internal static RasterizerState _cameraRasterizerState;

		
		/// <summary>
		/// Matrix for offsetting, scaling and rotating canvas contents.
		/// </summary>
		public static Matrix CanvasMatrix = Matrix.CreateTranslation(Vector3.Zero); //We need zero matrix here, or else mouse position will derp out.
		
		/// <summary>
		/// Initialization function for draw manager. 
		/// </summary>
		public static void Init(GraphicsDevice device)
		{
			Device = device;			
			Device.DepthStencilState = DepthStencilState.DepthRead;

			
			CircleShape.CircleVerticesCount = 16;
			
			_cameraRasterizerState = new RasterizerState
			{
				CullMode = CullMode.None,
				ScissorTestEnable = false,
				FillMode = FillMode.Solid
			};
			VertexBatch = new VertexBatch(Device);
			

			VertexBatch.World = Matrix.CreateTranslation(Vector3.Zero);
		}


		/// <summary>
		/// Performs Draw events for all objects.
		/// </summary>
		public static void Update(GameTime gameTime)
		{
			
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
					CurrentCamera = camera;
					
					Surface.SetTarget(
						camera.Surface, 
						camera.ConstructViewMatrix(), 
						camera.ConstructProjectionMatrix()
					);
					
					Input.MousePosition = camera.GetRelativeMousePosition();
					
					if (camera.ClearBackground)
					{
						Device.Clear(camera.BackgroundColor);
					}

					SceneMgr.CallDrawEvents();
					VertexBatch.FlushBatch();

					Surface.ResetTarget();
				}
			}
			#endregion Main draw events.


			// Resetting camera, transform matrix and mouse position.
			CurrentCamera = null;
			VertexBatch.PushProjectionMatrix(
				Matrix.CreateOrthographicOffCenter(
					0,
					GameMgr.WindowManager.PreferredBackBufferWidth,
					GameMgr.WindowManager.PreferredBackBufferHeight,
					0,
					0,
					1
				)
			);
			VertexBatch.PushViewMatrix(CanvasMatrix);
			Input.MousePosition = Input.ScreenMousePosition;
			// Resetting camera, transform matrix and mouse position
			

			// Drawing camera surfaces.
			Device.Clear(Color.TransparentBlack);
			
			// We don't need in-game rasterizer to apply to camera surfaces.
			var oldRasterizerState = VertexBatch.RasterizerState;
			var oldBlendState = VertexBatch.BlendState;

			VertexBatch.RasterizerState = _cameraRasterizerState;
			VertexBatch.BlendState = BlendState.AlphaBlend;

			foreach(var camera in CameraMgr.Cameras)
			{
				if (camera.Visible && camera.Enabled)
				{
					camera.Render();
				}
			}

			VertexBatch.FlushBatch();

			VertexBatch.RasterizerState = oldRasterizerState;
			VertexBatch.BlendState = oldBlendState;
			// Drawing camera surfaces.

			
			// Drawing GUI stuff.
			SceneMgr.CallDrawGUIEvents();
			
			VertexBatch.FlushBatch();
			// Drawing GUI stuff.

			VertexBatch.PopViewMatrix();
			VertexBatch.PopProjectionMatrix();
			
			// Safety checks.
			if (!Surface.SurfaceStackEmpty)
			{
				throw new InvalidOperationException("Unbalanced surface stack! Did you forgot to reset a surface somewhere?");
			}
			if (!VertexBatch.WorldStackEmpty)
			{
				throw new InvalidOperationException("Unbalanced World matrix stack! Did you forgot to pop a matrix somewhere?");
			}
			if (!VertexBatch.ViewStackEmpty)
			{
				throw new InvalidOperationException("Unbalanced View matrix stack! Did you forgot to pop a matrix somewhere?");
			}
			if (!VertexBatch.ProjectionStackEmpty)
			{
				throw new InvalidOperationException("Unbalanced Projection matrix stack! Did you forgot to pop a matrix somewhere?");
			}
			// Safety checks.
		}
		
	}
}