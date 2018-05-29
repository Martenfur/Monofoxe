using Microsoft.Xna.Framework.Content.Pipeline;
using Microsoft.Xna.Framework.Content.Pipeline.Serialization.Compiler;


namespace PipelineExt
{
	[ContentTypeWriter]
	public class AtlassWriter : ContentTypeWriter<AtlassSprites>
	{
		protected override void Write(ContentWriter output, AtlassSprites value)
		{
			output.WriteObject(value.Texture);
			
			output.Write(value.Sprites.Count);
			foreach(Sprite sprite in value.Sprites)
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
			typeof (AtlassSprites).AssemblyQualifiedName;

		public override string GetRuntimeReader(TargetPlatform targetPlatform) =>
			"Monofoxe.Engine.AtlassReader, Monofoxe";
	}
}
