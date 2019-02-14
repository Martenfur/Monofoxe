using System;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Monofoxe.Engine.Converters
{
	/// <summary>
	/// Basic abstract json converter. Can be inherited to create new converters. 
	/// </summary>
	public abstract class BasicConverter : JsonConverter
	{
		public override bool CanConvert(Type objectType) =>
			throw new NotImplementedException();

		protected static int GetInt(JObject o, string tokenName)
		{
			return o.TryGetValue(tokenName, StringComparison.InvariantCultureIgnoreCase, out JToken t) ? (int)t : 0;
		}

		protected static float GetFloat(JObject o, string tokenName)
		{
			return o.TryGetValue(tokenName, StringComparison.InvariantCultureIgnoreCase, out JToken t) ? (float)t : 0f;
		}
		
		protected static string GetString(JObject o, string tokenName)
		{
			return o.TryGetValue(tokenName, StringComparison.InvariantCultureIgnoreCase, out JToken t) ? (string)t : "";
		}

	}
}
