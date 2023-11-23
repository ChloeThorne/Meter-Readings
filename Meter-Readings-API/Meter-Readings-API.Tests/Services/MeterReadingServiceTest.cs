using FluentAssertions;
using FluentValidation;
using Meter_Readings_API.Data;
using Meter_Readings_API.Interfaces;
using Meter_Readings_API.Models;
using Meter_Readings_API.Services;
using Meter_Readings_API.ViewModels;
using Moq;
using System.ComponentModel.DataAnnotations;
using System.Reflection;

namespace Meter_Readings_API.Tests.Services
{
    public class MeterReadingServiceTest
    {
        [Fact]
        public async Task UploadFromCsv_WithValidContent_ReturnsViewModel()
        { 
            // Arrange
            Mock<DatabaseContext> mockDbContext = new Mock<DatabaseContext>();
            mockDbContext.Setup(x => x.SaveChangesAsync(It.IsAny<CancellationToken>())).ReturnsAsync(1);

            Mock<IAccountService> mockAccountService = new Mock<IAccountService>();

            MeterReadingService meterReadingService = new MeterReadingService(mockDbContext.Object, mockAccountService.Object);

            // Setup the validator to always return valid.
            Mock<IValidator<MeterReading>> mockValidator = new Mock<IValidator<MeterReading>>();

            // An empty list of errors will make IsValid true.
            FluentValidation.Results.ValidationResult result = new FluentValidation.Results.ValidationResult()
            {
                Errors = new List<FluentValidation.Results.ValidationFailure>()
            };
            
            mockValidator.Setup(x => x.Validate(It.IsAny<MeterReading>())).Returns(result);
            meterReadingService.GetType()?.GetField("validator", BindingFlags.NonPublic | BindingFlags.Instance)?.SetValue(meterReadingService, mockValidator.Object);

            // Setup the meter reading CsvHelper to return a pre-defined object.
            Mock<ICsvHelper<MeterReading>> mockCsvHelper = new Mock<ICsvHelper<MeterReading>>();
            List<MeterReading> meterReadings = new List<MeterReading>()
            {
                new MeterReading(1, DateTime.Now, 12345)
            };

            mockCsvHelper.Setup(x => x.ReadCsv(It.IsAny<string>())).Returns(meterReadings);

            meterReadingService.GetType()?.GetField("meterReadingCsvHelper", BindingFlags.NonPublic | BindingFlags.Instance)?.SetValue(meterReadingService, mockCsvHelper.Object);

            // Act
            UploadMeterReadingsViewModel vm = await meterReadingService.UploadFromCsv("testCsv");

            // Assert
            mockDbContext.Verify(m => m.Add(It.IsAny<MeterReading>()), Times.Once);

            vm.SuccessfulReadingsCount.Should().Be(1);
            vm.FailedReadingsCount.Should().Be(0);
        }

        [Fact]
        public async Task UploadFromCsv_WithInvalidContent_ReturnsViewModel()
        {
            // Arrange
            Mock<DatabaseContext> mockDbContext = new Mock<DatabaseContext>();
            mockDbContext.Setup(x => x.SaveChangesAsync(It.IsAny<CancellationToken>())).ReturnsAsync(0);

            Mock<IAccountService> mockAccountService = new Mock<IAccountService>();

            MeterReadingService meterReadingService = new MeterReadingService(mockDbContext.Object, mockAccountService.Object);

            // Setup the validator to always return valid.
            Mock<IValidator<MeterReading>> mockValidator = new Mock<IValidator<MeterReading>>();

            // A list of errors will make IsValid false.
            FluentValidation.Results.ValidationResult result = new FluentValidation.Results.ValidationResult()
            {
                Errors = new List<FluentValidation.Results.ValidationFailure>()
                {
                    new FluentValidation.Results.ValidationFailure("PropertyName", "Failed validation")
                }
            };

            mockValidator.Setup(x => x.Validate(It.IsAny<MeterReading>())).Returns(result);
            meterReadingService.GetType()?.GetField("validator", BindingFlags.NonPublic | BindingFlags.Instance)?.SetValue(meterReadingService, mockValidator.Object);

            // Setup the meter reading csv helper to return a pre-defined object.
            Mock<ICsvHelper<MeterReading>> mockCsvHelper = new Mock<ICsvHelper<MeterReading>>();
            List<MeterReading> meterReadings = new List<MeterReading>()
            {
                new MeterReading(1, DateTime.Now, 12345)
            };

            mockCsvHelper.Setup(x => x.ReadCsv(It.IsAny<string>())).Returns(meterReadings);

            meterReadingService.GetType()?.GetField("meterReadingCsvHelper", BindingFlags.NonPublic | BindingFlags.Instance)?.SetValue(meterReadingService, mockCsvHelper.Object);

            // Act
            UploadMeterReadingsViewModel vm = await meterReadingService.UploadFromCsv("testCsv");
            
            // Assert
            mockDbContext.Verify(m => m.Add(It.IsAny<MeterReading>()), Times.Never);

            vm.SuccessfulReadingsCount.Should().Be(0);
            vm.FailedReadingsCount.Should().Be(1);
        }
    }
}
