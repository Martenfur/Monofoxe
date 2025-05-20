using Microsoft.Xna.Framework;
using Monofoxe.Engine;
using Monofoxe.Engine.Drawing;
using Monofoxe.Engine.EC;
using Monofoxe.Engine.Resources;
using Monofoxe.Engine.SceneSystem;
using System;


namespace Monofoxe.Samples.Demos
{
	public class PrimitiveDemo : Entity
	{
		public static readonly string Description = ToggleWireframeButton + " - toggle wireframe.";

		Color _mainColor = Color.White;
		Color _secondaryColor = Color.Violet;
		Color _secondaryColor2 = Color.CadetBlue;


		TriangleFanPrimitive _trianglefan;
		TriangleStripPrimitive _trianglestrip;
		
		MeshPrimitive _mesh;
		float _meshRepeat = 2;

		LineStripPrimitive _linestrip;

		CustomTrianglePrimitive _custom;


		bool _useWireframe = false;
		public const Buttons ToggleWireframeButton = Buttons.W;

		Sprite _autismCatSprite;

		public PrimitiveDemo(Layer layer) : base(layer)
		{	
			// Primitives can only be drawn from instances. There are no static methods.
			_trianglefan = new TriangleFanPrimitive(5);

			_trianglefan.Vertices[0] = new Vertex(new Vector2(0, 0));
			_trianglefan.Vertices[1] = new Vertex(new Vector2(16, 0));
			_trianglefan.Vertices[2] = new Vertex(new Vector2(40, 10));
			_trianglefan.Vertices[3] = new Vertex(new Vector2(64, 64));
			_trianglefan.Vertices[4] = new Vertex(new Vector2(0, 32));
			


			_trianglestrip = new TriangleStripPrimitive(8);
			_trianglestrip.Vertices[0] = new Vertex(new Vector2(0, 0), _mainColor);
			_trianglestrip.Vertices[1] = new Vertex(new Vector2(32, 32), _secondaryColor);
			_trianglestrip.Vertices[2] = new Vertex(new Vector2(64, 0),  _secondaryColor2);
			_trianglestrip.Vertices[3] = new Vertex(new Vector2(96, 32),  _secondaryColor);
			_trianglestrip.Vertices[4] = new Vertex(new Vector2(64+32, 0),  _secondaryColor2);
			_trianglestrip.Vertices[5] = new Vertex(new Vector2(96+32, 32), _secondaryColor);
			_trianglestrip.Vertices[6] = new Vertex(new Vector2(64+64, 0),  _secondaryColor2);
			_trianglestrip.Vertices[7] = new Vertex(new Vector2(96+64, 32), _mainColor);
		

			_mesh = new MeshPrimitive(16, 16);
			_mesh.Position = new Vector2(0, 100);

			// You can set the texture for a primitive. Preferrably it shouldn't be in texture atlas.
			// If in atlas, textures wouldn't be able to repeat.
			_autismCatSprite = ResourceHub.GetResource<Sprite>("DefaultSprites", "AutismCat");
			_mesh.SetTextureFromFrame(_autismCatSprite[0]);
			
			var vIndex = 0;
			for(var k = 0; k < _mesh.Height; k += 1)
			{
				for(var i = 0; i < _mesh.Width; i += 1)
				{
					_mesh.Vertices[vIndex] = 
						new Vertex(
							Vector2.Zero, // Positions will be set later.
							Color.White, 
							new Vector2(i / (float)(_mesh.Width - 1), k / (float)(_mesh.Height - 1)) * _meshRepeat
						);
					
					vIndex += 1;
				}
			}


			_linestrip = new LineStripPrimitive(16);
			
			for(var i = 0; i < 16; i += 1)
			{
				_linestrip.Vertices[i] = new Vertex(Vector2.Zero, new Color(16 * i, 8 * i, 4 * i));
			}


			// You can make your own custom primitives. 
			// CustomTrianglePrimitive and CustomLinePrimitive give you
			// access to the index array. Index array tells inwhat order vertices should be drawn.
			// One vertex can be used multiple times in an index array.
			_custom = new CustomTrianglePrimitive(4);

			_custom.Vertices[0] = new Vertex(new Vector2(0, 0), Color.Green);
			_custom.Vertices[1] = new Vertex(new Vector2(32, 0));
			_custom.Vertices[2] = new Vertex(new Vector2(32, 32));
			_custom.Vertices[3] = new Vertex(new Vector2(64, 32), Color.Blue);
			
			_custom.Indices = new short[]{0, 1, 2, 1, 3, 2};
			
		}

		public override void Update()
		{
			base.Update();

			if (Input.CheckButtonPress(ToggleWireframeButton))
			{
				_useWireframe = !_useWireframe;
			}
		}

		public override void Draw()
		{
			base.Draw();

			if (_useWireframe)
			{
				GraphicsMgr.VertexBatch.RasterizerState = GameController.WireframeRasterizer;
			}

			var startingPosition = new Vector2(100, 100);
			var position = startingPosition;
			var spacing = 100;

			// Trianglefan.
			_trianglefan.Position = position;
			_trianglefan.Draw();
			// Trianglefan.


			position += Vector2.UnitX * spacing;


			// Trianglestrip.
			_trianglestrip.Position = position;
			_trianglestrip.Draw();
			// Trianglestrip.


			position += Vector2.UnitX * spacing * 2;


			// Mesh.
			_mesh.Position = position;

			var cell = new Vector2(
				_autismCatSprite.Size.X / _mesh.Width,
				_autismCatSprite.Size.Y / _mesh.Height
			) * _meshRepeat;
			var c = 0;
			for(var k = 0; k < _mesh.Height; k += 1)
			{
				for(var i = 0; i < _mesh.Width; i += 1)
				{
					var v = _mesh.Vertices[c];
					// Displacing vertices to make ripple effect.
					v.Position = new Vector3(i, k, 0) * cell.ToVector3() + new Vector3(1, 1, 0) * (float)Math.Sin(GameMgr.ElapsedTimeTotal + i) * 4;
					_mesh.Vertices[c] = v;
					c += 1;
				}
			}

			_mesh.Draw();
			// Mesh.


			position += Vector2.UnitY * spacing * 2;
			position.X = startingPosition.X;


			// Linestrip.
			var vertex = _linestrip.Vertices[0];

			// Tail effect.
			vertex.Position = position.ToVector3() + new Vector3(
				(float)Math.Cos(GameMgr.ElapsedTimeTotal * 2), 
				(float)Math.Sin(-GameMgr.ElapsedTimeTotal * 4),
				0
			) * 50;
			_linestrip.Vertices[0] = vertex;

			for(var i = 1; i < _linestrip.Vertices.Length; i += 1)
			{
				vertex = _linestrip.Vertices[i];
				var delta = _linestrip.Vertices[i - 1].Position - vertex.Position;
				if (delta.Length() > 8)
				{
					var e = delta.SafeNormalize();
					vertex.Position = _linestrip.Vertices[i - 1].Position - e * 8;
					_linestrip.Vertices[i] = vertex;
				}
			}

			_linestrip.Draw();
			// Linestrip.


			position += Vector2.UnitX * spacing;


			// Custom.
			_custom.Position = position;
			_custom.Draw();
			// Custom.


			if (_useWireframe)
			{
				GraphicsMgr.VertexBatch.RasterizerState = GameController.DefaultRasterizer;
			}

		}
		

	}
}
