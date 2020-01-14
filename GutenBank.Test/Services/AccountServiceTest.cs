using GutenBank.Data;
using GutenBank.Domain;
using GutenBank.Exceptions;
using GutenBank.Models;
using GutenBank.Test;
using Microsoft.EntityFrameworkCore;
using NUnit.Framework;
using System.Linq;
using System.Threading.Tasks;

namespace GutenBank.Services.Tests
{
    [TestFixture]
    public class AccountServiceTest
    {
        private ApplicationContext _dbContext;
        private AccountService _accountService;
        private int _setupCount;

        [SetUp]
        public void Init()
        {
            _setupCount += 1;
            _dbContext = new InMemoryDbFactory().SetupDb($"DbName{_setupCount}");
            _accountService = new AccountService(_dbContext);
        }

        [Test]
        public async Task GetAccountTest()
        {
            var account = await _accountService.GetAccount(1);

            Assert.AreEqual(1, account.AccountNumber);
            Assert.AreEqual(100, account.Balance);
            Assert.AreEqual(Currency.Dollar.ToString(), account.Currency);
        }

        [Test]
        public void AccountNotFound()
        {
            Assert.ThrowsAsync<NotFoundException>(async () => await _accountService.GetAccount(3));
        }

        [Test]
        public async Task DepositTest()
        {
            var transaction = new TransactionDTO
            {
                AccountNumber = 1,
                Amount = 50,
                Currency = Currency.Dollar.ToString()
            };

            var account = await _accountService.Deposit(transaction);

            Assert.AreEqual(account.Successful, true);
            Assert.AreEqual(account.Message, new Message(TransactionStatus.Success).ToString());
            Assert.AreEqual(account.Balance, 150);
        }

        [Test]
        public async Task WithdrawTest()
        {
            var transaction = new TransactionDTO
            {
                AccountNumber = 1,
                Amount = 100,
                Currency = Currency.Dollar.ToString()
            };

            var account = await _accountService.Withdraw(transaction);

            Assert.AreEqual(account.Successful, true);
            Assert.AreEqual(account.Message, new Message(TransactionStatus.Success).ToString());
            Assert.AreEqual(account.Balance, 0);
        }

        [Test]
        public void NotEnoughBalanceForWithdraw()
        {
            var transaction = new TransactionDTO
            {
                AccountNumber = 1,
                Amount = 150,
                Currency = Currency.Dollar.ToString()
            };

            Assert.ThrowsAsync<InsufficientBalanceException>(async () => await _accountService.Withdraw(transaction));
        }

        [Test]
        public async Task GetTransactions()
        {
            var transaction = new TransactionDTO
            {
                AccountNumber = 1,
                Amount = 50,
                Currency = Currency.Dollar.ToString()
            };

            await _accountService.Deposit(transaction);

            await _accountService.Withdraw(transaction);
            await _accountService.Withdraw(transaction);
            await _accountService.Withdraw(transaction);

            var desposites = _dbContext.Transactions.Where(t => t.Type == TransactionType.Deposit).Count();
            var withdraws = _dbContext.Transactions.Where(t => t.Type == TransactionType.Withdraw).Count();

            Assert.GreaterOrEqual(desposites, 1);
            Assert.GreaterOrEqual(withdraws, 3);
        }


        [Test]
        public void NotFoundAccountForTransaction()
        {
            var transaction = new TransactionDTO
            {
                AccountNumber = 3,
                Amount = 150,
                Currency = Currency.Dollar.ToString()
            };

            Assert.ThrowsAsync<NotFoundException>(async () => await _accountService.Withdraw(transaction));
        }

        [Test]
        public async Task ConcurrencyError()
        {
            var account = _dbContext.Accounts.FirstOrDefault(a => a.AccountNumber == 1);

            _dbContext.Entry(account).State = EntityState.Deleted;

            await _dbContext.SaveChangesAsync(); // Version Got Updated

            _dbContext.Entry(account).State = EntityState.Modified; // Modified after Entity in Db was changed

            Assert.ThrowsAsync<DbUpdateConcurrencyException>(() => _dbContext.SaveChangesAsync());  // Old Version
        }
    }
}
