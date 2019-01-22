using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace de.fearvel.io.FnPak
{
    public class FnPakDiscriptor
    {
        public string Name;
        public Guid Vendor;
        public Guid PluginIdentifier;
        public DateTime DateOfCreation;
        public string PluginDllName;
        public Dictionary<string, string> FileHashes;

        public static FnPakDiscriptor DeSerializeToJsonFromFile(string fileName)
        {
            var fileRead = new System.IO.StreamReader(fileName);
            return JsonConvert.DeserializeObject<FnPakDiscriptor>(fileRead.ReadToEnd());
        }
        public static FnPakDiscriptor DeSerializeToJsonFromString(string jsonConent)
        {
            return JsonConvert.DeserializeObject<FnPakDiscriptor>(jsonConent);
        }

        public void SerializeToJsonFile(string fileName)
        {
            string json = JsonConvert.SerializeObject(this, Formatting.Indented);
            var file = new System.IO.StreamWriter(fileName);
            file.Write(json);
            file.Close();
        }
        public string SerializeToJsonString(string fileName)
        {
            return  JsonConvert.SerializeObject(this, Formatting.Indented);            
        }

    }
}
