using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Monofoxe.Engine.Drawing;
using Monofoxe.Engine.SceneSystem;
using Monofoxe.Engine.Utils.Cameras;

namespace Monofoxe.Engine
{

	public static class DrawMgr
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
		/// Current pipeline mode. Tells, which type of graphics is being drawn right now.
		/// </summary>
		private static PipelineMode _currentPipelineMode = PipelineMode.None;
		private static Texture2D _currentTexture;
		
		/// <summary>
		/// We can set surface targets inside another surfaces.
		/// </summary>
		private static Stack<RenderTarget2D> _surfaceStack = new Stack<RenderTarget2D>();
		private static RenderTarget2D _currentSurface;

		#region Modifiers.

		/// <summary>
		/// Disables rendering for everything that's outside of rectangle.
		/// NOTE: To enable scissoring, enable scissor test in Rasterizer.
		/// </summary>
		public static Rectangle ScissorRectangle
		{
			set
			{
				SwitchPipelineMode(PipelineMode.None);
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
				SwitchPipelineMode(PipelineMode.None); 
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
				SwitchPipelineMode(PipelineMode.None); 
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
				SwitchPipelineMode(PipelineMode.None); 
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
				SwitchPipelineMode(PipelineMode.None);
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
		

		#region Shapes.

		private static readonly PipelineMode[] _shapePipelineModes = {PipelineMode.TrianglePrimitives, PipelineMode.OutlinePrimitives};
		
		// Triangle.
		private static readonly short[][] _triangleIndices = 
		{
			new short[]{0, 1, 2}, // Filled.
			new short[]{0, 1, 1, 2, 2, 0} // Outline.
		};
		// Triangle.

		// Rectangle.
		private static readonly short[][] _rectangleIndices = 
		{
			new short[]{0, 1, 3, 1, 2, 3}, // Filled.
			new short[]{0, 1, 1, 2, 2, 3, 3, 0} // Outline.
		};
		// Rectangle.

		// Circle.
		/// <summary>
		/// Amount of vertices in one circle. 
		/// </summary>
		public static int CircleVerticesCount 
		{
			set
			{
				_circleVectors = new List<Vector2>();
			
				var angAdd = Math.PI * 2 / value;
				
				for(var i = 0; i < value; i += 1)
				{
					_circleVectors.Add(new Vector2((float)Math.Cos(angAdd * i), (float)Math.Sin(angAdd * i)));
				}
			}
			get => _circleVectors.Count;
		}

		private static List<Vector2> _circleVectors; 
		// Circle.

		#endregion Shapes.


		// Primitives.
		private static List<VertexPositionColorTexture> _primitiveVertices = new List<VertexPositionColorTexture>();
		private static List<short> _primitiveIndices = new List<short>();
		private static PipelineMode _primitiveType = PipelineMode.None;
		private static Texture2D _primitiveTexture;

		private static Vector2 _primitiveTextureOffset;
		private static Vector2 _primitiveTextureRatio;
		// Primitives.

		
		// Text.
		public static IFont CurrentFont;

		public static TextAlign HorAlign = TextAlign.Left;
		public static TextAlign VerAlign = TextAlign.Top;
		// Text.

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
			
			CircleVerticesCount = 16;
			
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
							windowManager.PreferredBackBufferWidth / (float)windowManager.CanvasW,
							windowManager.PreferredBackBufferHeight / (float)windowManager.CanvasH,
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
					float canvasRatio = windowManager.CanvasW / windowManager.CanvasH;

					if (canvasRatio > backbufferRatio)
					{
						ratio = windowManager.PreferredBackBufferWidth / (float)windowManager.CanvasW;
						offsetY = (windowManager.PreferredBackBufferHeight - (windowManager.CanvasH * ratio)) / 2;
					}
					else
					{
						ratio = windowManager.PreferredBackBufferHeight / (float)windowManager.CanvasH;
						offsetX = (windowManager.PreferredBackBufferWidth - (windowManager.CanvasW * ratio)) / 2;
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

					Input.MousePos = camera.GetRelativeMousePosition();

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
			Input.MousePos = Input.ScreenMousePos;
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
			SwitchPipelineMode(PipelineMode.None);
			_rasterizer = oldRasterizerState;
			_blendState = oldBlendState;
			// Drawing camera surfaces.

			
			// Drawing GUI stuff.
			_currentPipelineMode = PipelineMode.None;
			
			SceneMgr.CallDrawGUIEvents();

			if (_currentPipelineMode == PipelineMode.Sprites) // If there's something left in batch or vertex buffer, we should draw it.
			{
				Batch.End();
			}
			DrawVertices();
			// Drawing GUI stuff.


			_currentPipelineMode = PipelineMode.None;

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



		#region Pipeline.

		/// <summary>
		/// Switches graphics pipeline mode.
		/// 
		/// Call it before manually using sprite batches or vertex buffers.
		/// </summary>
		public static void SwitchPipelineMode(PipelineMode mode, Texture2D texture = null)
		{
			if (mode != _currentPipelineMode || texture != _currentTexture) // No need to switch to same pipeline mode.
			{
				// Ending drawing stuff of previous call.
				if (_currentPipelineMode != PipelineMode.None)
				{
					if (_currentPipelineMode == PipelineMode.Sprites || _currentPipelineMode == PipelineMode.SpritesNonPremultiplied)
					{
						Batch.End();
					}
					else
					{
						DrawVertices();
					}
				}
				// Ending drawing stuff of previous call.

				if (mode == PipelineMode.Sprites || mode == PipelineMode.SpritesNonPremultiplied)
				{
					Device.ScissorRectangle = _scissorRectangle;
					
					Effect resultingEffect;

					if (_currentEffect == null)
					{
						resultingEffect = _defaultEffect;
						resultingEffect.Parameters["View"].SetValue(CurrentTransformMatrix);
						resultingEffect.Parameters["Projection"].SetValue(CurrentProjection);
						if (mode == PipelineMode.Sprites)
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
				_currentPipelineMode = mode;
				_currentTexture = texture;
			}
		}



		/// <summary>
		/// Adds vertices and indices to global vertex and index list.
		/// If current and suggested pipeline modes are different, draws accumulated vertices first.
		/// </summary>
		private static void AddVertices(PipelineMode mode, Texture2D texture, List<VertexPositionColorTexture> vertices, short[] indices)
		{
			if (_indices.Count + indices.Length >= _vertexBufferSize)
			{
				DrawVertices(); // If buffer overflows, we need to empty it.
			}

			SwitchPipelineMode(mode, texture);

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

				if (_currentPipelineMode == PipelineMode.OutlinePrimitives)
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

		#endregion Pipeline.

		
		
		#region Matrices.

		/// <summary>
		/// Sets new transform matrix.
		/// </summary>
		public static void SetTransformMatrix(Matrix matrix)
		{
			SwitchPipelineMode(PipelineMode.None);
			_transformMatrixStack.Push(CurrentTransformMatrix);
			CurrentTransformMatrix = matrix;
		}

		/// <summary>
		/// Sets new transform matrix multiplied by current transform matrix.
		/// </summary>
		public static void AddTransformMatrix(Matrix matrix)
		{
			SwitchPipelineMode(PipelineMode.None);
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

			SwitchPipelineMode(PipelineMode.None); 
			CurrentTransformMatrix = _transformMatrixStack.Pop();
		}

		#endregion Matrices.



		#region Sprites.

		public static void DrawFrame(
			Frame frame, 
			Vector2 pos, 
			Vector2 scale, 
			float rotation, 
			Vector2 offset, 
			Color color, 
			SpriteEffects effect
		)
		{
			SwitchPipelineMode(PipelineMode.Sprites);

			Batch.Draw(
				frame.Texture, 
				pos, 
				frame.TexturePosition, 
				color, 
				MathHelper.ToRadians(rotation), 
				offset + frame.Origin,
				scale, 
				effect, 
				0
			);
		}

		/// <summary>
		/// Returns sprite frame based on a value from 0 to 1.
		/// </summary>
		public static Frame CalculateSpriteFrame(Sprite sprite, double animation) =>
			sprite.Frames[Math.Max(0, Math.Min(sprite.Frames.Length - 1, (int)(animation * sprite.Frames.Length)))];
		
		// Vectors.

		public static void DrawSprite(Sprite sprite, Vector2 pos) =>
			DrawFrame(sprite.Frames[0], pos, Vector2.One, 0, sprite.Origin, CurrentColor, SpriteEffects.None);
		
		public static void DrawSprite(Sprite sprite, double animation, Vector2 pos) =>
			DrawFrame(CalculateSpriteFrame(sprite, animation), pos, sprite.Origin);
		
		public static void DrawSprite(Sprite sprite, double animation, Vector2 pos, Vector2 scale, float rotation, Color color) =>
			DrawFrame(CalculateSpriteFrame(sprite, animation), pos, sprite.Origin, scale, rotation, color);
		
		public static void DrawSprite(Sprite sprite, double animation, Vector2 pos, Vector2 offset, Vector2 scale, float rotation, Color color) =>
			DrawFrame(CalculateSpriteFrame(sprite, animation), pos, sprite.Origin + offset, scale, rotation, color);

		public static void DrawFrame(Frame frame, Vector2 pos, Vector2 offset) =>
			DrawFrame(frame, pos, Vector2.One, 0, offset, CurrentColor, SpriteEffects.None);
		
		public static void DrawFrame(Frame frame, Vector2 pos, Vector2 offset, Vector2 scale, float rotation, Color color)
		{
			var mirroring = SpriteEffects.None;

			// Proper negative scaling.
			
			if (scale.X < 0)
			{
				mirroring = mirroring | SpriteEffects.FlipHorizontally;
				scale.X *= -1;
				offset.X += frame.W;
			}

			if (scale.Y < 0)
			{
				mirroring = mirroring | SpriteEffects.FlipVertically;
				scale.Y *= -1;
				offset.Y += frame.H;
			}
			// Proper negative scaling.

			DrawFrame(frame, pos, scale, rotation, frame.Origin + offset, color, mirroring);
		}

		// Vectors.
		
		public static void DrawSprite(Sprite sprite, float x, float y) =>
			DrawSprite(sprite, new Vector2(x, y));
		
		public static void DrawSprite(Sprite sprite, double animation, float x, float y) =>
			DrawSprite(sprite, animation, new Vector2(x, y));
		
		public static void DrawSprite(Sprite sprite, double animation, float x, float y, float scaleX, float scaleY, float rotation, Color color) =>
			DrawSprite(sprite, animation, new Vector2(x, y), new Vector2(scaleX, scaleY), rotation, color);
		
		public static void DrawFrame(Frame frame, float x, float y, float offsetX, float offsetY) =>
			DrawFrame(frame, new Vector2(x, y), new Vector2(offsetX, offsetY));
		
		public static void DrawFrame(Frame frame, float x, float y, float offsetX, float offsetY, float scaleX, float scaleY, float rotation, Color color) =>
			DrawFrame(frame, new Vector2(x, y), new Vector2(offsetX, offsetY), new Vector2(scaleX, scaleY), rotation, color);
		
		// Floats.

		// Rectangles.

		public static void DrawSprite(Sprite sprite, double animation, Rectangle destRect) =>
			DrawFrame(CalculateSpriteFrame(sprite, animation), sprite.Origin, destRect, 0, CurrentColor);

		public static void DrawSprite(Sprite sprite, double animation, Rectangle destRect, float rotation, Color color) =>
			DrawFrame(CalculateSpriteFrame(sprite, animation), sprite.Origin, destRect, rotation, color);

		public static void DrawSprite(Sprite sprite, double animation, Rectangle destRect, Rectangle srcRect) =>
			DrawFrame(CalculateSpriteFrame(sprite, animation), sprite.Origin, destRect, srcRect, 0, CurrentColor);

		public static void DrawSprite(Sprite sprite, double animation, Rectangle destRect, Rectangle srcRect, float rotation, Color color) =>
			DrawFrame(CalculateSpriteFrame(sprite, animation), sprite.Origin, destRect, srcRect, rotation, color);
		
		public static void DrawFrame(Frame frame, Vector2 offset, Rectangle destRect, float rotation, Color color)
		{
			SwitchPipelineMode(PipelineMode.Sprites);
			
			Batch.Draw(
				frame.Texture, 
				destRect, 
				frame.TexturePosition, 
				color, 
				rotation,
				offset + frame.Origin, 
				SpriteEffects.None, 
				0
			);
		}

		public static void DrawFrame(Frame frame, Vector2 offset, Rectangle destRect, Rectangle srcRect, float rotation, Color color)
		{
			SwitchPipelineMode(PipelineMode.Sprites);
			
			srcRect.X += frame.TexturePosition.X;
			srcRect.Y += frame.TexturePosition.Y;

			Batch.Draw(
				frame.Texture,
				destRect, 
				srcRect, 
				color, 
				rotation, 
				offset + frame.Origin,
				SpriteEffects.None, 
				0
			);
		}

		// Rectangles.
		
		#endregion Sprites.



		#region Shapes.


		#region Line overloads.
		
		/// <summary>
		/// Draws a line.
		/// </summary>
		public static void DrawLine(Vector2 p1, Vector2 p2) =>
			DrawLine(p1.X, p1.Y, p2.X, p2.Y, CurrentColor, CurrentColor);

		/// <summary>
		/// Draws a line with specified colors.
		/// </summary>
		public static void DrawLine(Vector2 p1, Vector2 p2, Color c1, Color c2) =>
			DrawLine(p1.X, p1.Y, p2.X, p2.Y, c1, c2);

		/// <summary>
		/// Draws a line with specified width and colors.
		/// </summary>
		public static void DrawLine(Vector2 p1, Vector2 p2, float w, Color c1, Color c2) =>
			DrawLine(p1.X, p1.Y, p2.X, p2.Y, w, c1, c2);

		/// <summary>
		/// Draws a line.
		/// </summary>
		public static void DrawLine(float x1, float y1, float x2, float y2) =>
			DrawLine(x1, y1, x2, y2, CurrentColor, CurrentColor);

		/// <summary>
		/// Draws a line with specified width.
		/// </summary>
		public static void DrawLine(float x1, float y1, float x2, float y2, float w) =>
			DrawLine(x1, y1, x2, y2, w, CurrentColor, CurrentColor);
		
		#endregion Line overloads.
		
		/// <summary>
		/// Draws a line with specified colors.
		/// </summary>
		public static void DrawLine(float x1, float y1, float x2, float y2, Color c1, Color c2)
		{
			var vertices = new List<VertexPositionColorTexture>
			{
				new VertexPositionColorTexture(new Vector3(x1, y1, 0), c1, Vector2.Zero),
				new VertexPositionColorTexture(new Vector3(x2, y2, 0), c2, Vector2.Zero)
			};

			AddVertices(PipelineMode.OutlinePrimitives, null, vertices, new short[]{0, 1});
		}

		/// <summary>
		/// Draws a line with specified width and colors.
		/// </summary>
		public static void DrawLine(float x1, float y1, float x2, float y2, float w, Color c1, Color c2)
		{
			var normal = new Vector3(y2 - y1, x1 - x2, 0);
			normal.Normalize(); // The result is a unit vector rotated by 90 degrees.
			normal *= w / 2;

			var vertices = new List<VertexPositionColorTexture>
			{
				new VertexPositionColorTexture(new Vector3(x1, y1, 0) + normal, c1, Vector2.Zero),
				new VertexPositionColorTexture(new Vector3(x1, y1, 0) - normal, c1, Vector2.Zero),
				new VertexPositionColorTexture(new Vector3(x2, y2, 0) - normal, c2, Vector2.Zero),
				new VertexPositionColorTexture(new Vector3(x2, y2, 0) + normal, c2, Vector2.Zero)
			};

			AddVertices(PipelineMode.TrianglePrimitives, null, vertices, _rectangleIndices[1]); // Thick line is in fact just a rotated rectangle.
		}


		#region Triangle overloads.

		/// <summary>
		/// Draws a triangle.
		/// </summary>
		public static void DrawTriangle(Vector2 p1, Vector2 p2, Vector2 p3, bool isOutline) =>
			DrawTriangle(p1.X, p1.Y, p2.X, p2.Y, p3.X, p3.Y, isOutline, CurrentColor, CurrentColor, CurrentColor);

		/// <summary>
		/// Draws a triangle with specified colors.
		/// </summary>
		public static void DrawTriangle(Vector2 p1, Vector2 p2, Vector2 p3, bool isOutline, Color c1, Color c2, Color c3) =>
			DrawTriangle(p1.X, p1.Y, p2.X, p2.Y, p3.X, p3.Y, isOutline, c1, c2, c3);
		
		/// <summary>
		/// Draws a triangle.
		/// </summary>
		public static void DrawTriangle(float x1, float y1, float x2, float y2, float x3, float y3, bool isOutline) =>
			DrawTriangle(x1, y1, x2, y2, x3, y3, isOutline, CurrentColor, CurrentColor, CurrentColor);

		#endregion Triangle overloads.

		/// <summary>
		/// Draw a triangle with specified colors.
		/// </summary>
		public static void DrawTriangle(float x1, float y1, float x2, float y2, float x3, float y3, bool isOutline, Color c1, Color c2, Color c3)
		{
			int isOutlineInt = Convert.ToInt32(isOutline); // We need to convert true/false to 1/0 to be able to get different sets of values from arrays. 

			var vertices = new List<VertexPositionColorTexture>
			{
				new VertexPositionColorTexture(new Vector3(x1, y1, 0), c1, Vector2.Zero),
				new VertexPositionColorTexture(new Vector3(x2, y2, 0), c2, Vector2.Zero),
				new VertexPositionColorTexture(new Vector3(x3, y3, 0), c3, Vector2.Zero)
			};

			AddVertices(_shapePipelineModes[isOutlineInt], null, vertices, _triangleIndices[isOutlineInt]);
		}



		#region Rectangle overloads.

		/// <summary>
		/// Draws a rectangle.
		/// </summary>
		public static void DrawRectangle(Vector2 p1, Vector2 p2, bool isOutline) =>
			DrawRectangle(p1.X, p1.Y, p2.X, p2.Y, isOutline, CurrentColor, CurrentColor, CurrentColor, CurrentColor);

		/// <summary>
		/// Draws a rectangle with specified colors for each corner.
		/// </summary>
		public static void DrawRectangle(Vector2 p1, Vector2 p2, bool isOutline, Color c1, Color c2, Color c3, Color c4) =>
			DrawRectangle(p1.X, p1.Y, p2.X, p2.Y, isOutline, c1, c2, c3, c4);

		/// <summary>
		/// Draws a rectangle.
		/// </summary>
		public static void DrawRectangle(float x1, float y1, float x2, float y2, bool isOutline) =>
			DrawRectangle(x1, y1, x2, y2, isOutline, CurrentColor, CurrentColor, CurrentColor, CurrentColor);

		#endregion Rectangle overloads.

		/// <summary>
		/// Draws a rectangle with specified colors for each corner.
		/// </summary>
		public static void DrawRectangle(float x1, float y1, float x2, float y2, bool isOutline, Color c1, Color c2, Color c3, Color c4)
		{
			int isOutlineInt = Convert.ToInt32(isOutline); // We need to convert true/false to 1/0 to be able to get different sets of values from arrays. 

			var vertices = new List<VertexPositionColorTexture>
			{
				new VertexPositionColorTexture(new Vector3(x1, y1, 0), c1, Vector2.Zero),
				new VertexPositionColorTexture(new Vector3(x2, y1, 0), c2, Vector2.Zero),
				new VertexPositionColorTexture(new Vector3(x2, y2, 0), c3, Vector2.Zero),
				new VertexPositionColorTexture(new Vector3(x1, y2, 0), c4, Vector2.Zero)
			};

			AddVertices(_shapePipelineModes[isOutlineInt], null, vertices, _rectangleIndices[isOutlineInt]);
		}


		#region Circle overloads.

		/// <summary>
		/// Draws a circle.
		/// </summary>
		public static void DrawCircle(Vector2 p, float r, bool isOutline) =>
			DrawCircle(p.X, p.Y, r, isOutline);

		#endregion Circle overloads.

		/// <summary>
		/// Draws a circle.
		/// </summary>
		public static void DrawCircle(float x, float y, float r, bool isOutline)
		{
			short[] indexArray;
			PipelineMode prType;
			if (isOutline)
			{
				indexArray = new short[CircleVerticesCount * 2];
				prType = PipelineMode.OutlinePrimitives;
				
				for(var i = 0; i< CircleVerticesCount - 1; i += 1)
				{
					indexArray[i * 2] = (short)i;
					indexArray[i * 2 + 1] = (short)(i + 1);
				}
				indexArray[(CircleVerticesCount - 1) * 2] = (short)(CircleVerticesCount - 1);
				indexArray[(CircleVerticesCount - 1) * 2 + 1] = 0;
			}
			else
			{
				indexArray = new short[CircleVerticesCount * 3];
				prType = PipelineMode.TrianglePrimitives;

				for(var i = 0; i < CircleVerticesCount - 1; i += 1)
				{
					indexArray[i * 3] = 0;
					indexArray[i * 3] = (short)i;
					indexArray[i * 3 + 1] = (short)(i + 1);
				}

			}

			var vertices = new List<VertexPositionColorTexture>();
			
			for(var i = 0; i < CircleVerticesCount; i += 1)
			{
				vertices.Add(
					new VertexPositionColorTexture(
						new Vector3(
							x + r * _circleVectors[i].X, 
							y + r * _circleVectors[i].Y, 
							0
						), 
						CurrentColor, 
						Vector2.Zero
					)
				);
			}
			AddVertices(prType, null, vertices, indexArray);
		}

		#endregion Shapes.



		#region Primitives.
		
		/// <summary>
		/// Sets texture for a primitive.
		/// </summary>
		public static void PrimitiveSetTexture(Texture2D texture)
		{
			_primitiveTexture = texture;
			_primitiveTextureOffset = Vector2.Zero;
			_primitiveTextureRatio = Vector2.One;
		}

		/// <summary>
		/// Sets texture for a primitive.
		/// </summary>
		public static void PrimitiveSetTexture(Sprite sprite, float frameId)
		{
			SwitchPipelineMode(PipelineMode.None);
			var frame = sprite.Frames[(int)frameId];

			_primitiveTexture = frame.Texture;
			_primitiveTextureOffset = new Vector2(
				frame.TexturePosition.X / (float)frame.Texture.Width, 
				frame.TexturePosition.Y / (float)frame.Texture.Height
			);
			
			_primitiveTextureRatio = new Vector2(
				frame.TexturePosition.Width / (float)frame.Texture.Width, 
				frame.TexturePosition.Height / (float)frame.Texture.Height
			);
		}



		#region AddVertex overloads.

		public static void PrimitiveAddVertex(Vector2 pos) =>
			PrimitiveAddVertex(pos.X, pos.Y, CurrentColor, Vector2.Zero);

		public static void PrimitiveAddVertex(Vector2 pos, Color color) =>
			PrimitiveAddVertex(pos.X, pos.Y, color, Vector2.Zero);

		public static void PrimitiveAddVertex(Vector2 pos, Vector2 texturePos) =>
			PrimitiveAddVertex(pos.X, pos.Y, CurrentColor, texturePos);

		public static void PrimitiveAddVertex(Vector2 pos, Color color, Vector2 texturePos) =>
			PrimitiveAddVertex(pos.X, pos.Y, color, texturePos);

		public static void PrimitiveAddVertex(float x, float y) =>
			PrimitiveAddVertex(x, y, CurrentColor, Vector2.Zero);

		public static void PrimitiveAddVertex(float x, float y, Color color) =>
			PrimitiveAddVertex(x, y, color, Vector2.Zero);
	
		public static void PrimitiveAddVertex(float x, float y, Vector2 texturePos) =>
			PrimitiveAddVertex(x, y, CurrentColor, texturePos);
		
		#endregion AddVertex overloads.

		public static void PrimitiveAddVertex(float x, float y, Color color, Vector2 texturePos)
		{
			// Since we may work with sprites, which are only little parts of whole texture atlas,
			// we need to convert local sprite coordinates to global atlas coordinates.
			Vector2 atlasPos = _primitiveTextureOffset + texturePos * _primitiveTextureRatio;
			_primitiveVertices.Add(new VertexPositionColorTexture(new Vector3(x, y, 0), color, atlasPos));
		}



		/// <summary>
		/// Sets indices according to trianglestrip pattern.
		/// NOTE: Use counter-clockwise culling.
		/// </summary>
		public static void PrimitiveSetTriangleStripIndices()
		{
			// 0 - 2 - 4
			//  \ / \ /
			//   1 - 3
			
			_primitiveType = PipelineMode.TrianglePrimitives;

			var flip = true;
			for(var i = 0; i < _primitiveVertices.Count - 2; i += 1)
			{
				_primitiveIndices.Add((short)i);
				if (flip) // Taking in account counter-clockwise culling.
				{
					_primitiveIndices.Add((short)(i + 2));
					_primitiveIndices.Add((short)(i + 1));
				}
				else
				{
					_primitiveIndices.Add((short)(i + 1));
					_primitiveIndices.Add((short)(i + 2));
				}
				flip = !flip;
			}
		}
		
		/// <summary>
		/// Sets indexes according to trianglefan pattern.
		/// NOTE: Use counter-clockwise culling.
		/// </summary>
		public static void PrimitiveSetTriangleFanIndices()
		{
			//   1
			//  / \
			// 0 - 2 
			//  \ / 
			//   3 

			_primitiveType = PipelineMode.TrianglePrimitives;

			for(var i = 1; i < _primitiveVertices.Count - 1; i += 1)
			{
				_primitiveIndices.Add(0);
				_primitiveIndices.Add((short)i);
				_primitiveIndices.Add((short)(i + 1));
			}
		}
		
		/// <summary>
		/// Sets indexes according to mesh pattern.
		/// NOTE: Make sure there's enough vertices for width and height of the mesh.
		/// NOTE: Use counter-clockwise culling.
		/// </summary>
		/// <param name="w">Width of the mesh.</param>
		/// <param name="h">Height of the mesh.</param>
		public static void PrimitiveSetMeshIndices(int w, int h)
		{
			// 0 - 1 - 2
			// | / | / |
			// 3 - 4 - 5
			// | / | / |
			// 6 - 7 - 8

			_primitiveType = PipelineMode.TrianglePrimitives;

			var offset = 0; // Basically, equals w * y.

			for(var y = 0; y < h - 1; y += 1)
			{
				for(var x = 0; x < w - 1; x += 1)
				{
					_primitiveIndices.Add((short)(x + offset));
					_primitiveIndices.Add((short)(x + 1 + offset));
					_primitiveIndices.Add((short)(x + w + offset));

					_primitiveIndices.Add((short)(x + w + offset));
					_primitiveIndices.Add((short)(x + 1 + offset));
					_primitiveIndices.Add((short)(x + w + 1 + offset));
				}
				offset += w;
			}
		}

		/// <summary>
		/// Sets indexes according to line strip pattern.
		/// </summary>
		/// <param name="loop">Tells is first and last vertix will have a line between them.</param>
		public static void PrimitiveSetLineStripIndices(bool loop)
		{
			// 0 - 1 - 2 - 3
			
			_primitiveType = PipelineMode.OutlinePrimitives;

			for(var i = 0; i < _primitiveVertices.Count - 1; i += 1)
			{
				_primitiveIndices.Add((short)i);
				_primitiveIndices.Add((short)(i + 1));
			}
			if (loop)
			{
				_primitiveIndices.Add((short)(_primitiveVertices.Count - 1));
				_primitiveIndices.Add(0);
			}
		}



		/// <summary>
		/// Sets user-defined array of indices for alist of lines.
		/// </summary>
		public static void PrimitiveSetCustomLineIndices(short[] indices)
		{ 
			_primitiveType = PipelineMode.OutlinePrimitives;
			_primitiveIndices = new List<short>(indices);
		}
		
		/// <summary>
		/// Sets user-defined list of indices for list of lines.
		/// </summary>
		public static void PrimitiveSetCustomLineIndices(List<short> indices)
		{
			_primitiveType = PipelineMode.OutlinePrimitives;
			_primitiveIndices = indices;
		}
		
		/// <summary>
		/// Sets user-defined array of indices for list of triangles.
		/// </summary>
		public static void PrimitiveSetCustomTriangleIndices(short[] indices)
		{
			_primitiveType = PipelineMode.TrianglePrimitives;
			_primitiveIndices = new List<short>(indices);
		}
		
		/// <summary>
		/// Sets user-defined list of indices for list of triangles.
		/// </summary>
		public static void PrimitiveSetCustomTriangleIndices(List<short> indices)
		{
			_primitiveType = PipelineMode.TrianglePrimitives;
			_primitiveIndices = indices;
		}


		/// <summary>
		/// Begins primitive drawing. 
		/// It's more of a safety check for junk primitives.
		/// </summary>
		public static void PrimitiveBegin()
		{
			if (_primitiveVertices.Count != 0 || _primitiveIndices.Count != 0)
			{
				throw new Exception("Junk primitive data detected! Did you set index data wrong or forgot PrimitiveEnd somewhere?");
			}
		}

		/// <summary>
		/// Ends drawing of a primitive.
		/// </summary>
		public static void PrimitiveEnd()
		{
			AddVertices(_primitiveType, _primitiveTexture, _primitiveVertices, _primitiveIndices.ToArray());

			_primitiveVertices.Clear();
			_primitiveIndices.Clear();
			_primitiveTexture = null;
		}

		#endregion Primitives.



		#region Text.

		/// <summary>
		/// Draws text in specified coordinates.
		/// </summary>
		public static void DrawText(string text, float x, float y) => 
			DrawText(text, new Vector2(x, y));

		/// <summary>
		/// Draws text in specified coordinates.
		/// </summary>
		public static void DrawText(string text, Vector2 pos)
		{
			/*
			 * Font is a wrapper for MG's SpriteFont, which uses non-premultiplied alpha.
			 * Using PipelineMode.Sprites will result in black pixels everywhere.
			 * TextureFont, on the other hand, is just a bunch of regular sprites, 
			 * so it's fine to draw with sprite mode.
			 */
			if (CurrentFont is Font)
			{
				SwitchPipelineMode(PipelineMode.SpritesNonPremultiplied);
			}
			else
			{
				SwitchPipelineMode(PipelineMode.Sprites);	
			}
			CurrentFont.Draw(Batch, text, pos, HorAlign, VerAlign);
		}

		/// <summary>
		/// Draws text in specified coordinates with rotation, scale and origin.
		/// </summary>
		public static void DrawText(string text, Vector2 pos, Vector2 scale, Vector2 origin, float rot = 0) => 
			DrawText(text, pos.X, pos.Y, scale.X, scale.Y, origin.X, origin.Y, rot);

		/// <summary>
		/// Draws text in specified coordinates with rotation, scale and origin.
		/// </summary>
		public static void DrawText(string text, float x, float y, float scaleX, float scaleY, float originX = 0, float originY = 0, float rot = 0)
		{
			Matrix transformMatrix = 
				Matrix.CreateTranslation(new Vector3(-originX, -originY, 0)) * // Origin.
				Matrix.CreateRotationZ(MathHelper.ToRadians(-rot)) *		       // Rotation.
				Matrix.CreateScale(new Vector3(scaleX, scaleY, 1)) *	         // Scale.
				Matrix.CreateTranslation(new Vector3(x, y, 0));                // Position.
												
			AddTransformMatrix(transformMatrix);
			
			/*
			 * Font is a wrapper for MG's SpriteFont, which uses non-premultiplied alpha.
			 * Using PipelineMode.Sprites will result in black pixels everywhere.
			 * TextureFont, on the other hand, is just regular sprites, so it's fine to 
			 * draw with sprite mode.
			 */
			if (CurrentFont is Font)
			{
				SwitchPipelineMode(PipelineMode.SpritesNonPremultiplied);
			}
			else
			{
				SwitchPipelineMode(PipelineMode.Sprites);	
			}

			CurrentFont.Draw(Batch, text, Vector2.Zero, HorAlign, VerAlign);
			
			ResetTransformMatrix();
		}

		#endregion Text.



		#region Surfaces.

		/// <summary>
		/// Sets surface as a render target.
		/// </summary>
		public static void SetSurfaceTarget(RenderTarget2D surf) => 
			SetSurfaceTarget(surf, Matrix.CreateTranslation(Vector3.Zero));

		/// <summary>
		/// Sets surface as a render target.
		/// </summary>
		/// <param name="surf">Target surface.</param>
		/// <param name="matrix">Surface transformation matrix.</param>
		public static void SetSurfaceTarget(RenderTarget2D surf, Matrix matrix)
		{
			SetTransformMatrix(matrix);

			_surfaceStack.Push(_currentSurface);
			_currentSurface = surf;

			CurrentProjection = Matrix.CreateOrthographicOffCenter(0,	_currentSurface.Width, _currentSurface.Height, 0, 0, 1);

			Device.SetRenderTarget(_currentSurface);
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
			}

			Device.SetRenderTarget(_currentSurface);
		}



		private static void DrawRenderTarget(
			RenderTarget2D renderTarget, 
			Vector2 pos, 
			Vector2 scale, 
			float rotation, 
			Vector2 offset, 
			Color color, 
			SpriteEffects effect
		)
		{
			SwitchPipelineMode(PipelineMode.Sprites);
			Batch.Draw(renderTarget, pos, renderTarget.Bounds, color, MathHelper.ToRadians(rotation), offset, scale, effect, 0);
		}
		
		// Vectors.

		public static void DrawSurface(RenderTarget2D surf, Vector2 pos)
		{
			SwitchPipelineMode(PipelineMode.Sprites);
			Batch.Draw(surf, pos, CurrentColor);
		}
		
		public static void DrawSurface(RenderTarget2D surf, Vector2 pos, Vector2 scale, float rotation, Color color)
		{
			SwitchPipelineMode(PipelineMode.Sprites);
			
			var mirroring = SpriteEffects.None;

			// Proper negative scaling.
			var offset = Vector2.Zero;

			if (scale.X < 0)
			{
				mirroring = mirroring | SpriteEffects.FlipHorizontally;
				scale.X *= -1;
				offset.X = surf.Width;
			}

			if (scale.Y < 0)
			{
				mirroring = mirroring | SpriteEffects.FlipVertically;
				scale.Y *= -1;
				offset.Y = surf.Height;
			}
			// Proper negative scaling.

			DrawRenderTarget(surf, pos, scale, rotation, offset, color, mirroring);
		}
		
		public static void DrawSurface(RenderTarget2D surf, Vector2 pos, Vector2 scale, float rotation, Vector2 offset, Color color)
		{
			SwitchPipelineMode(PipelineMode.Sprites);
			
			var mirroring = SpriteEffects.None;

			// Proper negative scaling.
			var scaleOffset = Vector2.Zero;

			if (scale.X < 0)
			{
				mirroring = mirroring | SpriteEffects.FlipHorizontally;
				scale.X *= -1;
				scaleOffset.X = surf.Width;
			}

			if (scale.Y < 0)
			{
				mirroring = mirroring | SpriteEffects.FlipVertically;
				scale.Y *= -1;
				scaleOffset.Y = surf.Height;
			}
			// Proper negative scaling.

			DrawRenderTarget(surf, pos, scale, rotation, scaleOffset + offset, color, mirroring);
		}

		// Vectors.
		
		// Floats.

		public static void DrawSurface(RenderTarget2D surf, float x, float y) =>
			DrawSurface(surf, new Vector2(x, y));
		
		public static void DrawSurface(RenderTarget2D surf, float x, float y, float scaleX, float scaleY, float rotation, Color color) =>
			DrawSurface(surf, new Vector2(x, y), new Vector2(scaleX, scaleY), rotation, color);

		// Floats.

		// Rectangles.

		public static void DrawSurface(RenderTarget2D surf, Rectangle destRect)
		{
			SwitchPipelineMode(PipelineMode.Sprites);
			Batch.Draw(surf, destRect, surf.Bounds, CurrentColor);
		}
		
		public static void DrawSurface(RenderTarget2D surf, Rectangle destRect, float rotation, Color color)
		{
			SwitchPipelineMode(PipelineMode.Sprites);
			Batch.Draw(
				surf, 
				destRect, 
				surf.Bounds, 
				color, 
				rotation,
				Vector2.Zero,
				SpriteEffects.None, 
				0
			);
		}

		public static void DrawSurface(RenderTarget2D surf, Rectangle destRect, Rectangle srcRect)
		{
			SwitchPipelineMode(PipelineMode.Sprites);
			
			srcRect.X += surf.Bounds.X;
			srcRect.Y += surf.Bounds.Y;

			Batch.Draw(surf, destRect, srcRect, CurrentColor);
		}
		
		public static void DrawSurface(RenderTarget2D surf, Rectangle destRect, Rectangle srcRect, float rotation, Color color)
		{
			SwitchPipelineMode(PipelineMode.Sprites);
						
			srcRect.X += surf.Bounds.X;
			srcRect.Y += surf.Bounds.Y;

			Batch.Draw(
				surf, 
				destRect, 
				surf.Bounds, 
				color, 
				rotation, 
				Vector2.Zero,
				SpriteEffects.None, 
				0
			);
		}

		// Rectangles.


		#endregion Surfaces.
	}
}