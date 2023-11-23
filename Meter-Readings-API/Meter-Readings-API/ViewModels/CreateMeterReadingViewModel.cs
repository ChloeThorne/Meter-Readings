namespace Meter_Readings_API.ViewModels
{
    public class CreateMeterReadingViewModel
    {
        public int AccountId { get; set; }
        public DateTime MeterReadingDateTime { get; set; }
        public int MeterReadValue { get; set; }
        public CreateMeterReadingViewModel() { }
        public CreateMeterReadingViewModel(int accountId, DateTime meterReadingDateTime, int meterReadValue)
        {
            AccountId = accountId;
            MeterReadingDateTime = meterReadingDateTime;
            MeterReadValue = meterReadValue;
        }
    }
}
