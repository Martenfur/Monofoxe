using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Monofoxe.Engine.Utils;

namespace Monofoxe.Engine.Drawing
{
	public abstract class Primitive2D : IDrawable
	{
		public Vector2 Position {get; set;}

		/// <summary>
		/// First triangle point. 
		/// NOTE: all line points treat position as an origin point;
		/// </summary>
		public List<Vertex> Vertices;
		
		protected abstract GraphicsMode _type {get;}
		
		protected Texture2D _texture;
		protected Vector2 _textureOffset;
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
		

		protected abstract short[] GetIndices();

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
			DrawMgr.AddVertices(
				_type, 
				_texture, 
				GetConvertedVertices(), 
				GetIndices()
			);
			
			_texture = null;
		}

		
	}
}
