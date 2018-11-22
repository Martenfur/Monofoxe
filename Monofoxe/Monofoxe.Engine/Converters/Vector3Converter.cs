using System;
using Microsoft.Xna.Framework;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Monofoxe.Engine.Converters
{
	/// <summary>
	/// Vector3 JSON converter.
	/// JSON format for vectors is: {x: 0, y: 0, z: 0}.
	/// </summary>
	public class Vector3Converter : BasicConverter
	{
		public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
		{
			var vec = (Vector3)value;

			var o = JObject.FromObject(new {vec.X, vec.Y, vec.Z});

			o.WriteTo(writer);
		}

		public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
		{
			var o = JObject.Load(reader);

			var x = GetFloat(o, "x");
			var y = GetFloat(o, "y");
			var z = GetFloat(o, "z");

			return new Vector3(x, y, z);
		}
	}
}