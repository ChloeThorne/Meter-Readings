namespace Meter_Readings_API.Interfaces
{
    public interface ICsvHelper<T>
    {
        /// <summary>
        /// Read a CSV file into a <see cref="List{T}"/> of the provided type.
        /// </summary>
        /// <param name="csvContent">A text representation of the CSV file.</param>
        /// <returns>A <see cref="List{T}"/> containing the objects that could be parsed from the file.</returns>
        public List<T> ReadCsv(string csvContent);
    }
}
