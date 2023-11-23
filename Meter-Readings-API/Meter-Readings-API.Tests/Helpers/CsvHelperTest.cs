using FluentAssertions;
using Meter_Readings_API.Helpers;
using Meter_Readings_API.Interfaces;
using Meter_Readings_API.Models;
using Microsoft.Extensions.Logging;
using Moq;
using System.Reflection;

namespace Meter_Readings_API.Tests.Helper
{
    public class CsvHelperTest
    {
        [Fact]
        public void ReadMeterReadingsCsv_WhenRead_ReturnsMeterReadings()
        {
            // Arrange
            List<MeterReading> expectedReadings = new List<MeterReading>()
            {
                new MeterReading(123, DateTime.Parse("22/11/2023 16:40"), 12345),
                new MeterReading(456, DateTime.Parse("22/11/2023 17:40"), 67890)
            };

            string csvContent = @"AccountId,MeterReadingDateTime,MeterReadValue,
                                123, 22/11/2023 16:40, 12345,
                                456, 22/11/2023 17:40, 67890,";

            // Act
            ICsvHelper<MeterReading> csvHelper = new CsvHelper<MeterReading>();
            List<MeterReading> actualReadings = csvHelper.ReadCsv(csvContent);

            // Assert
            actualReadings.Should().NotBeNull().And.BeEquivalentTo(expectedReadings);
        }

        [Fact]
        public void GetColumnMetadata_WhenUsingMeterReadingHeaderRow_ReturnsMeterReadingMetadata()
        {
            // Arange
            List<ColumnMetadata> expectedColumnMetadata = new List<ColumnMetadata>()
            {
               new ColumnMetadata(typeof(MeterReading).GetProperties().First(x => x.Name == "AccountId").SetMethod, typeof(int)),
               new ColumnMetadata(typeof(MeterReading).GetProperties().First(x => x.Name == "MeterReadingDateTime").SetMethod, typeof(DateTime)),
               new ColumnMetadata(typeof(MeterReading).GetProperties().First(x => x.Name == "MeterReadValue").SetMethod, typeof(int)),
            };

            string headerRow = "AccountId,MeterReadingDateTime,MeterReadValue,";

            // Act
            CsvHelper<MeterReading> helper = new CsvHelper<MeterReading>();

            MethodInfo method = typeof(CsvHelper<MeterReading>).GetMethod("GetColumnMetadata", BindingFlags.NonPublic | BindingFlags.Instance);
            List<ColumnMetadata> actualColumnMetadata = method.Invoke(helper, new object[] { headerRow }) as List<ColumnMetadata>;

            // Assert
            actualColumnMetadata.Should().NotBeNull().And.BeEquivalentTo(expectedColumnMetadata);
        }

        [Fact]
        public void GetColumnMetadata_WhenUsingInvalidHeaderRow_ThrowsException()
        {
            // Arrange
            string headerRow = "Account,MeterReadingDateTime,MeterReadValue,";

            // Act
            ICsvHelper<MeterReading> helper = new CsvHelper<MeterReading>();

            MethodInfo method = typeof(CsvHelper<MeterReading>).GetMethod("GetColumnMetadata", BindingFlags.NonPublic | BindingFlags.Instance);
            Action action = () => method.Invoke(helper, new object[] { headerRow });

            // Assert
            action.Should().Throw<Exception>().WithInnerException<Exception>().WithMessage("Property Account not found in object MeterReading");
        }

        [Fact]
        public void ConvertRowToObject_WhenRowHasCorrectFormat_ReturnMeterReading()
        {
            // Arrange
            MeterReading expectedMeterReading = new MeterReading(123, DateTime.Parse("22/11/2023 16:40"), 12345);

            string row = "123, 22/11/2023 16:40, 12345,";

            List<ColumnMetadata> expectedColumnMetadata = new List<ColumnMetadata>()
            {
               new ColumnMetadata(typeof(MeterReading).GetProperties().First(x => x.Name == "AccountId").SetMethod, typeof(int)),
               new ColumnMetadata(typeof(MeterReading).GetProperties().First(x => x.Name == "MeterReadingDateTime").SetMethod, typeof(DateTime)),
               new ColumnMetadata(typeof(MeterReading).GetProperties().First(x => x.Name == "MeterReadValue").SetMethod, typeof(int)),
            };

            // Act

            ICsvHelper<MeterReading> helper = new CsvHelper<MeterReading>();
            MethodInfo method = helper.GetType().GetMethod("ConvertRowToObject", BindingFlags.NonPublic | BindingFlags.Instance);

            MeterReading actualMeterReading = method.Invoke(helper, new object[] { row, expectedColumnMetadata }) as MeterReading;
            
            // Assert
            actualMeterReading.Should().NotBeNull().And.BeEquivalentTo(expectedMeterReading);
        }

        [Fact]
        public void ConvertRowToObject_WhenRowHasInvalidFormat_LogWarning()
        {
            // Arrange
            Mock<ILogger> loggerMock = new Mock<ILogger>();

            string row = "ABC, 22/12/2023 16:40, 12345,";

            List<ColumnMetadata> expectedColumnMetadata = new List<ColumnMetadata>()
            {
               new ColumnMetadata(typeof(MeterReading).GetProperties().First(x => x.Name == "AccountId").SetMethod, typeof(int)),
               new ColumnMetadata(typeof(MeterReading).GetProperties().First(x => x.Name == "MeterReadingDateTime").SetMethod, typeof(DateTime)),
               new ColumnMetadata(typeof(MeterReading).GetProperties().First(x => x.Name == "MeterReadValue").SetMethod, typeof(int)),
            };

            // Act
            ICsvHelper<MeterReading> helper = new CsvHelper<MeterReading>();
            MethodInfo method = helper.GetType().GetMethod("ConvertRowToObject", BindingFlags.NonPublic | BindingFlags.Instance);

            helper.GetType()?.GetField("logger", BindingFlags.NonPublic | BindingFlags.Instance)?.SetValue(helper, loggerMock.Object);

            MeterReading actualMeterReading = method.Invoke(helper, new object[] { row, expectedColumnMetadata }) as MeterReading;

            // Assert
            loggerMock.Verify(x => x.Log(
            It.Is<LogLevel>(l => l == LogLevel.Warning),
            It.IsAny<EventId>(),
            It.Is<It.IsAnyType>((v, t) => v.ToString() == "Unable to convert ABC to System.Int32 due to the following exception: The input string 'ABC' was not in a correct format."),
            It.IsAny<Exception>(),
            It.Is<Func<It.IsAnyType, Exception, string>>((v, t) => true)));
        }
    }
}
