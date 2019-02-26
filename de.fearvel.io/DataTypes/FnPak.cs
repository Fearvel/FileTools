using de.fearvel.io.FnPak;

namespace de.fearvel.io.DataTypes
{
    /// <summary>
    /// EXPERIMENTAL
    /// Part of an Proprietary Encrypted file format
    /// for loading plugins
    /// <copyright>Andreas Schreiner 2019</copyright>
    /// </summary>
    public class FnPak
    {
        /// <summary>
        /// Path to file
        /// </summary>
        public string FnPakPath { get; private set; }

        /// <summary>
        /// Descriptor
        /// </summary>
        public FnPakDescriptor PakDescriptor { get; set; }

        /// <summary>
        /// Creates an FnPak
        /// </summary>
        /// <param name="path">path of an FnPak compatible file</param>
        public FnPak(string path)
        {
            FnPakPath = path;
        }
    }
}