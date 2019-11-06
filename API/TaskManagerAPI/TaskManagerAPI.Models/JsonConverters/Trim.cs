using System;
using Newtonsoft.Json;

namespace TaskManagerAPI.Models.JsonConverters
{
    public class Trim : JsonConverter
    {
        public override bool CanConvert(Type objectType)
        {
            return objectType == typeof(string);
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            if (reader.TokenType != JsonToken.String)
            {
                throw new FormatException($"TrimmingAndLowerCaseString: Unexpected parsing type. Expected string, got {reader.TokenType}.");
            }
            return reader.Value.ToString().Trim();
        }

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            throw new NotImplementedException();
        }
    }
}
