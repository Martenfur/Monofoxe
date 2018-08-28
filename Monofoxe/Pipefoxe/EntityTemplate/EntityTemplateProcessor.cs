using Microsoft.Xna.Framework.Content.Pipeline;

namespace Pipefoxe.EntityTemplate
{
	/// <summary>
	/// Encrypts json string with basic encryption.
	/// This won't prevent anyone from hacking into files, but
	/// entity templates won't be left as plain text files anymore.
	/// </summary>
	[ContentProcessor(DisplayName = "Entity Template Processor - Monofoxe")]
	public class EntityTemplateProcessor : ContentProcessor<byte[], byte[]>
	{
		public override byte[] Process(byte[] bytes, ContentProcessorContext context)
		{
			/*
			 * We're leaving first byte untouched and then xor rest of bytes with previous ones.
			 * We need to leave first byte unxored, because without it we won't be able to extract info back.
			 */
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
