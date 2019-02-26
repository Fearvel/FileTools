using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Threading;
using de.fearvel.io.DataTypes;
using PdfSharp.Pdf;
using PdfSharp.Pdf.Advanced;
using PdfSharp.Pdf.IO;
using Tesseract;

namespace de.fearvel.io.File
{
    /// <summary>
    /// EXPERIMENTAL
    /// Ocr Class
    /// </summary>
    public static class Ocr
    {
        /// <summary>
        /// function to receive Text from a pdf
        /// </summary>
        /// <param name="pdf">PDF File</param>
        /// <returns></returns>
        public static OcrDocument ScanPicturesOfAPdf(string pdf)
        {
            return ImageToText(Path.GetFileName(pdf), PdfPictureExtractor(pdf));
        }

        /// <summary>
        /// Extracts all pictures of an Pdf
        /// </summary>
        /// <param name="pdf">FileLocation of the PDF</param>
        /// <returns>List of Bitmaps of the PDF</returns>
        public static List<Bitmap> PdfPictureExtractor(string pdf)
        {
            List<Bitmap> pagePictures = new List<Bitmap>();
            PdfDocument document = PdfReader.Open(pdf);
            foreach (PdfPage page in document.Pages)
            {
                var resources = page.Elements.GetDictionary("/Resources");
                if (resources == null) continue;
                var xObjects = resources.Elements.GetDictionary("/XObject");
                if (xObjects == null) continue;
                var items = xObjects.Elements.Values;
                foreach (var item in items)
                {
                    PdfReference reference = item as PdfReference;
                    PdfDictionary xObject = reference?.Value as PdfDictionary;
                    if (xObject == null || xObject.Elements.GetString("/Subtype") != "/Image") continue;
                    byte[] stream = xObject.Stream.Value;
                    using (var ms = new MemoryStream(stream))
                    {
                        var a = new Bitmap(ms);
                        pagePictures.Add(a);
                    }
                }
            }
            return pagePictures;
        }
    
        /// <summary>
        /// Reads Text from images
        /// </summary>
        /// <param name="name">Name of the OCR Document</param>
        /// <param name="images">List of bitmaps to be read</param>
        /// <param name="lang">language of the Text within the pictures</param>
        /// <returns></returns>
        public static OcrDocument ImageToText(string name, List<Bitmap> images, string lang = "deu")
        {
            var ocrDocument = new OcrDocument(name);
            var threads = new List<Thread>();
            for (int i = 0; i < images.Count -1; i++)
            {
                var thread = new Thread(
                    () =>
                    {
                        using (var engine = new TesseractEngine(@"./tessdata", lang, EngineMode.Default))
                        {
                            using (var img = PixConverter.ToPix(images[i]))
                            {
                                using (var page = engine.Process(img))
                                {
                                    ocrDocument.Pages.Add(new OcrDocument.Page(){Number = i+1, Content = page.GetText()});
                                }
                            }
                        }
                    });
                thread.Start();
                threads.Add(thread);
            }
            var running = true;
            while (running)
            {
                running = false;
                foreach (var thread in threads)
                {
                    if (thread.ThreadState == ThreadState.Running)
                    {
                        running = true;
                    }
                }
            }
            return ocrDocument;
        }
    }
}