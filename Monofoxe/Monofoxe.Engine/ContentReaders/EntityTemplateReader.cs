using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;
using Microsoft.Xna.Framework.Content;
using Monofoxe.Engine.ECS;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Monofoxe.Engine.ContentReaders
{
	/// <summary>
	/// Reads and unencrypts entity templates.
	/// </summary>
	internal class EntityTemplateReader : ContentTypeReader<EntityTemplate>
	{
		/// <summary>
		/// Suggested namespaces for component classes. 
		/// </summary>
		internal static string[] _namespaces = new string[0];


		protected override EntityTemplate Read(ContentReader input, EntityTemplate existingInstance)
		{
			var l = input.ReadInt32();
			var json = Decode(input.ReadBytes(l));

			var entityData = JObject.Parse(json);
			
			var components = new List<Component>();
			var settings = new JsonSerializerSettings();
			
			// Converting from json to entity template object.
			foreach(JProperty prop in ((JObject)entityData["components"]).Properties())
			{
				
				components.Add(
					(Component)JsonConvert.DeserializeObject(
						prop.Value.ToString(), 
						GetEntityType(prop.Name)
					)
				);
			}

			return new EntityTemplate(entityData["tag"].ToString(), components.ToArray());
		}


		/// <summary>
		/// Returns entity type from string with mathing namespace. 
		/// </summary>
		private Type GetEntityType(string typeName)
		{
			Type type = null;
			try
			{
				type = Type.GetType(typeName + ", " + Assembly.GetEntryAssembly(), true);
			}
			catch(TypeLoadException)
			{
				// If type isn't found, check all suggested namespaces.
				foreach(var entityNamespace in _namespaces)
				{
					try
					{
						type = Type.GetType(entityNamespace + "." + typeName + ", " + Assembly.GetEntryAssembly(), true);
						break; // Break, if type was successfully found. Won't be executed on exception.
					}
					catch(TypeLoadException){}
				}
				if (type == null)
				{
					throw new TypeLoadException("Failed to load component for " + typeName + " - no namespaces match.");
				}
			}
			return type;
		}


		/// <summary>
		/// Decrypting entity template json.
		/// </summary>
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
