using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using de.fearvel.io.DataTypes;
using PdfSharp.Pdf;
using PdfSharp.Pdf.Advanced;
using PdfSharp.Pdf.IO;
using Tesseract;

namespace de.fearvel.io.File
{
    public static class Ocr
    {
        public static OcrDocument ScanPicturesOfAPdf(string file)
        {
            return ImageToText(Path.GetFileName(file), PdfPictureExtractor(file));
        }

        public static List<Bitmap> PdfPictureExtractor(string file)
        {
            List<Bitmap> pagePictures = new List<Bitmap>();
            PdfDocument document = PdfReader.Open(file);
            int imageCount = 0;
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

        public static OcrDocument ImageToText(string name, List<Bitmap> images, string lang = "deu")
        {
            OcrDocument ocrDocument = new OcrDocument(name);
            List<Thread> threads = new List<Thread>();
            var ocrtext = string.Empty;

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

            bool running = true;
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