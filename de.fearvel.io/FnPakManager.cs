using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.IO.Compression;
using System.Runtime.Remoting.Metadata.W3cXsd2001;
using System.Security.Cryptography;
using System.Runtime.InteropServices;

namespace de.fearvel.io
{
    public class FnPakManager
    {
        private Dictionary<string, string> _pakDictionary;
        private Dictionary<string, string> _decDictionary;

        private string TempDir { get; set; }

        public FnPakManager()
        {
            _pakDictionary = new Dictionary<string, string>();
            TempDir = @"temp\";
            Purge();
        }

        ~FnPakManager()
        {
            Purge();
        }





        public string OpenFnPak(string fnPak, string subDir)
        {
            var dirName = Guid.NewGuid().ToString();
            var unpackPath = CheckSubPathString(TempDir) + CheckSubPathString(subDir) + CheckSubPathString(dirName);
            DirectoryTools.CreateHiddenDirectory(TempDir);
            ZipFile.ExtractToDirectory(fnPak, unpackPath);
            _pakDictionary.Add(fnPak, unpackPath);
            return unpackPath;
        }

        public string OpenFnPak(string fnPak, string subDir, string password)
        {
            var decPath = CheckSubPathString(TempDir) + @"DEC\";
            var unpackPath = CheckSubPathString(decPath) + CheckSubPathString(Guid.NewGuid().ToString());
            var completeUnpackPath = unpackPath + GetFileNameFromPath(fnPak);
            DirectoryTools.CreateHiddenDirectory(TempDir);
            DirectoryTools.CreateHiddenDirectory(decPath);
            Directory.CreateDirectory(unpackPath);
            FileTools.DecryptFile(fnPak, completeUnpackPath, password);
            var extractPath = OpenFnPak(completeUnpackPath, subDir);
            System.IO.Directory.Delete(unpackPath, true);
            return extractPath;
        }

        

        public void CloseFnPak(string fnPak)
        {
            System.IO.Directory.Delete(_pakDictionary[fnPak], true);
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
