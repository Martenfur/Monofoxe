using System;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Microsoft.Xna.Framework;

namespace Monofoxe.Engine.Converters
{
	/// <summary>
	/// Vector2 JSON converter.
	/// JSON format for vectors is: {x: 0, y: 0}.
	/// </summary>
	public class Vector2Converter : BasicConverter
	{
		public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
		{
			var vec = (Vector2)value;
			
			var o = JObject.FromObject(new {vec.X, vec.Y});
	
			o.WriteTo(writer);
		}

		public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
		{
			var o = JObject.Load(reader);
	
			var x = GetFloat(o, "x");
			var y = GetFloat(o, "y");
			
			return new Vector2(x, y);
		}
	}
}
