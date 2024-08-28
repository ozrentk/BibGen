namespace BibGen.Svc.Model
{
    public class BibGenContext
    {
        public required string ExcelFilePath { get; set; }
        public required string BackgroundFilePath { get; set; }
        public required string OutputFilePath { get; set; }
        public required List<BibLineDescriptor> LineDescriptors { get; set; }
        public int? ExportBibAt { get; set; }
    }
}
