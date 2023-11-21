using Meter_Readings_API.Models;
using System.Reflection;

namespace Meter_Readings_API.Helpers
{
    public static class CsvHelper<T> where T : class
    {
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
            if(columnMetadata.Count != columns.Count)
            {
                throw new Exception($"Row does not contain the same number of properties as object {typeof(T).Name}");
            }

            for(int i = 0; i < columns.Count; i++)
            {
                columnMetadata[i].SetMethod.Invoke(convertedObject, new object[] { Convert.ChangeType(columns[i], columnMetadata[i].PropertyType) });
            }

            return convertedObject;
        }
    }
}
