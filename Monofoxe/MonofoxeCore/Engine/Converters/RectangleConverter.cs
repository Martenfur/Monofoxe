using System;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Microsoft.Xna.Framework;

namespace Monofoxe.Engine.Converters
{
	public class RectangleConverter : BasicConverter
	{
		public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
		{
			var rectangle = (Rectangle)value;

			var o = JObject.FromObject(
				new
				{
					rectangle.X,
					rectangle.Y,
					rectangle.Width,
					rectangle.Height
				}
			);

			o.WriteTo(writer);
		}

		public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
		{
			var o = JObject.Load(reader);

			var x = GetInt(o, "x");
			var y = GetInt(o, "y");
			var width = GetInt(o, "w");
			var height = GetInt(o, "h");

			return new Rectangle(x, y, width, height);
		}
	}
}
