using Meter_Readings_API.Interfaces;
using Meter_Readings_API.Models;
using System.Reflection;

namespace Meter_Readings_API.Helpers
{
    /// <summary>
    /// Helper functions for reading a CSV file into an object
    /// </summary>
    /// <typeparam name="T">The type of object to convert the CSV file into.</typeparam>
    public class CsvHelper<T> : ICsvHelper<T> where T : class
    {
        /// <summary>
        /// Initialises a new instance of <see cref="CsvHelper{T}"/>
        /// </summary>
        public CsvHelper() { }

        /// <summary>
        /// The logger to use.
        /// </summary>
        private ILogger logger = LoggerFactory.Create(builder => builder.AddConsole()).CreateLogger("CsvHelper");

        /// <inheritdoc />
        public List<T> ReadCsv(string csvContent)
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
                if(string.IsNullOrEmpty(row)) continue;

                T convertedObject = ConvertRowToObject(row, columnMetadata);
                objects.Add(convertedObject);
            }

            return objects;
        }

        /// <summary>
        /// Creates a list of <see cref="ColumnMetadata"/> from a CSV header row.
        /// </summary>
        /// <param name="headerRow">The header row of the CSV file.</param>
        /// <returns>A list of <see cref="ColumnMetadata"/></returns>
        /// <exception cref="Exception">Thrown when a header in the CSV file does not map to the object.</exception>
        private List<ColumnMetadata> GetColumnMetadata(string headerRow)
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

        /// <summary>
        /// Converts a CSV line to the provided object.
        /// </summary>
        /// <param name="row">The CSV line.</param>
        /// <param name="columnMetadata">The metadata for the columns.</param>
        /// <returns>A new object of type <see cref="T"/>.</returns>
        private T ConvertRowToObject(string row, List<ColumnMetadata> columnMetadata)
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
