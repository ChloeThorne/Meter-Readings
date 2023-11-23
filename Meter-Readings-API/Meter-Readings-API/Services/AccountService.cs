using Meter_Readings_API.Data;
using Meter_Readings_API.Interfaces;
using Meter_Readings_API.Models;
using Microsoft.EntityFrameworkCore;

namespace Meter_Readings_API.Services
{
    public class AccountService : IAccountService
    {
        private DatabaseContext dbContext { get; set; }
        public AccountService(DatabaseContext dbContext)
        {
            this.dbContext = dbContext;
        }

        /// <inheritdoc/>
        public async Task<bool> Create(Account account)
        {
            dbContext.Accounts.Add(account);
            int created = await dbContext.SaveChangesAsync();
            return created >= 1;
        }

        /// <inheritdoc/>
        public List<Account> Get()
        {
            return dbContext.Accounts.ToList();
        }

        /// <inheritdoc/>
        public Account? Get(int id)
        {
            return dbContext.Accounts.Find(id);
        }

        /// <inheritdoc/>
        public async Task<bool> Update(Account account)
        {
            dbContext.Accounts.Update(account);
            int updated = await dbContext.SaveChangesAsync();
            return updated >= 1;
        }

        /// <inheritdoc/>
        public async Task<bool> Delete(int id)
        {
            Account? account = dbContext.Accounts.Find(id);
            if (account == null)
            {
                return false;
            }

            dbContext.Accounts.Remove(account);
            int deleted = await dbContext.SaveChangesAsync();
            return deleted >= 1;

        }
    }
}
