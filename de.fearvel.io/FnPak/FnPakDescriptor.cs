using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace de.fearvel.io.FnPak
{
    /// <summary>
    /// EXPERIMENTAL
    /// Part of an Proprietary Encrypted file format
    /// for loading plugins
    /// <copyright>Andreas Schreiner 2019</copyright>
    /// </summary>
    public class FnPakDescriptor
    {
        /// <summary>
        /// Plugin Name
        /// </summary>
        public string Name;

        /// <summary>
        /// Vendor GUID
        /// </summary>
        public Guid Vendor;

        /// <summary>
        /// Plugin GUID
        /// </summary>
        public Guid PluginIdentifier;
        
        /// <summary>
        /// Date of creation
        /// </summary>
        public DateTime DateOfCreation;

        /// <summary>
        /// Name of the plugin dll within the FnPak
        /// </summary>
        public string PluginDllName;

        /// <summary>
        /// Hashes
        /// </summary>
        public Dictionary<string, string> FileHashes;

        /// <summary>
        /// Deserialization
        /// </summary>
        /// <param name="fileName">file to deserialize</param>
        /// <returns></returns>
        public static FnPakDescriptor DeSerializeToJsonFromFile(string fileName)
        {
            var fileRead = new System.IO.StreamReader(fileName);
            return JsonConvert.DeserializeObject<FnPakDescriptor>(fileRead.ReadToEnd());
        }

        /// <summary>
        /// Deserialization
        /// </summary>
        /// <param name="jsonContent">json string</param>
        /// <returns></returns>
        public static FnPakDescriptor DeSerializeToJsonFromString(string jsonContent)
        {
            return JsonConvert.DeserializeObject<FnPakDescriptor>(jsonContent);
        }

        /// <summary>
        /// Serialization
        /// </summary>
        /// <param name="fileName">file to save</param>
        public void SerializeToJsonFile(string fileName)
        {
            string json = JsonConvert.SerializeObject(this, Formatting.Indented);
            var file = new System.IO.StreamWriter(fileName);
            file.Write(json);
            file.Close();
        }

        /// <summary>
        /// Serialization
        /// </summary>
        /// <param name="fileName">file to be serialized</param>
        /// <returns>json string</returns>
        public string SerializeToJsonString(string fileName)
        {
            return  JsonConvert.SerializeObject(this, Formatting.Indented);            
        }
    }
}
