using BibGen.App.Viewmodel;
using BibGen.Services;
using BibGen.Svc.Model;
using System.Windows;
using System.Windows.Input;

namespace BibGen.App.Commands.File
{
    public class ExportBibBaseCommand
    {
        private readonly BibPipeline Pipeline;

        public ExportBibBaseCommand()
        {
            Pipeline = new BibPipeline(new BibDataLoader(), new BibImageGenerator(), new PdfGenerator());
        }

        protected void StartExportPipeline(
            string excelFilePath,
            string backgroundFilePath,
            string outputFilePath,
            List<BibLineDescriptor> lines,
            int? exportBibAt = null)
        {
            Pipeline.GenerateBibs(new BibGenContext
            {
                ExcelFilePath = excelFilePath,
                BackgroundFilePath = backgroundFilePath,
                OutputFilePath = outputFilePath,
                LineDescriptors = lines,
                ExportBibAt = exportBibAt
            });

            MessageBox.Show(
                "Export completed",
                "Export",
                MessageBoxButton.OK,
                MessageBoxImage.Information);
        }

    }

}
