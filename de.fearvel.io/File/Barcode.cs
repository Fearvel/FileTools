using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using ZXing;
using ZXing.Common;

namespace de.fearvel.io.File
{
    /// <summary>
    /// Static class to easily create a Barcode
    /// <copyright>Andreas Schreiner 2019</copyright>
    /// </summary>
    public static class Barcode
    {

        /// <summary>
        /// Creates barcode
        /// </summary>
        /// <param name="content">Content string</param>
        /// <param name="bf">Barcode Format</param>
        /// <param name="width">width (standard value 100)</param>
        /// <param name="height">height (standard value 100)</param>
        /// <param name="margin">Margin (standard value 0)</param>
        /// <returns></returns>
        public static Bitmap CreateBarcode(string content, BarcodeFormat bf = BarcodeFormat.QR_CODE,
            int width = 100, int height = 100, int margin = 0)
        {
            var barcodeWriter = new BarcodeWriter
            {
                Options = new EncodingOptions
                    { Width = width, Height = height, Margin = margin, PureBarcode = true},
                Format = bf
            };
            return barcodeWriter.Write(barcodeWriter.Encode(content));
        }
        
        /// <summary>
        /// Writes Bitmap to File
        /// </summary>
        /// <param name="b">Bitmap</param>
        /// <param name="path">Path to file</param>
        /// <param name="format">ImageFormat</param>
        public static void WriteCode(Bitmap b, string path, ImageFormat format)
        {
            b.Save(path, format);
        }

        /// <summary>
        /// Writes Bitmap to File (JPEG)
        /// </summary>
        /// <param name="b">Bitmap</param>
        /// <param name="fileName">Path to file</param>
        public static void WriteCode(Bitmap b, string fileName)
        {
            WriteCode(b, fileName, ImageFormat.Jpeg);
        }

        /// <summary>
        /// Reads file and provides a bitmap
        /// </summary>
        /// <param name="fileName">Path to file</param>
        /// <returns>Bitmap</returns>
        public static Bitmap ReadImageFileToBitmap(string fileName)
        {
            Bitmap bitmap;
            using (Stream bmpStream = System.IO.File.Open(fileName, FileMode.Open))
            {
                Image image = Image.FromStream(bmpStream);
                bitmap = new Bitmap(image);
            }

            return bitmap;
        }

        /// <summary>
        /// Parses a Barcode from a Bitmap
        /// </summary>
        /// <param name="b">Bitmap</param>
        /// <returns>Result</returns>
        public static Result ReadBarcodeWithinBitmap(Bitmap b)
        {
            var reader = new BarcodeReader();
            return reader.Decode(b);
        }
    }
}