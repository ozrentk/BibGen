using BibGen.Svc.Model;
using ClosedXML.Excel;

namespace BibGen.Services
{
    public interface IBibDataLoader
    {
        List<string> GetBibPropertyNames(string excelFilePath);
        List<BibEntry> LoadBibEntries(string excelFilePath);
    }

    public class BibDataLoader : IBibDataLoader
    {
        public List<string> GetBibPropertyNames(string excelFilePath)
        {
            using (var workbook = new XLWorkbook(excelFilePath))
            {
                var worksheet = workbook.Worksheet(1);
                var firstRow = worksheet.FirstRowUsed();

                var propertyNames = new List<string>();
                foreach (var cell in firstRow.CellsUsed())
                {
                    propertyNames.Add(cell.GetString());
                }

                return propertyNames;
            }
        }

        public List<BibEntry> LoadBibEntries(string excelFilePath)
        {
            var propertyNames = GetBibPropertyNames(excelFilePath);

            var entries = new List<BibEntry>();

            using (var workbook = new XLWorkbook(excelFilePath))
            {
                var worksheet = workbook.Worksheets.First();
                var headerRow = worksheet.FirstRowUsed();

                // BEGIN: remove
                var excelColMap = GetExcelColumnMap(headerRow);
                var bibPropMap = BibEntry.GetPropertyMap();
                // END: remove

                foreach (var row in worksheet.RowsUsed().Skip(1))
                {
                    var entry = new BibEntry();

                    foreach (var cell in row.CellsUsed())
                    {
                        var columnName = headerRow.Cell(cell.Address.ColumnNumber).GetValue<string>();
                        var cellValue = cell.GetValue<string>();
                        entry.DataItems[columnName] = cellValue;
                    }

                    // BEGIN: remove
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
                    // END: remove

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
