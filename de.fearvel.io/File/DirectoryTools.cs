using System.IO;

namespace de.fearvel.io.File
{
    /// <summary>
    /// Static class for custom functions and methods for Directory operations
    /// </summary>
    public static class DirectoryTools
    {
        /// <summary>
        /// Creates a hidden Directory
        /// </summary>
        /// <param name="dir">Folder that will be created and hidden</param>
        public static void CreateHiddenDirectory(string dir)
        {
            if (Directory.Exists(dir)) return;
            var di = System.IO.Directory.CreateDirectory(dir);
            di.Attributes = FileAttributes.Directory | FileAttributes.Hidden;
        }


        /// <summary>
        /// Hides a directory
        /// </summary>
        /// <param name="dir">Folder that will be hidden</param>
        public static void HideDirectory(string dir)
        {
            if (!Directory.Exists(dir)) return;
            var di = new DirectoryInfo(dir) {Attributes = FileAttributes.Directory | FileAttributes.Hidden};
        }
    }
}
