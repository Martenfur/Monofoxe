using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Monofoxe.Engine.Drawing;

namespace Monofoxe.Engine.ContentReaders
{
	/// <summary>
	/// Reads sprite group file.
	/// </summary>
	internal class SpriteGroupReader : ContentTypeReader<Dictionary<string, Sprite>>
	{

		protected override Dictionary<string, Sprite> Read(ContentReader input, Dictionary<string, Sprite> existingInstance)
		{
			var texturesCount = input.ReadInt32();

			var textures = new Texture2D[texturesCount];

			// Reading textures.
			for(var i = 0; i < texturesCount; i += 1)
			{
				var w = input.ReadInt32();
				var h = input.ReadInt32();
				var texture = new Texture2D(GameMgr.Game.GraphicsDevice, w, h, false, SurfaceFormat.Color);
			
				var pixels = new Color[w * h];

				for(var k = 0; k < pixels.Length; k += 1)
				{ 
					pixels[k] = input.ReadColor();
				}
				texture.SetData(pixels);

				textures[i] = texture;
			}
			
			Debug.WriteLine(input.AssetName + ": " + textures.Length + " textures loaded!");

			var dictionary = new Dictionary<string, Sprite>();

			var spritesCount = input.ReadInt32();

			// Reading sprite info.
			for(var i = 0; i < spritesCount; i += 1)
			{
				var spriteName = input.ReadString();
				var spriteOrigin = new Vector2(input.ReadInt32(), input.ReadInt32());
				var spriteW = input.ReadInt32();
				var spriteH = input.ReadInt32();
				var framesCount = input.ReadInt32();

				var frames = new Frame[framesCount];
				
				for(var k = 0; k < framesCount; k += 1)
				{
					var textureIndex = input.ReadInt32();

					var frameTexturePos = new RectangleF(
						input.ReadInt32(), 
						input.ReadInt32(), 
						spriteW,
						spriteH
					);
					
					frames[k] = new Frame(
						textures[textureIndex], 
						frameTexturePos, 
						Vector2.Zero
					);
				}
				dictionary.Add(spriteName, new Sprite(frames, spriteOrigin, spriteName));
			}
			
			//input.Dispose();
			//System.GC.Collect();
			return dictionary;
		}
	}
}
