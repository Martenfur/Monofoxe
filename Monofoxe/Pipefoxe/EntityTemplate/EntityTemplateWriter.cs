using Microsoft.Xna.Framework.Content.Pipeline;
using Microsoft.Xna.Framework.Content.Pipeline.Serialization.Compiler;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Serialization;
using System;
using System.Collections.Generic;

namespace Pipefoxe.SpriteGroup
{
	/// <summary>
	/// Atlas writer. Gets sprite data from processor and writes it into a file. 
	/// </summary>
	[ContentTypeWriter]
	public class EntityTemplateWriter : ContentTypeWriter<byte[]>
	{
		protected override void Write(ContentWriter output, byte[] value)
		{
			output.Write(value.Length);
			output.Write(value);
		}



		public override string GetRuntimeType(TargetPlatform targetPlatform) =>
			typeof (JObject).AssemblyQualifiedName;



		public override string GetRuntimeReader(TargetPlatform targetPlatform) =>
			"Monofoxe.Engine.ContentReaders.EntityTemplateReader, Monofoxe";

	}
}

