using DocumentFormat.OpenXml.Presentation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BibGen.Model
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
