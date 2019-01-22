using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using ZXing;
using ZXing.Aztec;
using ZXing.Aztec.Internal;
using ZXing.Common;
using ZXing.QrCode;

namespace de.fearvel.io.File
{
    public static class Barcode
    {
      
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
        
        public static void WriteCode(Bitmap b, string path, ImageFormat format)
        {
            b.Save(path, format);
        }

        public static void WriteCode(Bitmap b, string path)
        {
            WriteCode(b, path, ImageFormat.Jpeg);
        }

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

        public static Result ReadBarcodeWithinBitmap(Bitmap b)
        {
            var reader = new BarcodeReader();
            return reader.Decode(b);
        }
    }
}