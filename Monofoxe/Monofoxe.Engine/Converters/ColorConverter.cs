using System;
using System.Globalization;
using Microsoft.Xna.Framework;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Monofoxe.Engine.Converters
{
	/// <summary>
	/// Color JSON converter.
	/// Colors can be written as #RRGGBBAA or #RRGGBB.
	/// </summary>
	public class ColorConverter : BasicConverter
	{
		public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
		{
			var color = (Color)value;

			var o = JToken.FromObject(
				'#'
				+ color.R.ToString("X2") 
				+ color.G.ToString("X2") 
				+ color.B.ToString("X2") 
				+ color.A.ToString("X2") 
			);
	
			o.WriteTo(writer);
		}
		
		public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
		{
			var color = JToken.Load(reader).ToString().Replace("#", "");
			
			int r, g, b, a;

			a = 255;
			
			try
			{
				r = int.Parse(color.Substring(0, 2), NumberStyles.HexNumber);
				g = int.Parse(color.Substring(2, 2), NumberStyles.HexNumber);
				b = int.Parse(color.Substring(4, 2), NumberStyles.HexNumber);
			
				if (color.Length == 8)
				{
					a = int.Parse(color.Substring(6, 2), NumberStyles.HexNumber);
				}
				
				return new Color(r, g, b, a);
			}
			catch(Exception) 
			{	
				throw new Exception("Incorrect color format! Use #RRGGBB or #RRGGBBAA");
			}
		}
	}
}