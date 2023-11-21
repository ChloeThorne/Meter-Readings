using FluentValidation;
using Meter_Readings_API.Interfaces;
using Meter_Readings_API.Models;
using Meter_Readings_API.Services;
using System.Text.RegularExpressions;

namespace Meter_Readings_API.Validators
{
    public class MeterReadingValidator : AbstractValidator<MeterReading>
    {
        private IMeterReadingService _meterReadingService { get; set; }
        private IAccountService _accountService { get; set; }
        public MeterReadingValidator(IMeterReadingService meterReadingService, IAccountService accountService) 
        {
            _meterReadingService = meterReadingService;
            _accountService = accountService;
            RuleFor(x => x).Must(DoesNotExist);
            RuleFor(x => x.AccountId).Must(AccountExists);
            RuleFor(x => x.MeterReadValue).Must(MatchValueFormat);
        }
        private bool DoesNotExist(MeterReading meterReading)
        {
            return _meterReadingService.DoesNotExist(meterReading);
        }
        private bool AccountExists(int accountId)
        {
            return _accountService.Get(accountId) != null;
        }

        private bool MatchValueFormat(int meterReadValue)
        {
            return Regex.IsMatch(meterReadValue.ToString(), @"\b\d{5}\b");
        }
    }
}
