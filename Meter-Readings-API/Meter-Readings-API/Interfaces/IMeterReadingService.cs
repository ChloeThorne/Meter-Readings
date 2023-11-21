using Meter_Readings_API.Models;

namespace Meter_Readings_API.Interfaces
{
    public interface IMeterReadingService
    {
        public Task<UploadMeterReadingsViewModel> UploadFromCsv(string csvContent);
        public Task<bool> Create(MeterReading meterReading);
        public List<MeterReading> Get();
        public MeterReading? Get(int id);
        public Task<bool> Update(MeterReading meterReading);
        public Task<bool> Delete(int id);
        public bool DoesNotExist(MeterReading meterReading);
    }
}
