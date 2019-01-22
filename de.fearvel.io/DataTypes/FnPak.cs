using de.fearvel.io.FnPak;

namespace de.fearvel.io.DataTypes
{
    public class FnPak
    {
        public string FnPakPath { get; private set; }
        public FnPakDiscriptor PakDiscriptor { get; set; }

        public FnPak(string path)
        {
            FnPakPath = path;
        }

    }
}
