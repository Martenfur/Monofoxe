using Microsoft.Xna.Framework.Content.Pipeline;
using Microsoft.Xna.Framework.Content.Pipeline.Serialization.Compiler;
using System.Collections.Generic;
using System.Drawing;
using System;

namespace Pipefoxe.SpriteGroup
{
	/// <summary>
	/// Atlas writer. Gets sprite data from processor and writes it into a file. 
	/// </summary>
	[ContentTypeWriter]
	public class SpriteGroupWriter : ContentTypeWriter<(List<RawSprite>, List<Bitmap>)>
	{
		protected override void Write(ContentWriter output, (List<RawSprite>, List<Bitmap>) value)
		{
			/*
			 * File structure:
			 * - Textures count (int)
			 * 
			 *   For each texture:
			 *   - Texture (TextureContent)
			 *  
			 * - Sprites count (int)
			 * 
			 *   For each sprite:
			 *   - Sprite name (string)   
			 *   - Origin x (int)
			 *   - Origin y (int)
			 *   - Width (int)
			 *   - Height (int)
			 *   - Amount of frames (int)
			 *   
			 *     For each frame:
			 *     - Texture id (int)
			 *     - Texture x (int)
			 *     - Texture y (int)
			 */
			
			var sprites = value.Item1;
			var bitmaps = value.Item2;

			// Writing textures.
			//output.WriteObject(value.Texture);
			// Writing textures.


			// Writing sprites.
			output.Write(sprites.Count);
			foreach(RawSprite sprite in sprites)
			{
				output.Write(sprite.Name);
				
				output.Write(sprite.Offset.X);
				output.Write(sprite.Offset.Y);

				output.Write(sprite.Frames[0].TexturePos.Width);
				output.Write(sprite.Frames[0].TexturePos.Height);
				
				output.Write(sprite.Frames.Count);
				
				foreach(Frame frame in sprite.Frames)
				{
					output.Write(frame.TextureIndex);
					output.Write(frame.TexturePos.X);
					output.Write(frame.TexturePos.Y);
				}
			}
			// Writing sprites.
		}

		public override string GetRuntimeType(TargetPlatform targetPlatform) =>
			typeof (Tuple<List<RawSprite>, List<Bitmap>>).AssemblyQualifiedName;

		public override string GetRuntimeReader(TargetPlatform targetPlatform) =>
			"Monofoxe.Engine.SpriteGroupReader, Monofoxe";
	}
}
