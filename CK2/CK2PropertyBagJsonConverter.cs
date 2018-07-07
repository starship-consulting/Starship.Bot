using System;
using Newtonsoft.Json;

namespace Starship.Bot.CK2 {
    public class CK2PropertyBagJsonConverter : JsonConverter<CK2Object> {

        public override void WriteJson(JsonWriter writer, CK2Object value, JsonSerializer serializer) {
            writer.WriteStartObject();
            
            foreach(var each in value) {
                serializer.Serialize(writer, each.Value);
            }

            writer.WriteEndObject();
        }

        public override CK2Object ReadJson(JsonReader reader, Type objectType, CK2Object existingValue, bool hasExistingValue, JsonSerializer serializer) {
            throw new NotImplementedException();
        }
    }
}