using Microsoft.Xna.Framework.Content.Pipeline;

namespace Pipefoxe.EntityTemplate
{
	[ContentProcessor(DisplayName = "Entity Template Processor - Monofoxe")]
	public class EntityTemplateProcessor : ContentProcessor<byte[], byte[]>
	{
		public override byte[] Process(byte[] bytes, ContentProcessorContext context)
		{
			var encodedBytes = new byte[bytes.Length];
			encodedBytes[0] = bytes[0];

			for(var i = 1; i < bytes.Length; i += 1)
			{
				encodedBytes[i] = (byte)((bytes[i]) ^ (bytes[i - 1]));
			}

			return encodedBytes;
		}
	}
}
