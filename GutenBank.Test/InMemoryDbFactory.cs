using GutenBank.Data;
using GutenBank.Domain;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;

namespace GutenBank.Test
{
    public class InMemoryDbFactory
    {
        public ApplicationContext SetupDb(string dbName)
        {
            var options = new DbContextOptionsBuilder<ApplicationContext>()
                            .UseInMemoryDatabase(dbName)
                            .Options;
            var dbContext = new ApplicationContext(options);

            dbContext.Accounts.RemoveRange(dbContext.Accounts);
            dbContext.Transactions.RemoveRange(dbContext.Transactions);

            dbContext.Accounts.AddRange(new List<Account> {
                new Account {
                    Balance = 100,
                    Currency = Currency.Dollar,
                },
                new Account {
                    Balance = 200,
                    Currency = Currency.Dollar,
                }
            });

            dbContext.SaveChanges();

            return dbContext;
        }
    }
}
