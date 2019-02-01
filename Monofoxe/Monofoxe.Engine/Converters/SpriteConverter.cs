using System;
using System.Reflection;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Monofoxe.Engine.Converters
{
	/// <summary>
	/// Sprite JSON converter.
	/// Sprites can only be deserialized as static fields in some class.
	/// For example: Resources.Sprites.SpritesDefault.Foxe.
	/// 
	/// NOTE: Sprites cannot be serialized!
	/// </summary>
	public class SpriteConverter : BasicConverter
	{
		/// <summary>
		/// Suggested namespaces for sprite class. 
		/// </summary>
		internal string[] _namespaces = {""};

		public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer) =>
			throw new InvalidOperationException("Cannot serialize sprites!");

		public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
		{
			var name = JToken.Load(reader).ToString();
			var words = name.Split(new char[]{'.'});

			var fieldName = words[words.Length - 1];
			var className = name.Substring(0, name.Length - fieldName.Length - 1);


			Type type = null;
			try
			{
				type = Type.GetType(className + ", " + Assembly.GetEntryAssembly(), true);
			}
			catch(TypeLoadException)
			{
				// If type isn't found, check all suggested namespaces.
				foreach(var spriteNamespace in _namespaces)
				{
					try
					{
						type = Type.GetType(spriteNamespace + "." + className + ", " + Assembly.GetEntryAssembly(), true);
						break; // Break, if type was successfully found. Won't be executed on exception.
					}
					catch(TypeLoadException){}
				}
				if (type == null)
				{
					throw new TypeLoadException("Failed to load sprite from " + className + " - no namespaces match.");
				}
			}


			return type.GetField(fieldName).GetValue(null);
		}
	}
}