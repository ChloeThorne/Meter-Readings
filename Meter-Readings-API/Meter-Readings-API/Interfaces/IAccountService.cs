using Meter_Readings_API.Models;

namespace Meter_Readings_API.Interfaces
{
    public interface IAccountService
    {
        public List<Account> Get();
        public Account? Get(int id);
        public Task<bool> Update(Account account);
        public Task<bool> Delete(int id);
    }
}
