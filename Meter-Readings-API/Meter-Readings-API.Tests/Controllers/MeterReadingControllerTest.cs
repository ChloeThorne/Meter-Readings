using FluentAssertions;
using Meter_Readings_API.Controllers;
using Meter_Readings_API.Interfaces;
using Meter_Readings_API.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Newtonsoft.Json.Linq;
using System.Collections.ObjectModel;
using System.Text;

namespace Meter_Readings_API.Tests.Controllers
{
    public class MeterReadingControllerTest
    {
        [Fact]
        public async void MeterReadingUploads_WhereCsvIsValid_ReturnsOkWithSuccessFailureCount()
        {
            // Arrange
            Mock<IMeterReadingService> mockMeterReadingService = new Mock<IMeterReadingService>();
            UploadMeterReadingsViewModel expectedViewModel = new UploadMeterReadingsViewModel()
            {
                SuccessfulReadingsCount = 4,
                FailedReadingsCount = 31
            };

            mockMeterReadingService.Setup(x => x.UploadFromCsv(It.IsAny<string>())).ReturnsAsync(expectedViewModel);

            // Act
            MeterReadingController meterReadingController = new MeterReadingController(mockMeterReadingService.Object);
            Stream fileFormStream = new MemoryStream(Encoding.UTF8.GetBytes("testCsv"));
            IActionResult result = await meterReadingController.MeterReadingUploads(new FormFile(fileFormStream, 0, fileFormStream.Length, "testCsv", "testCsv.csv"));

            // Assert
            result.Should().BeOfType<OkObjectResult>();
            UploadMeterReadingsViewModel actualViewModel = ((OkObjectResult)result).Value as UploadMeterReadingsViewModel;
            actualViewModel.Should().NotBeNull().And.Be(expectedViewModel);
        }

        [Fact]
        public async void MeterReadingUploads_WhenCalled_ExtractsCsvContentFromFormFile()
        {
            // Arrange
            Mock<IMeterReadingService> mockMeterReadingService = new Mock<IMeterReadingService>();

            ICollection<string> uploadFromCsvCalls = new Collection<string>();

            mockMeterReadingService.Setup(x => x.UploadFromCsv(Capture.In(uploadFromCsvCalls)));

            // Act
            MeterReadingController meterReadingController = new MeterReadingController(mockMeterReadingService.Object);
            Stream fileFormStream = new MemoryStream(Encoding.UTF8.GetBytes("testCsv"));
            IActionResult result = await meterReadingController.MeterReadingUploads(new FormFile(fileFormStream, 0, fileFormStream.Length, "testCsv", "testCsv.csv"));

            // Assert
            uploadFromCsvCalls.First().Should().Be("testCsv\r\n");
        }

        [Fact]
        public async void MeterReadingUploads_WhereExceptionIsThrown_ReturnsInternalServerErrorWithMessage()
        {
            // Arrange
            Mock<IMeterReadingService> mockMeterReadingService = new Mock<IMeterReadingService>();

            Exception exception = new Exception("Failure");

            mockMeterReadingService.Setup(x => x.UploadFromCsv(It.IsAny<string>())).ThrowsAsync(exception);

            // Act
            MeterReadingController meterReadingController = new MeterReadingController(mockMeterReadingService.Object);
            Stream fileFormStream = new MemoryStream(Encoding.UTF8.GetBytes("testCsv"));
            IActionResult result = await meterReadingController.MeterReadingUploads(new FormFile(fileFormStream, 0, fileFormStream.Length, "testCsv", "testCsv.csv"));

            // Assert
            result.Should().BeOfType<ObjectResult>();
            ObjectResult objectResult = (ObjectResult)result;
            objectResult.StatusCode.Should().Be(StatusCodes.Status500InternalServerError);
            objectResult.Value.Should().Be("Failure");
        }
    }
}