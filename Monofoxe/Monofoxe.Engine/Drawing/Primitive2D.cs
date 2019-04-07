using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Monofoxe.Engine.Utils;

namespace Monofoxe.Engine.Drawing
{
	public class Primitive2D : IDrawable
	{
		public Vector2 Position {get; set;}

		/// <summary>
		/// First triangle point. 
		/// NOTE: all line points treat position as an origin point;
		/// </summary>
		public Vertex Points;
		
		public void Draw() {}
	

		private static List<VertexPositionColorTexture> _primitiveVertices = new List<VertexPositionColorTexture>();
		private static List<short> _primitiveIndices = new List<short>();
		private static GraphicsMode _primitiveType = GraphicsMode.None;
		private static Texture2D _primitiveTexture;

		private static Vector2 _primitiveTextureOffset;
		private static Vector2 _primitiveTextureRatio;


		/// <summary>
		/// Sets texture for a primitive.
		/// </summary>
		public static void SetTexture(Texture2D texture)
		{
			_primitiveTexture = texture;
			_primitiveTextureOffset = Vector2.Zero;
			_primitiveTextureRatio = Vector2.One;
		}

		/// <summary>
		/// Sets texture for a primitive.
		/// </summary>
		public static void SetTexture(Frame frame)
		{
			DrawMgr.SwitchGraphicsMode(GraphicsMode.None);
			
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


		public static void AddVertex(Vertex vertex)
		{
			// Since we may work with sprites, which are only little parts of whole texture atlas,
			// we need to convert local sprite coordinates to global atlas coordinates.
			Vector2 atlasPos = _primitiveTextureOffset + vertex.TexturePosition * _primitiveTextureRatio;
			_primitiveVertices.Add(new VertexPositionColorTexture(vertex.Position.ToVector3(), vertex.Color, atlasPos));
		}


		/// <summary>
		/// Sets indices according to trianglestrip pattern.
		/// NOTE: Use counter-clockwise culling.
		/// </summary>
		public static void SetTriangleStripIndices()
		{
			// 0 - 2 - 4
			//  \ / \ /
			//   1 - 3
			
			_primitiveType = GraphicsMode.TrianglePrimitives;

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
		public static void SetTriangleFanIndices()
		{
			//   1
			//  / \
			// 0 - 2 
			//  \ / 
			//   3 

			_primitiveType = GraphicsMode.TrianglePrimitives;

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
		public static void SetMeshIndices(int w, int h)
		{
			// 0 - 1 - 2
			// | / | / |
			// 3 - 4 - 5
			// | / | / |
			// 6 - 7 - 8

			_primitiveType = GraphicsMode.TrianglePrimitives;

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
		public static void SetLineStripIndices(bool loop)
		{
			// 0 - 1 - 2 - 3
			
			_primitiveType = GraphicsMode.OutlinePrimitives;

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
		/// Sets user-defined list of indices for list of lines.
		/// </summary>
		public static void SetCustomLineIndices(List<short> indices)
		{
			_primitiveType = GraphicsMode.OutlinePrimitives;
			_primitiveIndices = indices;
		}
		
		/// <summary>
		/// Sets user-defined list of indices for list of triangles.
		/// </summary>
		public static void SetCustomTriangleIndices(List<short> indices)
		{
			_primitiveType = GraphicsMode.TrianglePrimitives;
			_primitiveIndices = indices;
		}


		/// <summary>
		/// Begins primitive drawing. 
		/// It's more of a safety check for junk primitives.
		/// </summary>
		public static void Begin()
		{
			if (_primitiveVertices.Count != 0 || _primitiveIndices.Count != 0)
			{
				throw new Exception("Junk primitive data detected! Did you set index data wrong or forgot PrimitiveEnd somewhere?");
			}
		}

		/// <summary>
		/// Ends drawing of a primitive.
		/// </summary>
		public static void End()
		{
			DrawMgr.AddVertices(_primitiveType, _primitiveTexture, _primitiveVertices, _primitiveIndices.ToArray());

			_primitiveVertices.Clear();
			_primitiveIndices.Clear();
			_primitiveTexture = null;
		}


		
	}
}
