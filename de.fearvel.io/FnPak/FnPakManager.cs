using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using de.fearvel.io.File;

namespace de.fearvel.io.FnPak
{
    /// <summary>
    /// EXPERIMENTAL
    /// Part of an Proprietary Encrypted file format
    /// for loading plugins
    /// <copyright>Andreas Schreiner 2019</copyright>
    /// </summary>
    public class FnPakManager
    {
        /// <summary>
        /// Dictionary of strings and FnPak's
        /// </summary>
        private readonly Dictionary<string, DataTypes.FnPak> _pakDictionary;

        /// <summary>
        /// Temporary directory for unpacking
        /// </summary>
        private string TempDir { get; set; }

        /// <summary>
        /// Creates an FnPakManager
        /// </summary>
        public FnPakManager()
        {
            _pakDictionary = new Dictionary<string, DataTypes.FnPak>();
            TempDir = @"temp\";
            Purge();
        }

        /// <summary>
        /// Destructs an FnPakManager
        /// </summary>
        ~FnPakManager()
        {
            Purge();
        }

        /// <summary>
        /// Opens an FnPak
        /// </summary>
        /// <param name="fnPak">FnPak</param>
        /// <param name="subDir">SubDir</param>
        /// <returns></returns>
        public DataTypes.FnPak OpenFnPak(string fnPak, string subDir)
        {
            var unpackPath = CheckSubPathString(TempDir) + CheckSubPathString(subDir) +
                             CheckSubPathString(Guid.NewGuid().ToString());
            DirectoryTools.CreateHiddenDirectory(TempDir);
            Directory.CreateDirectory(unpackPath);
            var pak = UnpackFnPak(ZipFile.Open(fnPak, ZipArchiveMode.Read), unpackPath);
            _pakDictionary.Add(fnPak, pak);
            return pak;
        }

        /// <summary>
        /// Unpacks an FmPak
        /// </summary>
        /// <param name="openZipArchive">openZipArchive</param>
        /// <param name="unpackPath">unpackPath</param>
        /// <returns></returns>
        private DataTypes.FnPak UnpackFnPak(ZipArchive openZipArchive, string unpackPath)
        {
            var pak = new DataTypes.FnPak(unpackPath);
            foreach (var n in openZipArchive.Entries)
            {
                if (n.Name == @"plugin.json")
                {
                    var str = n.Open();
                    var reade = new StreamReader(str);
                    string s = reade.ReadToEnd();
                    pak.PakDescriptor = FnPakDescriptor.DeSerializeToJsonFromString(s);
                }

                if (n.Name.Contains(@"plugin.pak"))
                {
                    var pluginZip = new ZipArchive(n.Open());
                    pluginZip.ExtractToDirectory(unpackPath);
                }
            }

            return pak;
        }

        /// <summary>
        /// Opens an FnPak
        /// </summary>
        /// <param name="fnPak">fnPak</param>
        /// <param name="subDir">subDir</param>
        /// <param name="password">password</param>
        /// <returns></returns>
        public DataTypes.FnPak OpenFnPak(string fnPak, string subDir, string password)
        {
            var unpackPath = CheckSubPathString(TempDir) + CheckSubPathString(subDir) +
                             CheckSubPathString(Guid.NewGuid().ToString());
            DirectoryTools.CreateHiddenDirectory(TempDir);
            Directory.CreateDirectory(unpackPath);
            var memStream = Encryption.DecryptFileToMemory(fnPak, password);
            var pak = UnpackFnPak(new ZipArchive(memStream), unpackPath);
            _pakDictionary.Add(fnPak, pak);
            return pak;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="fnPak"></param>
        public void CloseFnPak(string fnPak)
        {
            _pakDictionary.Remove(fnPak);
        }


        /// <summary>
        /// Removes all sub dirs of the TempFolder + the TempFolder itself
        /// </summary>
        private void Purge()
        {
            if (!Directory.Exists(TempDir)) return;
            try
            {
                System.IO.Directory.Delete(TempDir, true);
            }
            catch (Exception)
            {
                // ignored
            }
        }

        /// <summary>
        /// old function to cut the path from the path+filename string
        /// </summary>
        /// <param name="s">string</param>
        /// <returns>filename</returns>
        private string GetFileNameFromPath(string s)
        {
            while (s.Contains("\\"))
            {
                s = s.Substring(s.IndexOf("\\") + 1, s.Length - s.IndexOf("\\") - 1);
            }

            return s;
        }

        /// <summary>
        /// replaces \\ with \
        /// </summary>
        /// <param name="s"> string</param>
        /// <returns>modified string</returns>
        private static string CheckSubPathString(string s)
        {
            char divider = '\\';
            if (s.Length > 0)
            {
                if (s[0] == divider)
                {
                    s = s.Substring(1, s.Length - 1);
                }

                if (s[s.Length - 1] != divider)
                {
                    s += @"\";
                }
            }
            return s;
        }
    }
}