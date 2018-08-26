using Microsoft.Xna.Framework.Content;
using System.Text;
using System;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Monofoxe.Engine.ECS;
using System.Collections.Generic;

namespace Monofoxe.Engine.ContentReaders
{
	/// <summary>
	/// Reads sprite group file.
	/// </summary>
	internal class EntityTemplateReader : ContentTypeReader<EntityTemplate>
	{
		protected override EntityTemplate Read(ContentReader input, EntityTemplate existingInstance)
		{
			var l = input.ReadInt32();
			var json = Decode(input.ReadBytes(l));

			var entityData = JObject.Parse(json);
			
			var components = new List<Component>();

			foreach(JProperty prop in ((JObject)entityData["components"]).Properties())
			{
				components.Add((Component)JsonConvert.DeserializeObject(prop.Value.ToString(), Type.GetType(prop.Name)));
			}

			return new EntityTemplate(entityData["tag"].ToString(), components.ToArray());
		}


		private string Decode(byte[] encodedBytes)
		{
			var bytes = new byte[encodedBytes.Length];

			bytes[0] = encodedBytes[0];

			for(var i = 1; i < bytes.Length; i += 1)
			{
				bytes[i] = (byte)(bytes[i - 1] ^ encodedBytes[i]);
			}
			
			return Encoding.UTF8.GetString(bytes);
		}
	}
}
