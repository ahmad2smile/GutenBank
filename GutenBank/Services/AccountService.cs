using GutenBank.Data;
using GutenBank.Domain;
using GutenBank.Exceptions;
using GutenBank.Models;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace GutenBank.Services
{
    public class AccountService : IAccountService
    {
        private readonly ApplicationContext _context;

        public AccountService(ApplicationContext context)
        {
            _context = context;
        }

        public async Task<AccountDTO> GetAccount(int accountNumber)
        {
            var account = await _context.Accounts
                    .AsNoTracking()
                    .FirstOrDefaultAsync(a => a.AccountNumber == accountNumber);

            if (account == null)
            {
                throw new NotFoundException(new AccountDTO
                {
                    AccountNumber = accountNumber,
                    Successful = false,
                    Message = new Message().ToString()
                });
            }

            return new AccountDTO
            {
                AccountNumber = accountNumber,
                Balance = account.Balance,
                Currency = account.Currency.ToString(),
                Successful = true,
                Message = new Message(TransactionStatus.Success).ToString()
            };
        }

        public Task<AccountDTO> Deposit(TransactionDTO transaction)
        {
            return ExecuteTransaction(transaction, TransactionType.Deposit);
        }

        public Task<AccountDTO> Withdraw(TransactionDTO transaction)
        {
            return ExecuteTransaction(transaction, TransactionType.Withdraw);
        }

        private async Task<AccountDTO> ExecuteTransaction(TransactionDTO transaction, TransactionType type)
        {
            var transactionStatus = TransactionStatus.Success;
            var account = await _context.Accounts.FindAsync(transaction.AccountNumber);

            try
            {
                if (account == null)
                {
                    throw new NotFoundException(new AccountDTO
                    {
                        AccountNumber = transaction.AccountNumber,
                        Successful = false,
                        Message = new Message().ToString()
                    });
                }

                // TODO: Handle Currency Conversion if needed
                var amount = transaction.Amount;

                if (type == TransactionType.Deposit)
                {
                    account.Balance += amount;
                }
                else if (account.Balance >= amount)
                {
                    account.Balance -= amount;
                }
                else
                {
                    transactionStatus = TransactionStatus.InsufficientBalanceError;

                    throw new InsufficientBalanceException(new AccountDTO
                    {
                        AccountNumber = transaction.AccountNumber,
                        Successful = false,
                        Message = new Message(transactionStatus).ToString()
                    });
                }

                await _context.SaveChangesAsync();


                return new AccountDTO
                {
                    AccountNumber = transaction.AccountNumber,
                    Balance = account.Balance,
                    Currency = account.Currency.ToString(),
                    Successful = true,
                    Message = new Message(transactionStatus).ToString()
                };
            }
            catch (DbUpdateConcurrencyException)
            {
                transactionStatus = TransactionStatus.UpdateNotAllowed;

                throw new UpdateNotAllowedException(new AccountDTO
                {
                    AccountNumber = transaction.AccountNumber,
                    Successful = false,
                    Message = new Message(transactionStatus).ToString()
                });
            }
            finally
            {
                _context.Entry(account).State = EntityState.Detached;
                await _context.SaveChangesAsync();

                var executedTransaction = new Transaction
                {
                    AccountNumber = transaction.AccountNumber,
                    Status = transactionStatus,
                    Type = type
                };

                await _context.Transactions.AddAsync(executedTransaction);
                await _context.SaveChangesAsync();
            }
        }
    }
}
