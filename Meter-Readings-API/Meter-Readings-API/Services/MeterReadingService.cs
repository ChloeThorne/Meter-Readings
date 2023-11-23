using FluentValidation;
using Meter_Readings_API.Data;
using Meter_Readings_API.Helpers;
using Meter_Readings_API.Interfaces;
using Meter_Readings_API.Models;
using Meter_Readings_API.Validators;
using Meter_Readings_API.ViewModels;


namespace Meter_Readings_API.Services
{
    public class MeterReadingService : IMeterReadingService
    {
        private DatabaseContext dbContext { get; set; }
        private IValidator<MeterReading> validator;
        private ICsvHelper<MeterReading> meterReadingCsvHelper;

        /// <summary>
        /// Initialises a new instance of <see cref="MeterReadingService"/>.
        /// </summary>
        /// <param name="dbContext">An instance of the <see cref="DatabaseContext"/>.</param>
        /// <param name="accountService">An instance of the <see cref="IAccountService"/>.</param>
        public MeterReadingService(DatabaseContext dbContext, IAccountService accountService) 
        { 
            this.dbContext = dbContext;
            validator = new MeterReadingValidator(this, accountService);
            meterReadingCsvHelper = new CsvHelper<MeterReading>();
        }

        /// <inheritdoc/>
        public async Task<UploadMeterReadingsViewModel> UploadFromCsv(string csvContent)
        {
            List<MeterReading> meterReadings = meterReadingCsvHelper.ReadCsv(csvContent);

            foreach(MeterReading meterReading in meterReadings)
            {
                if(validator.Validate(meterReading).IsValid)
                {
                    dbContext.Add(meterReading);
                }
            }

            int successCount = await dbContext.SaveChangesAsync();

            return new UploadMeterReadingsViewModel(successCount, meterReadings.Count - successCount);
        }

        /// <inheritdoc/>
        public async Task<bool> Create(MeterReading meterReading)
        {
            int created = 0;

            if(validator.Validate(meterReading).IsValid)
            {
                dbContext.MeterReadings.Add(meterReading);
                created = await dbContext.SaveChangesAsync();
            }
            
            return created >= 1;
        }

        /// <inheritdoc/>
        public List<MeterReading> Get()
        {
            return dbContext.MeterReadings.ToList();
        }
        
        /// <inheritdoc/>
        public MeterReading? Get(int id)
        {
            return dbContext.MeterReadings.Find(id);
        }

        /// <inheritdoc/>
        public async Task<bool> Update(MeterReading meterReading)
        {
            int updated = 0;

            if (validator.Validate(meterReading).IsValid)
            {
                dbContext.MeterReadings.Update(meterReading);
                updated = await dbContext.SaveChangesAsync();
            }

            return updated >= 1;
        }

        /// <inheritdoc/>
        public async Task<bool> Delete(int id)
        {
            MeterReading? meterReading = dbContext.MeterReadings.Find(id);
            if(meterReading == null)
            {
                return false;
            }

            dbContext.MeterReadings.Remove(meterReading);
            int deleted = await dbContext.SaveChangesAsync();
            return deleted >= 1;
        }

        /// <inheritdoc/>
        public bool DoesNotExist(MeterReading meterReading)
        {
            return !dbContext.MeterReadings.Any(x => x.AccountId == meterReading.AccountId && x.MeterReadingDateTime == meterReading.MeterReadingDateTime && x.MeterReadValue == meterReading.MeterReadValue);
        }
    }
}
