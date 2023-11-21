using Meter_Readings_API.Helpers;
using Meter_Readings_API.Models;
using Microsoft.EntityFrameworkCore;

namespace Meter_Readings_API.Data
{
    public class DatabaseContext : DbContext
    {
        public DbSet<MeterReading> MeterReadings { get; set; }
        public DbSet<Account> Accounts { get; set; }

        private ILogger<DatabaseContext> _logger { get; set; }
        public DatabaseContext(ILogger<DatabaseContext> logger)
        {
            _logger = logger;
        }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlite(@"Data Source = MeterReadings.db;");
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            try
            {
                List<Account> accounts = CsvHelper<Account>.ReadCsv(File.ReadAllText("Test_Accounts.csv"));
                modelBuilder.Entity<Account>().HasData(accounts);
            }
            catch(Exception ex) 
            { 
              _logger.LogError(ex.Message);
            }
            
        } 
    }
}
