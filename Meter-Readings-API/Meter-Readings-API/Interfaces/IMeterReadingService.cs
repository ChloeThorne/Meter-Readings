using Meter_Readings_API.Models;
using Meter_Readings_API.ViewModels;

namespace Meter_Readings_API.Interfaces
{
    public interface IMeterReadingService
    {
        /// <summary>
        /// Upload a CSV file to the database.
        /// </summary>
        /// <param name="csvContent">The contents of the CSV file to upload.</param>
        /// <returns>A <see cref="UploadMeterReadingsViewModel"/> containing how many files were successfully uploaded or failed.</returns>
        public Task<UploadMeterReadingsViewModel> UploadFromCsv(string csvContent);

        /// <summary>
        /// Creates a new meter reading entry in the database.
        /// </summary>
        /// <param name="meterReading">The meter reading to add to the database.</param>
        /// <returns>An awaitable Task containing if the creation was a success.</returns>
        public Task<bool> Create(MeterReading meterReading);

        /// <summary>
        /// Get all meter reading entries from the database.
        /// </summary>
        /// <returns>A <see cref="List{MeterReading}"/> containing all meter readings in the database.</returns>
        public List<MeterReading> Get();

        /// <summary>
        /// Gets a single meter reading entry from the database.
        /// </summary>
        /// <param name="id">The ID of the meter reading.</param>
        /// <returns>An instance of <see cref="MeterReading"/> representing the database entry.</returns>
        public MeterReading? Get(int id);

        /// <summary>
        /// Update a meter reading entry in the database.
        /// </summary>
        /// <param name="meterReading">The new meter reading entry.</param>
        /// <returns>An awaitable Task containing if the update was a success.</returns>
        public Task<bool> Update(MeterReading meterReading);

        /// <summary>
        /// Delete an entry by ID in the database.
        /// </summary>
        /// <param name="id">The ID of the entity to delete.</param>
        /// <returns>An awaitable Task containing if the delete was a success.</returns>
        public Task<bool> Delete(int id);

        /// <summary>
        /// Checks if the provided meter reading already exists in the database.
        /// </summary>
        /// <param name="meterReading">The meter reading to check.</param>
        /// <returns>Whether the entity is present in the database.</returns>
        public bool DoesNotExist(MeterReading meterReading);
    }
}
