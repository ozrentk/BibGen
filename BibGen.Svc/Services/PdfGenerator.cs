using PdfSharpCore.Drawing;
using PdfSharpCore.Pdf;
using SkiaSharp;

namespace BibGen.Services
{
    public interface IPdfGenerator
    {
        void GeneratePdf(List<SKBitmap> images, string outputFilePath);
    }

    public class PdfGenerator : IPdfGenerator
    {
        public void GeneratePdf(List<SKBitmap> images, string outputFilePath)
        {
            var document = new PdfDocument();

            foreach (var image in images)
            {
                var page = document.AddPage();
                page.Width = "210mm";
                page.Height = "148mm";
                //page.Width = image.Width;
                //page.Height = image.Height;
                var gfx = XGraphics.FromPdfPage(page);
                using (var ms = new MemoryStream())
                {
                    image.Encode(ms, SKEncodedImageFormat.Png, 100);
                    ms.Position = 0;
                    var xImage = XImage.FromStream(() => ms);
                    gfx.DrawImage(xImage, 0, 0, page.Width - 1, page.Height - 1);
                }
            }

            document.Save(outputFilePath);
        }
    }
}
