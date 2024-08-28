using BibGen.Svc.Model;
using DocumentFormat.OpenXml.Drawing.Charts;
using SkiaSharp;

namespace BibGen.Services
{
    public interface IBibPipeline
    {
        void GenerateBibs(BibGenContext context);
    }

    public class BibPipeline : IBibPipeline
    {
        private readonly IBibDataLoader _dataLoader;
        private readonly IBibImageGenerator _imageGenerator;
        private readonly IPdfGenerator _pdfGenerator;

        public BibPipeline(IBibDataLoader dataLoader, IBibImageGenerator imageGenerator, IPdfGenerator pdfGenerator)
        {
            _dataLoader = dataLoader;
            _imageGenerator = imageGenerator;
            _pdfGenerator = pdfGenerator;
        }

        public void GenerateBibs(BibGenContext context)
        {
            var entries = _dataLoader.LoadBibEntries(context.ExcelFilePath);
            var background = SKBitmap.Decode(context.BackgroundFilePath);

            Directory.CreateDirectory(context.OutputFilePath);

            int count = 0;
            var numberedEntries = entries.Where(e => e.Number != 0).ToList();
            int total = numberedEntries.Count;
            foreach (var entry in entries)
            {
                if(entry.Number == 0)
                    continue;

                var image = _imageGenerator.GenerateBibImage(entry, background, context);
                var images = new List<SKBitmap> { image };

                var filePath = Path.Combine(context.OutputFilePath, $"bib-{entry}.pdf");
                _pdfGenerator.GeneratePdf(images, filePath);

                count++;
                Console.WriteLine($"Entry {count} of {total} processed.");
            }
        }
    }
}