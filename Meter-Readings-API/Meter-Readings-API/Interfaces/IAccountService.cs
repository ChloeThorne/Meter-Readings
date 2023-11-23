using Meter_Readings_API.Models;

namespace Meter_Readings_API.Interfaces
{
    public interface IAccountService
    {
        /// <summary>
        /// Creates an account entry in the database.
        /// </summary>
        /// <param name="account">The account to add to the database.</param>
        /// <returns>An awaitable Task containing if the creation was a success.</returns>
        public Task<bool> Create(Account account);

        /// <summary>
        /// Get all account entries from the database.
        /// </summary>
        /// <returns>A <see cref="List{Account}"/> containing all accounts in the database.</returns>
        public List<Account> Get();

        /// <summary>
        /// Gets a single account entry from the database.
        /// </summary>
        /// <param name="id">The ID of the account.</param>
        /// <returns>An instance of <see cref="Account"/> representing the database entry.</returns>
        public Account? Get(int id);

        /// <summary>
        /// Update an account entry in the database.
        /// </summary>
        /// <param name="account">The new account entry.</param>
        /// <returns>An awaitable Task containing if the update was a success.</returns>
        public Task<bool> Update(Account account);

        /// <summary>
        /// Delete an entry by ID in the database.
        /// </summary>
        /// <param name="id">The ID of the entity to delete.</param>
        /// <returns>An awaitable Task containing if the delete was a success.</returns>
        public Task<bool> Delete(int id);
    }
}
