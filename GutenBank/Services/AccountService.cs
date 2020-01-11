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
                    Balance = account.Balance,
                    Currency = account.Currency.ToString(),
                    Successful = true,
                    Message = new Message(TransactionStatus.Success).ToString()
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

            try
            {
                var account = await _context.Accounts.FindAsync(transaction.AccountNumber);

                // TODO: Handle Currency Conversion if needed
                var amount = transaction.Amount;

                if (type == TransactionType.Deposit)
                {
                    account.Balance += amount;
                }
                else if(account.Balance < amount)
                {
                    transactionStatus = TransactionStatus.InsufficientBalanceError;

                    throw new InsufficientBalanceException(new AccountDTO
                    {
                        AccountNumber = transaction.AccountNumber,
                        Successful = false,
                        Message = new Message(transactionStatus).ToString()
                    });
                }
                else
                {
                    account.Balance -= amount;
                }


                _context.Entry(account).State = EntityState.Modified;

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
                var executedTransaction = new Transaction
                {
                    AccountNumber = transaction.AccountNumber,
                    Status = transactionStatus,
                    Type = type
                };

                await _context.Transactions.AddAsync(executedTransaction);
            }
        }
    }
}
