using System;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Reflection;

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
		public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer) =>
			throw(new InvalidOperationException("Cannot serialize sprites!"));

		public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
		{
			var name = JToken.Load(reader).ToString();
			var words = name.Split(new char[]{'.'});

			var fieldName = words[words.Length - 1];
			var className = name.Substring(0, name.Length - fieldName.Length - 1);

			var type = Type.GetType(className + ", " + Assembly.GetEntryAssembly(), true);
			
			return type.GetField(fieldName).GetValue(null);
		}
	}
}