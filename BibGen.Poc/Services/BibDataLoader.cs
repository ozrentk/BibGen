using BibGen.Model;
using ClosedXML.Excel;


namespace BibGen.Services
{
    public interface IBibDataLoader
    {
        List<BibEntry> LoadBibEntries(string excelFilePath);
    }

    public class BibDataLoader : IBibDataLoader
    {
        public List<BibEntry> LoadBibEntries(string excelFilePath)
        {
            var entries = new List<BibEntry>();

            using (var workbook = new XLWorkbook(excelFilePath))
            {
                var worksheet = workbook.Worksheet(1);
                var firstRow = worksheet.FirstRowUsed();

                var excelColMap = GetExcelColumnMap(firstRow);
                var bibPropMap = BibEntry.GetPropertyMap();

                foreach (var row in worksheet.RowsUsed().Skip(1))
                {
                    var entry = new BibEntry();
                    foreach (var bibProp in bibPropMap)
                    {
                        if (excelColMap.TryGetValue(bibProp.Key, out var col))
                        {
                            var value = row.Cell(col).GetValue<string>();
                            if (string.IsNullOrEmpty(value))
                            {
                                bibProp.Value.SetValue(entry, default);
                            }
                            else
                            {
                                var convertedValue = Convert.ChangeType(value, bibProp.Value.PropertyType);
                                bibProp.Value.SetValue(entry, convertedValue);
                            }
                        }
                    }

                    entries.Add(entry);
                }
            }

            return entries;
        }

        private static Dictionary<string, int> GetExcelColumnMap(IXLRow firstRow)
        {
            var columnMap = new Dictionary<string, int>();
            foreach (var cell in firstRow.CellsUsed())
            {
                columnMap[cell.GetString()] = cell.Address.ColumnNumber;
            }

            return columnMap;
        }
    }
}
