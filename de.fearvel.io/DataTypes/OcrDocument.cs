using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace de.fearvel.io.DataTypes
{
    /// <summary>
    /// EXPERIMENTAL
    /// Wrapper to combine the name and the content of the read PDF
    /// <copyright>Andreas Schreiner 2019</copyright>
    /// </summary>
    public class OcrDocument
    {
        /// <summary>
        /// Name of the Document
        /// </summary>
        public string Name;

        /// <summary>
        /// List of Pages of a document
        /// </summary>
        public List<Page> Pages;

        /// <summary>
        /// Creates an OcrDocument
        /// </summary>
        /// <param name="name">Name of the document</param>
        public OcrDocument(string name)
        {
            Name = name;
            Pages = new List<Page>();
        }

        /// <summary>
        /// Nested Class for wrapping the text of an picture of a page whit the page number
        /// </summary>
        public class Page
        {
            /// <summary>
            /// Page number
            /// </summary>
            public int Number;

            /// <summary>
            /// content of this page
            /// </summary>
            public string Content;
        }
    }
}
