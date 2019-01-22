using System.IO;

namespace de.fearvel.io.File
{
    static class DirectoryTools
    {
        public static void CreateHiddenDirectory(string path)
        {
            if (!Directory.Exists(path))
            {
                DirectoryInfo di = System.IO.Directory.CreateDirectory(path);
                di.Attributes = FileAttributes.Directory | FileAttributes.Hidden;
            }
        }
    }
}
