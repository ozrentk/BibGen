namespace BibGen.Svc.Model
{
    public class LineDescriptor
    {
        public string FontName { get; set; }
        public float FontSize { get; set; }
        public float BaseDistance { get; set; }
    }

    public class BibGenContext
    {
        public required string ExcelFilePath { get; set; }
        public required string BackgroundFilePath { get; set; }
        public required string OutputFilePath { get; set; }
        public required List<float> MarginStripes { get; set; }
        public required List<List<LineDescriptor>> LineDescriptors { get; set; }
    }
}
