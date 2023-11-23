namespace Meter_Readings_API.ViewModels
{
    public class UploadMeterReadingsViewModel
    {
        public int SuccessfulReadingsCount { get; set; }
        public int FailedReadingsCount { get; set; }
        public UploadMeterReadingsViewModel() { }
        public UploadMeterReadingsViewModel(int successfulReadingsCount, int failedReadingsCount)
        {
            SuccessfulReadingsCount = successfulReadingsCount;
            FailedReadingsCount = failedReadingsCount;
        }
    }
}
