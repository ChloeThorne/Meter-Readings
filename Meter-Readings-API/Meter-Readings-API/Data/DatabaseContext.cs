using Meter_Readings_API.Helpers;
using Meter_Readings_API.Models;
using Microsoft.EntityFrameworkCore;

namespace Meter_Readings_API.Data
{
    public class DatabaseContext : DbContext
    {
        public DbSet<MeterReading> MeterReadings { get; set; }
        public DbSet<Account> Accounts { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlite(@"Data Source = MeterReadings.db;");
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            List<Account> accounts = CsvHelper<Account>.ReadCsv(File.ReadAllText("Test_Accounts.csv"));
            modelBuilder.Entity<Account>().HasData(accounts);
        } 
    }
}
