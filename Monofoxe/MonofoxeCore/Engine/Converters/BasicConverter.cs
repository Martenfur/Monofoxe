using System;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Monofoxe.Engine.Converters
{
	public abstract class BasicConverter : JsonConverter
	{
		public override bool CanConvert(Type objectType) =>
			throw(new NotImplementedException());

		protected static int GetInt(JObject o, string tokenName)
		{
			JToken t;
			return o.TryGetValue(tokenName, StringComparison.InvariantCultureIgnoreCase, out t) ? (int)t : 0;
		}

		protected static float GetFloat(JObject o, string tokenName)
		{
			JToken t;
			return o.TryGetValue(tokenName, StringComparison.InvariantCultureIgnoreCase, out t) ? (float)t : 0f;
		}
		
		protected static string GetString(JObject o, string tokenName)
		{
			JToken t;
			return o.TryGetValue(tokenName, StringComparison.InvariantCultureIgnoreCase, out t) ? (string)t : "";
		}

	}
}
