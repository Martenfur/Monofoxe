using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework.Content;
using Monofoxe.Engine.Drawing;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using System.Diagnostics;

namespace Monofoxe.Engine
{
	/// <summary>
	/// Reads atlas file.
	/// </summary>
	public class SpriteGroupReader : ContentTypeReader<Dictionary<string, Sprite>>
	{
		// NOTE: replace Dictionary with something more fitting.
		protected override Dictionary<string, Sprite> Read(ContentReader input, Dictionary<string, Sprite> existingInstance)
		{
			var texturesCount = input.ReadInt32();

			var textures = new Texture2D[texturesCount];

			for(var i = 0; i < texturesCount; i += 1)
			{
				textures[i] = input.ReadObject<Texture2D>();
			}

			var dictionary = new Dictionary<string, Sprite>();

			var spritesCount = input.ReadInt32();

			for(var i = 0; i < spritesCount; i += 1)
			{
				var spriteName = input.ReadString();
				var spriteOrigin = new Vector2(input.ReadInt32(), input.ReadInt32());
				var spriteW = input.ReadInt32();
				var spriteH = input.ReadInt32();
				var framesCount = input.ReadInt32();

				Frame[] frames = new Frame[framesCount];
				
				for(var k = 0; k < framesCount; k += 1)
				{
					var textureIndex = input.ReadInt32();
					Debug.WriteLine("TEX ID: " + textureIndex + " " + texturesCount);
					var frameTexturePos = new Rectangle(
						input.ReadInt32(), 
						input.ReadInt32(), 
						spriteW,
						spriteH
					);
				
					frames[k] = new Frame(
						textures[textureIndex], 
						frameTexturePos, 
						Vector2.Zero, 
						spriteW, 
						spriteH
					);
				}
				dictionary.Add(spriteName, new Sprite(frames, spriteOrigin));
			}

			return dictionary;
		}
	}
}
