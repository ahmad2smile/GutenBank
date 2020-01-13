using GutenBank.Domain;
using Microsoft.EntityFrameworkCore;

namespace GutenBank.Data
{
    public class ApplicationContext: DbContext
    {
        public ApplicationContext(DbContextOptions options): base(options)
        {
        }

        public DbSet<Account> Accounts { get; set; }
        public DbSet<Transaction> Transactions { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Account>()
                .HasData(
                    new Account
                    {
                        AccountNumber = 1,
                        Balance = 50,
                        Currency = Currency.Dollar
                    },
                    new Account
                    {
                        AccountNumber = 2,
                        Balance = 100,
                        Currency = Currency.Dollar
                    }
            );
        }
    }
}
