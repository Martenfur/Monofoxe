using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Media;
using System.Diagnostics;


namespace Monofoxe.Engine
{
	class DrawCntrl
	{
		
		public static SpriteBatch Batch;
		public static GraphicsDevice Device;

		public static List<Camera> Cameras;
		public static Camera CurrentCamera {get; private set;} = null;
		public static BasicEffect BasicEffect;

		public static Color CurrentColor;

		private static DynamicVertexBuffer _vertexBuffer;
		private static DynamicIndexBuffer _indexBuffer;

		private static List<VertexPositionColor> _vertices;
		private static List<short> _indexes;

		static int __drawcalls;

		public enum PipelineModes
		{
			Sprites,
			TrianglePrimitives,
			OutlinePrimitives,
			TexturedPrimitives,
		}

		private static PipelineModes _currentPipelineMode;


		public static void Init(GraphicsDevice device, SpriteBatch batch)
		{
			Batch = batch;
			Device = device;
			Cameras = new List<Camera>();
			BasicEffect = new BasicEffect(Device);
			BasicEffect.VertexColorEnabled = true;
			Device.DepthStencilState = DepthStencilState.DepthRead;
			
			CurrentColor = Color.White;

			_vertexBuffer = new DynamicVertexBuffer(Device, typeof(VertexPositionColor), 320000, BufferUsage.WriteOnly);
			_indexBuffer = new DynamicIndexBuffer(Device, IndexElementSize.SixteenBits, 320000, BufferUsage.WriteOnly);
			_vertices = new List<VertexPositionColor>();
			_indexes = new List<short>();

			_currentPipelineMode = PipelineModes.Sprites;

			Device.SetVertexBuffer(_vertexBuffer);
			Device.Indices = _indexBuffer;


		}


		public static void Update()
		{__drawcalls = 0;
			_currentPipelineMode = PipelineModes.Sprites;
			
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
			DrawPrimitives();
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



		public static void DrawSprite(Texture2D texture, int x, int y, Color color)
		{
			Batch.Draw(texture, new Vector2(x, y), color);
		}	
		
		

		public static void DrawSurface(RenderTarget2D surf, int x, int y, Color color)
		{
			Batch.Draw(surf, new Vector2(x, y), color);
		}	



		public static void SwitchPipelineMode(PipelineModes mode)
		{
			if (mode != _currentPipelineMode)
			{
				if (_currentPipelineMode == PipelineModes.Sprites)
				{Batch.End();}
				else
				{DrawPrimitives();}

				switch(mode)
				{
					case PipelineModes.Sprites:
					{
						Batch.Begin(SpriteSortMode.BackToFront, BlendState.AlphaBlend, null, null, null, null, Cameras[0].CreateTranslationMatrix());
						break;
					}
					case PipelineModes.TrianglePrimitives:
					{
						break;
					}
					case PipelineModes.OutlinePrimitives:
					{ 
						break;
					}
				}

				_currentPipelineMode = mode;
			}
		}



		private static void DrawPrimitives()
		{__drawcalls += 1;
			if (_vertices.Count > 0)
			{
				PrimitiveType type;
				int prCount;

				if (_currentPipelineMode == PipelineModes.OutlinePrimitives)
				{
					type = PrimitiveType.LineList;
					prCount = _vertexBuffer.VertexCount;
				}
				else
				{
					type = PrimitiveType.TriangleList;
					prCount = _indexBuffer.IndexCount / 3;
				}

				_vertexBuffer.SetData(_vertices.ToArray(), 0, _vertices.Count, SetDataOptions.None);
				_indexBuffer.SetData(_indexes.ToArray(), 0, _indexes.Count);
				
				RasterizerState rasterizerState = new RasterizerState();
			  rasterizerState.CullMode = CullMode.None;
				Device.RasterizerState = rasterizerState;

				foreach(EffectPass pass in BasicEffect.CurrentTechnique.Passes)
				{
					pass.Apply();
					Device.DrawIndexedPrimitives(type, 0, 0, prCount);
				}

				_vertices.Clear();
				_indexes.Clear();
			}
		}



		public static void AddPrimitive(PipelineModes mode, List<VertexPositionColor> vertices, List<short> indexes)
		{
			SwitchPipelineMode(mode);

			for(var i = 0; i < indexes.Count; i += 1)
			{indexes[i] += (short)_vertices.Count;} // We need to offset each index because of single buffer for everything.

			_vertices.AddRange(vertices);
			_indexes.AddRange(indexes);
		}

		
	}
}
