using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace de.fearvel.io
{
    static class DirectoryTools
    {
        public static void CreateHiddenDirectory(string path)
        {
            if (!System.IO.Directory.Exists(path))
            {
                DirectoryInfo di = System.IO.Directory.CreateDirectory(path);
                di.Attributes = FileAttributes.Directory | FileAttributes.Hidden;
            }
        }
    }
}
