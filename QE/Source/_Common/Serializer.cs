using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace QE {

    public static class Serializer {

        class DictionaryJsonConverter : JsonConverter {
            public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer) {
                var dictionary = (IDictionary)value;

                writer.WriteStartArray();

                foreach (var key in dictionary.Keys) {
                    writer.WriteStartObject();

                    writer.WritePropertyName("Key");

                    serializer.Serialize(writer, key);

                    writer.WritePropertyName("Value");

                    serializer.Serialize(writer, dictionary[key]);

                    writer.WriteEndObject();
                }

                writer.WriteEndArray();
            }

            public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer) {
                if (!CanConvert(objectType))
                    throw new Exception(string.Format("This converter is not for {0}.", objectType));

                var keyType = objectType.GetGenericArguments()[0];
                var valueType = objectType.GetGenericArguments()[1];
                var dictionaryType = typeof(Dictionary<,>).MakeGenericType(keyType, valueType);
                var result = (IDictionary)Activator.CreateInstance(dictionaryType);

                if (reader.TokenType == JsonToken.Null)
                    return null;

                while (reader.Read()) {
                    if (reader.TokenType == JsonToken.EndArray) {
                        return result;
                    }

                    if (reader.TokenType == JsonToken.StartObject) {
                        AddObjectToDictionary(reader, result, serializer, keyType, valueType);
                    }
                }

                return result;
            }

            public override bool CanConvert(Type objectType) {
                return objectType.IsGenericType && (objectType.GetGenericTypeDefinition() == typeof(IDictionary<,>) || objectType.GetGenericTypeDefinition() == typeof(Dictionary<,>));
            }

            private void AddObjectToDictionary(JsonReader reader, IDictionary result, JsonSerializer serializer, Type keyType, Type valueType) {
                object key = null;
                object value = null;

                while (reader.Read()) {
                    if (reader.TokenType == JsonToken.EndObject && key != null) {
                        result.Add(key, value);
                        return;
                    }

                    var propertyName = reader.Value.ToString();
                    if (propertyName == "Key") {
                        reader.Read();
                        key = serializer.Deserialize(reader, keyType);
                    } else if (propertyName == "Value") {
                        reader.Read();
                        value = serializer.Deserialize(reader, valueType);
                    }
                }
            }
        }

        class SerializationContractResolver : DefaultContractResolver {
            public SerializationContractResolver() {
                IgnoreSerializableAttribute = false;
            }
            protected override JsonProperty CreateProperty(MemberInfo member, MemberSerialization memberSerialization) {
                var prop = base.CreateProperty(member, memberSerialization);
                if (prop.PropertyType.IsSubclassOf(typeof(Delegate)))
                    prop.ShouldSerialize = (o) => false;
                return prop;
            }
        }

        static JsonSerializerSettings serializationSettings = new JsonSerializerSettings {
            TypeNameHandling = TypeNameHandling.All,
            ObjectCreationHandling = ObjectCreationHandling.Replace,
            ContractResolver = new SerializationContractResolver(),
            Converters = { new DictionaryJsonConverter() },
            ConstructorHandling = ConstructorHandling.AllowNonPublicDefaultConstructor,
        };

        public static string ToJson(object o) {
            return JsonConvert.SerializeObject(o, serializationSettings);
        }

        public static T FromJson<T>(string json) {
            return JsonConvert.DeserializeObject<T>(json, serializationSettings);
        }

        public static byte[] ToBytes(object o) {
            return Compressor.Compress(GUtil.GetBytes(ToJson(o)));
        }

        public static T FromBytes<T>(byte[] data) {
            return FromJson<T>(GUtil.GetString(Compressor.Decompress(data)));
        }

        public static void Dump(object o, string path) {
            File.WriteAllBytes(path, ToBytes(o));
        }

        public static T Load<T>(string path) {
            return FromBytes<T>(File.ReadAllBytes(path));
        }

    }

}