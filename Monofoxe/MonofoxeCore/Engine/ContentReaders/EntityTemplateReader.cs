using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Monofoxe.Engine.Drawing;
using System.Collections.Generic;
using System.Diagnostics;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Text;
using System;
using System.IO;

namespace Monofoxe.Engine.ContentReaders
{
	/// <summary>
	/// Reads sprite group file.
	/// </summary>
	internal class EntityTemplateReader : ContentTypeReader<JObject>
	{
		protected override JObject Read(ContentReader input, JObject existingInstance)
		{
			var l = input.ReadInt32();
			var json = Decode(input.ReadBytes(l));

			JToken testData = JObject.Parse(raw);
			
			var mov = JsonConvert.DeserializeObject<CMovement>(testData["components"]["movement"].ToString());
			
			return JObject.Parse(json);
		}


		private string Decode(byte[] encodedBytes)
		{
			var bytes = new byte[l];

			bytes[0] = encodedBytes[0];

			for(var i = 1; i < bytes.Length; i += 1)
			{
				bytes[i] = (byte)(bytes[i - 1] ^ encodedBytes[i]);
			}
			
			return Encoding.UTF8.GetString(bytes);
		}
	}
}
