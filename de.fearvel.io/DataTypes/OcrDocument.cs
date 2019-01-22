using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace de.fearvel.io.DataTypes
{
    public class OcrDocument
    {
        public string Name;
        public List<Page> Pages;

        public OcrDocument(string name)
        {
            Name = name;
            Pages = new List<Page>();
        }

        public class Page
        {
            public int Number;
            public string Content;
        }
    }
}
