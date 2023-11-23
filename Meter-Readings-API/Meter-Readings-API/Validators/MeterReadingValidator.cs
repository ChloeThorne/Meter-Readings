using FluentValidation;
using Meter_Readings_API.Interfaces;
using Meter_Readings_API.Models;
using Meter_Readings_API.Services;
using System.Text.RegularExpressions;

namespace Meter_Readings_API.Validators
{
    public class MeterReadingValidator : AbstractValidator<MeterReading>
    {
        /// <summary>
        /// Gets or sets the meter reading service.
        /// </summary>
        private IMeterReadingService meterReadingService { get; set; }

        /// <summary>
        /// Gets or sets the account service.
        /// </summary>
        private IAccountService accountService { get; set; }

        /// <summary>
        /// Initialises a new instance of <see cref="MeterReadingValidator"/>.
        /// </summary>
        /// <param name="meterReadingService">An instance of the <see cref="IMeterReadingService"/>.</param>
        /// <param name="accountService">An instance of the <see cref="IAccountService"/>.</param>
        public MeterReadingValidator(IMeterReadingService meterReadingService, IAccountService accountService) 
        {
            this.meterReadingService = meterReadingService;
            this.accountService = accountService;
            RuleFor(x => x).Must(DoesNotExist).WithMessage("Meter reading already exists.");
            RuleFor(x => x.AccountId).Must(AccountExists).WithMessage("Account does not exist.");
            RuleFor(x => x.MeterReadValue).Must(MatchValueFormat).WithMessage("Meter reading value does not match format NNNNN.");
        }

        /// <summary>
        /// Validation rule for checking if the provided meter reading exisits.
        /// </summary>
        /// <param name="meterReading">The meter reading to check.</param>
        /// <returns>Whether the meter reading does not exists.</returns>
        private bool DoesNotExist(MeterReading meterReading)
        {
            return meterReadingService.DoesNotExist(meterReading);
        }

        /// <summary>
        /// Validation rule for checking if the provided account exists.
        /// </summary>
        /// <param name="accountId">The account to check.</param>
        /// <returns>Whether the account exists.</returns>
        private bool AccountExists(int accountId)
        {
            return accountService.Get(accountId) != null;
        }

        /// <summary>
        /// Validation rule for if the given number matches the required format.
        /// </summary>
        /// <param name="meterReadValue">The value to check.</param>
        /// <returns>Whether the meter reading value matches the required format.</returns>
        private bool MatchValueFormat(int meterReadValue)
        {
            return Regex.IsMatch(meterReadValue.ToString(), @"\b\d{5}\b");
        }
    }
}
