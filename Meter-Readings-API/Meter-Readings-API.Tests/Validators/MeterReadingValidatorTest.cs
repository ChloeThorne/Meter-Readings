using FluentAssertions;
using FluentValidation.Results;
using Meter_Readings_API.Interfaces;
using Meter_Readings_API.Models;
using Meter_Readings_API.Validators;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Meter_Readings_API.Tests.Validators
{
    public class MeterReadingValidatorTest
    {
        [Fact]
        public void MeterReadingValidator_Validate_WhenMeterReadingAlreadyExists_ReturnNotValid()
        {
            // Arrange
            Mock<IMeterReadingService> mockMeterReadingService = new Mock<IMeterReadingService>();
            Mock<IAccountService> mockAccountService = new Mock<IAccountService>();

            mockMeterReadingService.Setup(x => x.DoesNotExist(It.IsAny<MeterReading>())).Returns(false);
            mockAccountService.Setup(x => x.Get(It.IsAny<int>())).Returns(new Account());

            MeterReading meterReading = new MeterReading(123, DateTime.Now, 12345);

            // Act
            MeterReadingValidator meterReadingValidator = new MeterReadingValidator(mockMeterReadingService.Object, mockAccountService.Object);
            ValidationResult validationResult = meterReadingValidator.Validate(meterReading);

            // Assert
            validationResult.IsValid.Should().BeFalse();
            validationResult.Errors.Should().Contain(x => x.ErrorMessage == "Meter reading already exists.").And.HaveCount(1);
        }

        [Fact]
        public void MeterReadingValidator_Validate_WhenAccountDoesNotExist_ReturnNotValid()
        {
            // Arrange
            Mock<IMeterReadingService> mockMeterReadingService = new Mock<IMeterReadingService>();
            Mock<IAccountService> mockAccountService = new Mock<IAccountService>();

            mockMeterReadingService.Setup(x => x.DoesNotExist(It.IsAny<MeterReading>())).Returns(true);

            MeterReading meterReading = new MeterReading(123, DateTime.Now, 12345);

            // Act
            MeterReadingValidator meterReadingValidator = new MeterReadingValidator(mockMeterReadingService.Object, mockAccountService.Object);
            ValidationResult validationResult = meterReadingValidator.Validate(meterReading);

            // Assert
            validationResult.IsValid.Should().BeFalse();
            validationResult.Errors.Should().Contain(x => x.ErrorMessage == "Account does not exist.").And.HaveCount(1);
        }

        [Fact]
        public void MeterReadingValidator_Validate_WhenMeterReadingValueIsInvalid_ReturnNotValid()
        {
            // Arrange
            Mock<IMeterReadingService> mockMeterReadingService = new Mock<IMeterReadingService>();
            Mock<IAccountService> mockAccountService = new Mock<IAccountService>();

            mockMeterReadingService.Setup(x => x.DoesNotExist(It.IsAny<MeterReading>())).Returns(true);
            mockAccountService.Setup(x => x.Get(It.IsAny<int>())).Returns(new Account());

            MeterReading meterReading = new MeterReading(123, DateTime.Now, 123);

            // Act
            MeterReadingValidator meterReadingValidator = new MeterReadingValidator(mockMeterReadingService.Object, mockAccountService.Object);
            ValidationResult validationResult = meterReadingValidator.Validate(meterReading);

            // Assert
            validationResult.IsValid.Should().BeFalse();
            validationResult.Errors.Should().Contain(x => x.ErrorMessage == "Meter reading value does not match format NNNNN.").And.HaveCount(1);
        }

        [Fact]
        public void MeterReadingValidator_Validate_WhenMeterReadingValid_ReturnValid()
        {
            // Arrange
            Mock<IMeterReadingService> mockMeterReadingService = new Mock<IMeterReadingService>();
            Mock<IAccountService> mockAccountService = new Mock<IAccountService>();

            mockMeterReadingService.Setup(x => x.DoesNotExist(It.IsAny<MeterReading>())).Returns(true);
            mockAccountService.Setup(x => x.Get(It.IsAny<int>())).Returns(new Account());

            MeterReading meterReading = new MeterReading(123, DateTime.Now, 12345);

            // Act
            MeterReadingValidator meterReadingValidator = new MeterReadingValidator(mockMeterReadingService.Object, mockAccountService.Object);
            ValidationResult validationResult = meterReadingValidator.Validate(meterReading);

            // Assert
            validationResult.IsValid.Should().BeTrue();
        }
    }
}
