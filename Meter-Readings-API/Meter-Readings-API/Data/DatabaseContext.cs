using Meter_Readings_API.Helpers;
using Meter_Readings_API.Interfaces;
using Meter_Readings_API.Models;
using Microsoft.EntityFrameworkCore;

namespace Meter_Readings_API.Data
{
    public class DatabaseContext : DbContext
    {
        /// <summary>
        /// Gets or sets a database set of the <see cref="MeterReading"/> table.
        /// </summary>
        public DbSet<MeterReading> MeterReadings { get; set; }

        /// <summary>
        /// Gets or sets a database set of the <see cref="Account"/> table.
        /// </summary>
        public DbSet<Account> Accounts { get; set; }

        /// <summary>
        /// Gets or sets the database logger.
        /// </summary>
        private ILogger<DatabaseContext> logger { get; set; }

        /// <summary>
        /// Initialises a new instance of <see cref="DatabaseContext"/>.
        /// </summary>
        public DatabaseContext() { }

        /// <summary>
        /// Initialises a new instance of <see cref="DatabaseContext"/>.
        /// </summary>
        /// <param name="logger">The logger to use.</param>
        public DatabaseContext(ILogger<DatabaseContext> logger)
        {
            this.logger = logger;
        }


        /// <inheritdoc />
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlite(@"Data Source = MeterReadings.db;");
        }

        /// <inheritdoc />
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            try
            {
                ICsvHelper<Account> csvHelper = new CsvHelper<Account>();
                List<Account> accounts = csvHelper.ReadCsv(File.ReadAllText("Test_Accounts.csv"));
                modelBuilder.Entity<Account>().HasData(accounts);
            }
            catch(Exception ex) 
            { 
              logger.LogError(ex.Message);
            }
            
        } 
    }
}
