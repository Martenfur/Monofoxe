using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Diagnostics;


namespace Monofoxe.Engine
{
	class DrawCntrl
	{
		
		public static SpriteBatch Batch;
		public static GraphicsDevice Device;

		/// <summary>
		/// List of all cameras.
		/// </summary>
		public static List<Camera> Cameras;

		/// <summary>
		/// Current enabled camera.
		/// </summary>
		public static Camera CurrentCamera {get; private set;} = null;
		public static BasicEffect BasicEffect;

		/// <summary>
		/// Current drawing color. Affects shapes and primitives.
		/// </summary>
		public static Color CurrentColor;

		private static DynamicVertexBuffer _vertexBuffer;
		private static DynamicIndexBuffer _indexBuffer;

		private static List<VertexPositionColorTexture> _vertices;
		private static List<short> _indices;

		/// <summary>
		/// Amount of draw calls per frame.
		/// </summary>
		public static int __drawcalls {get; private set;}

		/// <summary>
		/// Every time we want to draw primitive of a new type
		/// or switch texture, we need to empty vertex buffer
		/// and switch pipeline mode.
		/// </summary>
		private enum PipelineMode
		{
			Sprites,            // Sprite batch.
			TrianglePrimitives, // Triangle list.
			OutlinePrimitives,  // Line list.
		}

		private static PipelineMode _currentPipelineMode;



		#region shapes

		private static readonly PipelineMode[] _types = {PipelineMode.OutlinePrimitives, PipelineMode.TrianglePrimitives};
		
		// Triangle.
		private static readonly int[] _trianglePrAmounts = {3, 1};
		private static readonly short[][] _triangleIndices = 
		{
			new short[]{0, 1, 1, 2, 2, 0}, 
			new short[]{0, 1, 2}
		};
		// Triangle.

		// Rectangle.
		private static readonly int[] _rectanglePrAmounts = {4, 2};
		private static readonly short[][] _rectangleIndices = 
		{
			new short[]{0, 1, 1, 2, 2, 3, 3, 0},
			new short[]{0, 1, 3, 1, 2, 3}
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
			
				var angAdd = (float)Math.PI * 2 / value;
				
				for(var i = 0; i < value; i += 1)
				{_circleVectors.Add(new Vector2((float)Math.Cos(angAdd * i), (float)Math.Sin(angAdd * i)));}
			}

			get
			{return _circleVectors.Count;}
		}

		private static List<Vector2> _circleVectors; 
		// Circle.

		// Primitive.
		private static List<VertexPositionColorTexture> _primitiveVertices;
		private static List<short> _primitiveIndices;
		private static PipelineMode _primitiveType;
		// Primitive.

		#endregion shapes



		/// <summary>
		/// Initialization function for draw controller. 
		/// Should be called only once, by Game class.
		/// </summary>
		/// <param name="device">Graphics device.</param>
		public static void Init(GraphicsDevice device)
		{
			Device = device;

			Batch = new SpriteBatch(device);
			Cameras = new List<Camera>();
			BasicEffect = new BasicEffect(Device);
			BasicEffect.VertexColorEnabled = true;
			Device.DepthStencilState = DepthStencilState.DepthRead;
			
			_vertexBuffer = new DynamicVertexBuffer(Device, typeof(VertexPositionColorTexture), 320000, BufferUsage.WriteOnly);
			_indexBuffer = new DynamicIndexBuffer(Device, IndexElementSize.SixteenBits, 320000, BufferUsage.WriteOnly);
			_vertices = new List<VertexPositionColorTexture>();
			_indices = new List<short>();

			_currentPipelineMode = PipelineMode.Sprites;

			Device.SetVertexBuffer(_vertexBuffer);
			Device.Indices = _indexBuffer;

			_primitiveVertices = new List<VertexPositionColorTexture>();
			_primitiveIndices = new List<short>();

			CurrentColor = Color.White;
			CircleVerticesCount = 32;
		}



		/// <summary>
		/// Performs Draw events for all objects.
		/// </summary>
		public static void Update()
		{
			__drawcalls = 0;
			_currentPipelineMode = PipelineMode.Sprites;
			
			var depthSortedObjects = Objects.GameObjects.OrderByDescending(o => o.Depth);
			
			// Main draw events.
			foreach(Camera camera in Cameras)
			{
				CurrentCamera = camera;
				//BasicEffect.World = camera.CreateTranslationMatrix();
				BasicEffect.View = camera.CreateTranslationMatrix();
				BasicEffect.Projection = Matrix.CreateOrthographicOffCenter(0, camera.W, camera.H, 0, 0, 1);


				if (camera.Enabled)
				{
					Device.SetRenderTarget(camera.ViewSurface);
					Device.Clear(camera.BackgroundColor);
					Batch.Begin(SpriteSortMode.BackToFront, BlendState.AlphaBlend, null, null, null, null, camera.CreateTranslationMatrix());
					
					foreach(GameObj obj in depthSortedObjects)
					{
						if (obj.Active)
						{obj.DrawBegin();}
					}

					foreach(GameObj obj in depthSortedObjects)
					{
						if (obj.Active)
						{obj.Draw();}
					}
			
					foreach(GameObj obj in depthSortedObjects)
					{
						if (obj.Active)
						{obj.DrawEnd();}
					}
					Batch.End();
				}
			}
			// Main draw events.

			CurrentCamera = null;
			BasicEffect.World = Matrix.CreateTranslation(0, 0, 0);
			BasicEffect.Projection = Matrix.CreateOrthographicOffCenter(0, 800, 480, 0, 0, 1);


			Device.SetRenderTarget(null);
			
			
			// Drawing camera surfaces.
			Device.Clear(Color.TransparentBlack);
			
			Batch.Begin();
			foreach(Camera camera in Cameras)
			{
				if (camera.Autodraw && camera.Enabled)
				{Batch.Draw(camera.ViewSurface, new Vector2(camera.PortX, camera.PortY), Color.White);}
			}
			Batch.End();
			DrawVertices();
			// Drawing camera surfaces.

			Debug.WriteLine("CALLS: " + __drawcalls);

			// Drawing GUI stuff.
			Batch.Begin();
			foreach(GameObj obj in depthSortedObjects)
			{
				if (obj.Active)
				{obj.DrawGUI();}
			}
			Batch.End();
			// Drawing GUI stuff.
		}



		#region pipeline

		/// <summary>
		/// Switches graphics pipeline mode.
		/// </summary>
		/// <param name="mode"></param>
		private static void SwitchPipelineMode(PipelineMode mode)
		{
			if (mode != _currentPipelineMode)
			{
				if (_currentPipelineMode == PipelineMode.Sprites)
				{Batch.End();}
				else
				{DrawVertices();}

				switch(mode)
				{
					case PipelineMode.Sprites:
					{
						Batch.Begin(SpriteSortMode.BackToFront, BlendState.AlphaBlend, null, null, null, null, Cameras[0].CreateTranslationMatrix());
						break;
					}
					case PipelineMode.TrianglePrimitives:
					{
						break;
					}
					case PipelineMode.OutlinePrimitives:
					{ 
						break;
					}
				}

				_currentPipelineMode = mode;
			}
		}



		/// <summary>
		/// Adds vertices and indices to global vertex and index list.
		/// If current and suggested pipeline modes are different, draws accumulated vertices first.
		/// </summary>
		/// <param name="mode">Suggested pipeline mode.</param>
		/// <param name="vertices">List of vertices.</param>
		/// <param name="indexes">List of indices.</param>
		private static void AddVertices(PipelineMode mode, List<VertexPositionColorTexture> vertices, List<short> indexes)
		{
			SwitchPipelineMode(mode);

			for(var i = 0; i < indexes.Count; i += 1)
			{indexes[i] += (short)_vertices.Count;} // We need to offset each index because of single buffer for everything.

			_vertices.AddRange(vertices);
			_indices.AddRange(indexes);
		}



		/// <summary>
		/// Draws vertices from vertex buffer and empties it.
		/// </summary>
		private static void DrawVertices()
		{
			__drawcalls += 1;

			if (_vertices.Count > 0)
			{
				PrimitiveType type;
				int prCount;

				if (_currentPipelineMode == PipelineMode.OutlinePrimitives)
				{
					type = PrimitiveType.LineList;
					prCount = _vertices.Count;
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
			
				RasterizerState rasterizerState = new RasterizerState(); // Do something with it, I guees.
			  rasterizerState.CullMode = CullMode.None;
				//rasterizerState.FillMode = FillMode.WireFrame;
				Device.RasterizerState = rasterizerState;

				foreach(EffectPass pass in BasicEffect.CurrentTechnique.Passes)
				{
					pass.Apply();
					Device.DrawIndexedPrimitives(type, 0, 0, prCount);
				}

				_vertices.Clear();
				_indices.Clear();
				
			}
		}

		#endregion pipeline



		#region sprites
		
		public static void DrawSprite(Texture2D texture, int x, int y, Color color)
		{
			Batch.Draw(texture, new Vector2(x, y), color);
		}	
		
		

		public static void DrawSurface(RenderTarget2D surf, int x, int y, Color color)
		{
			Batch.Draw(surf, new Vector2(x, y), color);
		}	

		#endregion sprites



		#region shapes

		/// <summary>
		/// Draws a triangle.
		/// </summary>
		/// <param name="p1">First point.</param>
		/// <param name="p2">Second point.</param>
		/// <param name="p3">Third point.</param>
		/// <param name="isOutline">Is shape outline.</param>
		public static void DrawTriangle(Vector2 p1, Vector2 p2, Vector2 p3, bool isOutline)
		{
			DrawTriangle((int)p1.X, (int)p1.Y, (int)p2.X, (int)p2.Y, (int)p3.X, (int)p3.Y, isOutline, CurrentColor, CurrentColor, CurrentColor);
		}

		/// <summary>
		/// Draws a triangle with specified colors.
		/// </summary>
		/// <param name="p1">First point.</param>
		/// <param name="p2">Second point.</param>
		/// <param name="p3">Third point.</param>
		/// <param name="isOutline">Is shape outline.</param>
		/// <param name="c1">1st color.</param>
		/// <param name="c2">2nd color.</param>
		/// <param name="c3">3rd color.</param>
		public static void DrawTriangle(Vector2 p1, Vector2 p2, Vector2 p3, bool isOutline, Color c1, Color c2, Color c3)
		{
			DrawTriangle((int)p1.X, (int)p1.Y, (int)p2.X, (int)p2.Y, (int)p3.X, (int)p3.Y, isOutline, c1, c2, c3);
		}
		
		/// <summary>
		/// Draws a triangle.
		/// </summary>
		/// <param name="isOutline"></param>
		public static void DrawTriangle(int x1, int y1, int x2, int y2, int x3, int y3, bool isOutline)
		{
			DrawTriangle(x1, y1, x2, y2, x3, y3, isOutline, CurrentColor, CurrentColor, CurrentColor);
		}

		/// <summary>
		/// Draw a triangle with specified colors.
		/// </summary>
		/// <param name="isOutline">Is shape outline.</param>
		/// <param name="c1">1st color.</param>
		/// <param name="c2">2nd color.</param>
		/// <param name="c3">3rd color.</param>
		public static void DrawTriangle(int x1, int y1, int x2, int y2, int x3, int y3, bool isOutline, Color c1, Color c2, Color c3)
		{
			int o;
			if (isOutline)
			{o = 0;}
			else
			{o = 1;}

			var vertices = new List<VertexPositionColorTexture>();

			vertices.Add(new VertexPositionColorTexture(new Vector3(x1, y1, 0), c1, Vector2.Zero));
			vertices.Add(new VertexPositionColorTexture(new Vector3(x2, y2, 0), c2, Vector2.Zero));
			vertices.Add(new VertexPositionColorTexture(new Vector3(x3, y3, 0), c3, Vector2.Zero));
			
			AddVertices(_types[o], vertices, new List<short>(_triangleIndices[o]));
		}



		/// <summary>
		/// Draws a rectangle.
		/// </summary>
		/// <param name="p1">First point.</param>
		/// <param name="p2">Second point.</param>
		/// <param name="isOutline">Is shape outline.</param>
		public static void DrawRectangle(Vector2 p1, Vector2 p2, bool isOutline)
		{
			DrawRectangle((int)p1.X, (int)p1.Y, (int)p2.X, (int)p2.Y, isOutline, CurrentColor, CurrentColor, CurrentColor, CurrentColor);
		}

		/// <summary>
		/// Draws a rectangle with specified colors for each corner.
		/// </summary>
		/// <param name="p1">First point.</param>
		/// <param name="p2">Second point.</param>
		/// <param name="isOutline">Is shape outline.</param>
		/// <param name="c1">1st color.</param>
		/// <param name="c2">2nd color.</param>
		/// <param name="c3">3rd color.</param>
		/// <param name="c4">4th color.</param>
		public static void DrawRectangle(Vector2 p1, Vector2 p2, bool isOutline, Color c1, Color c2, Color c3, Color c4)
		{
			DrawRectangle((int)p1.X, (int)p1.Y, (int)p2.X, (int)p2.Y, isOutline, c1, c2, c3, c4);
		}

		/// <summary>
		/// Draws a rectangle.
		/// </summary>
		/// <param name="isOutline">Is shape outline</param>
		public static void DrawRectangle(int x1, int y1, int x2, int y2, bool isOutline)
		{
			DrawRectangle(x1, y1, x2, y2, isOutline, CurrentColor, CurrentColor, CurrentColor, CurrentColor);
		}

		/// <summary>
		/// Draws a rectangle with specified colors for each corner.
		/// </summary>
		/// <param name="isOutline">Is shape outline.</param>
		/// <param name="c1">1st color.</param>
		/// <param name="c2">2nd color.</param>
		/// <param name="c3">3rd color.</param>
		/// <param name="c4">4th color.</param>
		public static void DrawRectangle(int x1, int y1, int x2, int y2, bool isOutline, Color c1, Color c2, Color c3, Color c4)
		{
			int o;
			if (isOutline)
			{o = 0;}
			else
			{o = 1;}

			var vertices = new List<VertexPositionColorTexture>();
			
			vertices.Add(new VertexPositionColorTexture(new Vector3(x1, y1, 0), c1, Vector2.Zero));
			vertices.Add(new VertexPositionColorTexture(new Vector3(x2, y1, 0), c2, Vector2.Zero));
			vertices.Add(new VertexPositionColorTexture(new Vector3(x2, y2, 0), c3, Vector2.Zero));
			vertices.Add(new VertexPositionColorTexture(new Vector3(x1, y2, 0), c4, Vector2.Zero));
			
			AddVertices(_types[o], vertices, new List<short>(_rectangleIndices[o]));
		}


		
		/// <summary>
		/// Draws a circle.
		/// </summary>
		/// <param name="r">Radius.</param>
		/// <param name="isOutline">Is shape outline.</param>
		public static void DrawCircle(Vector2 p, int r, bool isOutline)
		{
			DrawCircle((int)p.X, (int)p.Y, r, isOutline);
		}

		/// <summary>
		/// Draws a circle.
		/// </summary>
		/// <param name="p">Center point.</param>
		/// <param name="r">Radius.</param>
		/// <param name="isOutline">Is shape outline.</param>
		public static void DrawCircle(int x, int y, int r, bool isOutline)
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
			{vertices.Add(new VertexPositionColorTexture(new Vector3(x + r * _circleVectors[i].X, y + r * _circleVectors[i].Y, 0), CurrentColor, Vector2.Zero));}
			AddVertices(prType, vertices, new List<short>(indexArray));
		}

		#endregion shapes



		#region primitives
		
		public static void PrimitiveAddVertex(Vector2 pos)
		{
			_primitiveVertices.Add(new VertexPositionColorTexture(new Vector3(pos.X, pos.Y, 0), CurrentColor, Vector2.Zero));
		}

		public static void PrimitiveAddVertex(Vector2 pos, Color color)
		{
			_primitiveVertices.Add(new VertexPositionColorTexture(new Vector3(pos.X, pos.Y, 0), color, Vector2.Zero));
		}

		public static void PrimitiveAddVertex(Vector2 pos, Color color, Vector2 texturePos)
		{
			_primitiveVertices.Add(new VertexPositionColorTexture(new Vector3(pos.X, pos.Y, 0), color, texturePos));
		}

		public static void PrimitiveAddVertex(int x, int y)
		{
			_primitiveVertices.Add(new VertexPositionColorTexture(new Vector3(x, y, 0), CurrentColor, Vector2.Zero));
		}

		public static void PrimitiveAddVertex(int x, int y, Color color)
		{
			_primitiveVertices.Add(new VertexPositionColorTexture(new Vector3(x, y, 0), color, Vector2.Zero));
		}

		public static void PrimitiveAddVertex(int x, int y, Color color, Vector2 texturePos)
		{
			_primitiveVertices.Add(new VertexPositionColorTexture(new Vector3(x, y, 0), color, texturePos));
		}


		/// <summary>
		/// Sets indexes according to trianglestrip pattern.
		/// </summary>
		public static void PrimitiveSetTriangleStripIndices()
		{
			// 0 - 2 - 4
			//  \ / \ /
			//   1 - 3
			
			_primitiveType = PipelineMode.TrianglePrimitives;

			for(var i = 0; i < _primitiveVertices.Count - 2; i += 1)
			{
				_primitiveIndices.Add((short)i);
				_primitiveIndices.Add((short)(i + 1));
				_primitiveIndices.Add((short)(i + 2));
			}
		}
		
		/// <summary>
		/// Sets indexes according to trianglefan pattern.
		/// </summary>
		public static void PrimitiveSetTriangleFanIndices()
		{
			//   1
			//  / \
			// 0 - 2 
			//  \ / 
			//   3 

			_primitiveType = PipelineMode.TrianglePrimitives;

			for(var i = 0; i < _primitiveVertices.Count - 1; i += 1)
			{
				_primitiveIndices.Add(0);
				_primitiveIndices.Add((short)i);
				_primitiveIndices.Add((short)(i + 1));
			}
		}
		
		/// <summary>
		/// Sets indexes according to mesh pattern.
		/// IMPORTANT: Make sure there's enough vertices for width and height of the mesh.
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
					_primitiveIndices.Add((short)(x + w + 1 + offset));
					_primitiveIndices.Add((short)(x + 1 + offset));
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
		/// <param name="indices">Array of indices.</param>
		public static void PrimitiveSetCustomLineIndices(short[] indices)
		{
			_primitiveType = PipelineMode.OutlinePrimitives;
			_primitiveIndices = indices.ToList();
		}
		
		/// <summary>
		/// Sets user-defined list of indices for list of lines.
		/// </summary>
		/// <param name="indices">List of indices.</param>
		public static void PrimitiveSetCustomLineIndices(List<short> indices)
		{
			_primitiveType = PipelineMode.OutlinePrimitives;
			_primitiveIndices = indices;
		}
		
		/// <summary>
		/// Sets user-defined array of indices for list of triangles.
		/// </summary>
		/// <param name="indices">Array of indices.</param>
		public static void PrimitiveSetCustomTriangleIndices(short[] indices)
		{
			_primitiveType = PipelineMode.TrianglePrimitives;
			_primitiveIndices = indices.ToList();
		}
		
		/// <summary>
		/// Sets user-defined list of indices for list of triangles.
		/// </summary>
		/// <param name="indices">List of indices.</param>
		public static void PrimitiveSetCustomTriangleIndices(List<short> indices)
		{
			_primitiveType = PipelineMode.TrianglePrimitives;
			_primitiveIndices = indices;
		}



		/// <summary>
		/// Ends drawing of a primitive.
		/// </summary>
		public static void PrimitiveEnd()
		{
			AddVertices(_primitiveType, _primitiveVertices, _primitiveIndices);

			_primitiveVertices.Clear();
			_primitiveIndices.Clear();
		}

		#endregion primitives

	}
}
