using System.IO;
using Infozdrav.Web.Abstractions;
using QRCoder;
using Syncfusion.Drawing;
using Syncfusion.Pdf;
using Syncfusion.Pdf.Graphics;
using Syncfusion.Pdf.Graphics.Images.Decoder;
using ImageFormat = System.Drawing.Imaging.ImageFormat;

namespace Infozdrav.Web.Services
{
    public class QRCodeService : IDependency
    {
        public Stream GenerateQRCodeImage(string data)
        {
            var qrGenerator = new QRCodeGenerator();
            var qrCodeData = qrGenerator.CreateQrCode(data, QRCodeGenerator.ECCLevel.H);
            var qrCode = new QRCode(qrCodeData);
            var gfx = qrCode.GetGraphic(30);
            var stream = new MemoryStream();
            gfx.Save(stream, ImageFormat.Jpeg);
            stream.Reset();
            return stream;
        }

        public MemoryStream GetQrPdf(string displayString, string data)
        {
            const int width = 250;
            const int height = 125;

            const int qrSize = 100;

            var document = new PdfDocument
            {
                PageSettings =
                {
                    Size = new Syncfusion.Drawing.SizeF(250, 125),
                    Orientation = PdfPageOrientation.Landscape,
                    Margins = new PdfMargins { All = 0 },
                }
            };
            var page = document.Pages.Add();
            var graphics = page.Graphics;

            //Set the font.
            PdfFont font = new PdfStandardFont(PdfFontFamily.Courier, 14, PdfFontStyle.Bold);
            //Draw the text.

            using (var imageStream = GenerateQRCodeImage(data))
            {
                var image = new PdfBitmap(imageStream);
                graphics.DrawImage(image, width / 2 - qrSize / 2, 0, qrSize, qrSize);
            }
            graphics.DrawString(
                displayString, font, 
                PdfBrushes.Black, 
                new RectangleF(0, qrSize - 5, width, 20), 
                new PdfStringFormat {Alignment = PdfTextAlignment.Center});

            //Save the document.
            var stream = new MemoryStream();
            document.Save(stream);
            document.Close();
            stream.Reset();
            return stream;
        }
    }
}