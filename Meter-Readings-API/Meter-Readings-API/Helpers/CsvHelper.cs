using Meter_Readings_API.Models;
using System.Globalization;
using System.Reflection;

namespace Meter_Readings_API.Helpers
{
    public static class CsvHelper<T> where T : class
    {
        private static ILogger logger = LoggerFactory.Create(builder => builder.AddConsole()).CreateLogger("CsvHelper");
        public static List<T> ReadCsv(string csvContent)
        {
            List<T> objects = new List<T>();
            List<string> csvContentRows = csvContent.Split(Environment.NewLine).ToList();

            // Get Headers
            string headerRow = csvContentRows.First();
            csvContentRows.RemoveAt(0);

            // Get Column Metadata
            List<ColumnMetadata> columnMetadata = GetColumnMetadata(headerRow);

            // Read row and convert it to object T
            foreach (string row in csvContentRows)
            {
                if(String.IsNullOrEmpty(row)) continue;

                T convertedObject = ConvertRowToObject(row, columnMetadata);
                objects.Add(convertedObject);
            }

            return objects;
        }
        private static List<ColumnMetadata> GetColumnMetadata(string headerRow)
        {
            List<ColumnMetadata> columnMetadatas = new List<ColumnMetadata>();
            PropertyInfo[] properties = typeof(T).GetProperties();
            List<string> columnHeaders = headerRow.Split(',').ToList();

            foreach (string columnHeader in columnHeaders)
            {
                if(string.IsNullOrEmpty(columnHeader)) continue;

                PropertyInfo? property = properties.FirstOrDefault(x => x.Name.Equals(columnHeader, StringComparison.InvariantCultureIgnoreCase));
                if (property == null || property.SetMethod == null)
                {
                    throw new Exception($"Property {columnHeader} not found in object {typeof(T).Name}");
                }

                columnMetadatas.Add(new ColumnMetadata(property.SetMethod, property.PropertyType));
            }

            return columnMetadatas;
        }

        private static T ConvertRowToObject(string row, List<ColumnMetadata> columnMetadata)
        {
            T convertedObject = Activator.CreateInstance<T>();

            List<string> columns = row.Split(',').ToList();

            for(int i = 0; i < Math.Min(columnMetadata.Count, columns.Count); i++)
            {
                try
                {
                    string dateTime = columns[i];
                    var convertedColumn = Convert.ChangeType(dateTime, columnMetadata[i].PropertyType);
                    columnMetadata[i].SetMethod.Invoke(convertedObject, new object[] { convertedColumn });
                }
                catch(FormatException ex)
                {
                    logger.LogWarning($"Unable to convert {columns[i]} to {columnMetadata[i].PropertyType} due to the following exception: " + ex.Message);
                }
            }

            return convertedObject;
        }
    }
}
