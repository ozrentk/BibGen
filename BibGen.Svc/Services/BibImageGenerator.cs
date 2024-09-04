using BibGen.Svc.Model;
using SkiaSharp;

namespace BibGen.Services
{
    public interface IBibImageGenerator
    {
        SKBitmap GenerateBibImage(
            BibEntry entry,
            SKBitmap background,
            List<BibLineDescriptor> lineDescriptors);
    }

    public class BibImageGenerator : IBibImageGenerator
    {
        //private float GetCoverage(BibGenContext context) =>
        //    // margins: top, right, bottom, left
        //    1 - (context.MarginStripes[0] + context.MarginStripes[2]);

        public SKBitmap GenerateBibImage(
            BibEntry bibEntry,
            SKBitmap background,
            List<BibLineDescriptor> lineDescriptors)
        {
            var bgCopy = background.Copy();
            var canvas = new SKCanvas(bgCopy);

            for (int i = 0; i < lineDescriptors.Count; i++)
            {
                var lineDescriptor = lineDescriptors[i];

                bibEntry.DataItems.TryGetValue(lineDescriptor.ExcelColumnName, out string lineText);

                //var typeface = SKTypeface.FromFile(lineDescriptor.FontName);
                var typeface = SKTypeface.FromFamilyName(lineDescriptor.FontName);

                var linePaint = MakeSkPaint(
                    lineText,
                    typeface,
                    lineDescriptor.FontSize,
                    lineDescriptor.Color,
                    out float probeWidthPx,
                    out float probeHeightPx);

                var ratio = probeWidthPx / (bgCopy.Width * 0.9F);
                if (ratio > 1)
                {
                    // Preširoko - suziti
                    linePaint = MakeSkPaint(
                        lineText,
                        typeface,
                        lineDescriptor.FontSize / ratio,
                        lineDescriptor.Color,
                        out probeWidthPx,
                        out probeHeightPx);
                }

                // Pozicija i crtanje broja
                var x = bgCopy.Width / 2;
                var y = bgCopy.Height * lineDescriptor.Baseline;
                canvas.DrawText(
                    lineText,
                    x,
                    y,
                    linePaint);
            }

            return bgCopy;

            /*
            var coverage = GetCoverage(context);
            var wishedHeightPx = bgCopy.Height * coverage;
            var wishedWidthPx = bgCopy.Width * coverage;

            // Probe
            var canvas = new SKCanvas(bgCopy);
            var probePaint = MakeSkPaint(
                entry.Number.ToString(),
                typeface,
                wishedHeightPx,
                out float probeWidthPx,
                out float probeHeightPx);

            var widthRatio = wishedWidthPx / probeWidthPx;
            var heightRatio = wishedHeightPx / probeHeightPx;

            float imageWidthPx;
            float imageHeightPx;
            SKPaint paint;
            if (widthRatio > heightRatio)
            {
                var tunedHeightPx = wishedHeightPx * heightRatio;

                Console.WriteLine($"Tuned height (h): {tunedHeightPx}");

                paint = MakeSkPaint(
                    entry.Number.ToString(),
                    typeface,
                    tunedHeightPx,
                    out imageWidthPx,
                    out imageHeightPx);
            }
            else 
            {
                var tunedHeightPx = wishedHeightPx * widthRatio;

                Console.WriteLine($"Tuned height (w): {tunedHeightPx}");

                paint = MakeSkPaint(
                    entry.Number.ToString(),
                    typeface,
                    tunedHeightPx,
                    out imageWidthPx,
                    out imageHeightPx);
            }


            // Pozicija i crtanje broja
            var x = bgCopy.Width / 2;
            var y = (bgCopy.Height - imageHeightPx) / 2;
            canvas.DrawText(entry.Number.ToString(), x, bgCopy.Height * (context.MarginStripes[0] + coverage), paint);

            // Ako postoje ime i prezime, crtanje ispod broja
            //if (!string.IsNullOrWhiteSpace(entry.FirstName) && 
            //    !string.IsNullOrWhiteSpace(entry.LastName))
            //{
            //    y += 70;
            //    canvas.DrawText(entry.FullName, x, y, paint);
            //}
            */
        }

        private SKPaint MakeSkPaint(
            string text,
            SKTypeface typeface,
            float wishedHeightPx,
            string colorHex,
            out float probeWidthPx,
            out float probeHeightPx)
        {
            var paint = new SKPaint
            {
                Typeface = typeface,
                TextSize = wishedHeightPx,
                IsAntialias = true,
                Color = SKColor.Parse(colorHex),
                TextAlign = SKTextAlign.Center
            };

            //SKFontMetrics metrics;
            //paint.GetFontMetrics(out metrics);
            //float actualTextHeight = metrics.Descent - metrics.Ascent + metrics.Leading;

            var textBounds = new SKRect();
            paint.MeasureText(text, ref textBounds);
            probeWidthPx = textBounds.Width;
            probeHeightPx = textBounds.Height;

            return paint;
        }
    }
}
