using Meter_Readings_API.Data;
using Meter_Readings_API.Interfaces;
using Meter_Readings_API.Models;
using Microsoft.EntityFrameworkCore;

namespace Meter_Readings_API.Services
{
    public class AccountService : IAccountService
    {
        private DatabaseContext _dbContext { get; set; }
        public AccountService(DatabaseContext dbContext)
        {
            _dbContext = dbContext;
        }
        public async Task<bool> Create(Account account)
        {
            _dbContext.Accounts.Add(account);
            int created = await _dbContext.SaveChangesAsync();
            return created >= 1;
        }

        public List<Account> Get()
        {
            return _dbContext.Accounts.ToList();
        }
        public Account? Get(int id)
        {
            return _dbContext.Accounts.Find(id);
        }

        public async Task<bool> Update(Account account)
        {
            _dbContext.Accounts.Update(account);
            int updated = await _dbContext.SaveChangesAsync();
            return updated >= 1;
        }

        public async Task<bool> Delete(int id)
        {
            Account? account = _dbContext.Accounts.Find(id);
            if (account == null)
            {
                return false;
            }

            _dbContext.Accounts.Remove(account);
            int deleted = await _dbContext.SaveChangesAsync();
            return deleted >= 1;

        }
    }
}
