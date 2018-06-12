using Microsoft.Xna.Framework.Content.Pipeline;
using Microsoft.Xna.Framework.Content.Pipeline.Serialization.Compiler;


namespace PipelineExt
{
	/// <summary>
	/// Atlas writer. Gets sprite data from processor and writes it into a file. 
	/// </summary>
	[ContentTypeWriter]
	public class AtlasWriter : ContentTypeWriter<AtlasContainer<Sprite>>
	{
		protected override void Write(ContentWriter output, AtlasContainer<Sprite> value)
		{
			/*
			 * File structure:
			 * - Texture (TextureContent)
			 * - Sprites count (int)
			 * 
			 *   For each sprite:
			 *   - Sprite name (string)
			 *   - Amount of frames (int)
			 *   
			 *     For each frame:
			 *     - Width (int)
			 *     - Height (int)
			 *     - Origin x (int)
			 *     - Origin y (int)
			 *     - Texture x (int)
			 *     - Texture y (int)
			 *     - Width on texture (int)
			 *     - Height on texture (int)
			 */

			output.WriteObject(value.Texture);
			
			output.Write(value.Items.Count);
			foreach(Sprite sprite in value.Items)
			{
				output.Write(sprite.Name);
				output.Write(sprite.Frames.Length);
				
				foreach(Frame frame in sprite.Frames)
				{
					output.Write(frame.Size.X);
					output.Write(frame.Size.Y);
					output.Write(frame.Origin.X);
					output.Write(frame.Origin.Y);
					output.Write(frame.TexturePos.X);
					output.Write(frame.TexturePos.Y);
					output.Write(frame.TexturePos.Width);
					output.Write(frame.TexturePos.Height);
				}
			}
		}

		public override string GetRuntimeType(TargetPlatform targetPlatform) =>
			typeof (AtlasContainer<Sprite>).AssemblyQualifiedName;

		public override string GetRuntimeReader(TargetPlatform targetPlatform) =>
			"Monofoxe.Engine.AtlasReader, Monofoxe";
	}
}
