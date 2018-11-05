using System.IO;
using Newtonsoft.Json;
using UnityEngine;

namespace Configs {
    public class ConfigBase {
        public const string BaseConfigPath = "/Configs/";         
        public string ConfigPath = string.Empty;

        public void Read() {
            if (string.IsNullOrEmpty(ConfigPath)) {
                Debug.LogError("Cannot read config: config name is not set up.");
                return;
            }
            
            var serializer = new JsonSerializer {NullValueHandling = NullValueHandling.Ignore};
            using (StreamReader sr = new StreamReader(BaseConfigPath + ConfigPath))
            using (JsonReader reader = new JsonTextReader(sr)) {
                var deserialized = serializer.Deserialize(reader, GetType());
                // TODO: Config read (from controller?)
            }
        }
        
        public void Write() {
            if (string.IsNullOrEmpty(ConfigPath)) {
                Debug.LogError("Cannot write config: config name is not set up.");
                return;
            }

            var serializer = new JsonSerializer {NullValueHandling = NullValueHandling.Ignore};
            using (StreamWriter sw = new StreamWriter(BaseConfigPath + ConfigPath))
            using (JsonWriter writer = new JsonTextWriter(sw)) {
                serializer.Serialize(writer, this);
            }
        }
    }
}