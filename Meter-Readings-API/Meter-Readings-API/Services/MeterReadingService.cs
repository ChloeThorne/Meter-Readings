using Meter_Readings_API.Data;
using Meter_Readings_API.Helpers;
using Meter_Readings_API.Interfaces;
using Meter_Readings_API.Models;
using Meter_Readings_API.Validators;


namespace Meter_Readings_API.Services
{
    public class MeterReadingService : IMeterReadingService
    {
        private DatabaseContext _dbContext { get; set; }
        private MeterReadingValidator validator { get; set; }
        public MeterReadingService(DatabaseContext dbContext, IAccountService accountService) 
        { 
            _dbContext = dbContext;
            validator = new MeterReadingValidator(this, accountService);
        }

        public async Task<UploadMeterReadingsViewModel> UploadFromCsv(string csvContent)
        {
            List<MeterReading> meterReadings = CsvHelper<MeterReading>.ReadCsv(csvContent);

            foreach(MeterReading meterReading in meterReadings)
            {
                if(validator.Validate(meterReading).IsValid)
                {
                    _dbContext.Add(meterReading);
                }
            }

            int successCount = await _dbContext.SaveChangesAsync();

            return new UploadMeterReadingsViewModel(successCount, meterReadings.Count - successCount);
        }
        public async Task<bool> Create(MeterReading meterReading)
        {
            int created = 0;

            if(validator.Validate(meterReading).IsValid)
            {
                _dbContext.MeterReadings.Add(meterReading);
                created = await _dbContext.SaveChangesAsync();
            }
            
            return created >= 1;
        }

        public List<MeterReading> Get()
        {
            return _dbContext.MeterReadings.ToList();
        }
        public MeterReading? Get(int id)
        {
            return _dbContext.MeterReadings.Find(id);
        }

        public async Task<bool> Update(MeterReading meterReading)
        {
            int updated = 0;

            if (validator.Validate(meterReading).IsValid)
            {
                _dbContext.MeterReadings.Update(meterReading);
                updated = await _dbContext.SaveChangesAsync();
            }

            return updated >= 1;
        }

        public async Task<bool> Delete(int id)
        {
            MeterReading? meterReading = _dbContext.MeterReadings.Find(id);
            if(meterReading == null)
            {
                return false;
            }

            _dbContext.MeterReadings.Remove(meterReading);
            int deleted = await _dbContext.SaveChangesAsync();
            return deleted >= 1;
        }

        public bool DoesNotExist(MeterReading meterReading)
        {
            return !_dbContext.MeterReadings.Any(x => x.AccountId == meterReading.AccountId && x.MeterReadingDateTime == meterReading.MeterReadingDateTime && x.MeterReadValue == meterReading.MeterReadValue);
        }
    }
}
