using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using de.fearvel.io.File;

namespace de.fearvel.io.FnPak
{
    public class FnPakManager
    {
        private Dictionary<string, DataTypes.FnPak> _pakDictionary;

        private string TempDir { get; set; }

        public FnPakManager()
        {
            _pakDictionary = new Dictionary<string, DataTypes.FnPak>();
            TempDir = @"temp\";
            Purge();            
        }

        ~FnPakManager()
        {
            Purge();
        }
        public DataTypes.FnPak OpenFnPak(string fnPak, string subDir)
        {
            var unpackPath = CheckSubPathString(TempDir) + CheckSubPathString(subDir) + CheckSubPathString(Guid.NewGuid().ToString());
            DirectoryTools.CreateHiddenDirectory(TempDir);
            Directory.CreateDirectory(unpackPath);
            var pak = UnpackFnPak(ZipFile.Open(fnPak, ZipArchiveMode.Read), unpackPath);
            _pakDictionary.Add(fnPak, pak);
            return pak;
        }

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
                    pak.PakDiscriptor = FnPakDiscriptor.DeSerializeToJsonFromString(s);
                }

                if (n.Name.Contains(@"plugin.pak"))
                {
                    var pluginZip = new ZipArchive(n.Open());
                    pluginZip.ExtractToDirectory(unpackPath);
                }
            }
            return pak;
        }

        public DataTypes.FnPak OpenFnPak(string fnPak, string subDir, string password)
        {           
            var unpackPath = CheckSubPathString(TempDir) + CheckSubPathString(subDir) + CheckSubPathString(Guid.NewGuid().ToString());
            DirectoryTools.CreateHiddenDirectory(TempDir);
            Directory.CreateDirectory(unpackPath);
            var memStream = Encryption.DecryptFileToMemory(fnPak, password);
            var pak = UnpackFnPak(new ZipArchive(memStream), unpackPath);
            _pakDictionary.Add(fnPak, pak);
            return pak;
        }



        public void CloseFnPak(string fnPak)
        {
            _pakDictionary.Remove(fnPak);
        }

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


        private string GetFileNameFromPath(string s)
        {
            while (s.Contains("\\"))
            {
                s = s.Substring(s.IndexOf("\\") + 1, s.Length - s.IndexOf("\\") - 1);
            }

            return s;
        }

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
