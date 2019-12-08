using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Monofoxe.Engine.Utils;

namespace Monofoxe.Engine.Drawing
{
	/// <summary>
	/// Base 2D primitive class. Can be used to create other types of primitives.
	/// </summary>
	public abstract class Primitive2D : IDrawable
	{
		public Vector2 Position {get; set;}

		/// <summary>
		/// List of all primitive's vertices. 
		/// NOTE: all vertices treat position as an origin point;
		/// </summary>
		public List<Vertex> Vertices;
		
		/// <summary>
		/// Graphics mode which will be used while drawing primitive.
		/// </summary>
		protected abstract PrimitiveType _primitiveType {get;}
		
		/// <summary>
		/// Frame texture.
		/// NOTE: Frame and be only a small part of a big texture.
		/// </summary>
		protected Texture2D _texture;
		/// <summary>
		/// Offset of a texture region.
		/// </summary>
		protected Vector2 _textureOffset;
		/// <summary>
		/// Ratio between texture size and frame size.
		/// </summary>
		protected Vector2 _textureRatio;


		/// <summary>
		/// Sets texture for a primitive.
		/// </summary>
		public void SetTexture(Texture2D texture)
		{
			_texture = texture;
			_textureOffset = Vector2.Zero;
			_textureRatio = Vector2.One;
		}

		/// <summary>
		/// Sets texture for a primitive.
		/// </summary>
		public void SetTextureFromFrame(Frame frame)
		{
			if (frame != null)
			{
				_texture = frame.Texture;
				
				_textureOffset = new Vector2(
					frame.TexturePosition.X / (float)frame.Texture.Width, 
					frame.TexturePosition.Y / (float)frame.Texture.Height
				);
			
				_textureRatio = new Vector2(
					frame.TexturePosition.Width / (float)frame.Texture.Width, 
					frame.TexturePosition.Height / (float)frame.Texture.Height
				);
			}
			else
			{
				SetTexture(null);
			}
		}
		

		/// <summary>
		/// Returns an array of vertex indices which essentially determine how primitive will be drawn.
		/// </summary>
		protected abstract short[] GetIndices();

		/// <summary>
		/// Converts list of Monofoxe Vertex objects to Monogame's vertices.
		/// </summary>
		protected List<VertexPositionColorTexture> GetConvertedVertices()
		{
			var vertices = new List<VertexPositionColorTexture>();
			foreach(var vertex in Vertices)
			{
				// Since we may work with sprites, which are only little parts of whole texture atlas,
				// we need to convert local sprite coordinates to global atlas coordinates.
				var atlasPos = _textureOffset + vertex.TexturePosition * _textureRatio;
				vertices.Add(new VertexPositionColorTexture((vertex.Position + Position).ToVector3(), vertex.Color, atlasPos));
			}

			return vertices;
		}

		
		public void Draw()
		{
			GraphicsMgr.VertexBatch.Texture = _texture;
			GraphicsMgr.VertexBatch.DrawPrimitive(_primitiveType, GetConvertedVertices().ToArray(), GetIndices());
		}

		
	}
}
