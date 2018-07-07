using System;
using System.Linq;
using Newtonsoft.Json;

namespace Starship.Bot.CK2 {
    public class CK2TokenJsonConverter : JsonConverter<CK2Token> {

        public CK2TokenJsonConverter() {
            WriteEmptyObjects = false;
        }

        public override void WriteJson(JsonWriter writer, CK2Token value, JsonSerializer serializer) {
            
            switch (value.Type) {
                
                case CK2TokenTypes.List:

                    if(writer.WriteState != WriteState.Property) {
                        writer.WritePropertyName(value.Key);
                    }

                    writer.WriteStartArray();

                    foreach(var item in value.GetList()) {
                        serializer.Serialize(writer, item);
                    }
                    
                    writer.WriteEndArray();
                    break;

                case CK2TokenTypes.Object:
                    
                    if(!WriteEmptyObjects && !value.GetObject().Any()) {
                        return;
                    }

                    if(writer.WriteState != WriteState.Property) {
                        writer.WritePropertyName(value.Key);
                    }
                    
                    serializer.Serialize(writer, value.Value);
                    break;

                default:
                    WritePropertyNameValue(writer, value.Key, value.Value, serializer);
                    break;
            }
        }

        public override CK2Token ReadJson(JsonReader reader, Type objectType, CK2Token existingValue, bool hasExistingValue, JsonSerializer serializer) {
            throw new NotImplementedException();
        }

        private void WritePropertyNameValue<TValue>(JsonWriter writer, string key, TValue value, JsonSerializer serializer) where TValue : class {
            
            if(writer.WriteState == WriteState.Object) {
                writer.WritePropertyName(key);
            }
            
            if(value is CK2Token) {
                WriteJson(writer, value as CK2Token, serializer);
            }
            else {
                writer.WriteValue(value);
            }
        }

        public bool WriteEmptyObjects { get; set; }
    }
}