using Microsoft.Xna.Framework.Content.Pipeline;
using Microsoft.Xna.Framework.Content.Pipeline.Serialization.Compiler;

namespace Pipefoxe.EntityTemplate
{
	[ContentTypeWriter]
	public class EntityTemplateWriter : ContentTypeWriter<byte[]>
	{
		protected override void Write(ContentWriter output, byte[] value)
		{
			output.Write(value.Length);
			output.Write(value);
		}



		public override string GetRuntimeType(TargetPlatform targetPlatform) =>
			typeof (byte[]).AssemblyQualifiedName;



		public override string GetRuntimeReader(TargetPlatform targetPlatform) =>
			"Monofoxe.Engine.ContentReaders.EntityTemplateReader, Monofoxe.Engine";

	}
}

