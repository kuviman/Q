using System.IO;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace QE {

    public static class Serializer {

        class SerializationContractResolver : DefaultContractResolver {
            public SerializationContractResolver() {
                IgnoreSerializableAttribute = false;
            }
        }

        static JsonSerializerSettings serializationSettings = new JsonSerializerSettings {
            TypeNameHandling = TypeNameHandling.All,
            ObjectCreationHandling = ObjectCreationHandling.Replace,
            ContractResolver = new SerializationContractResolver(),
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