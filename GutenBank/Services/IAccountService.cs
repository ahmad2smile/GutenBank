using GutenBank.Models;
using System.Threading.Tasks;

namespace GutenBank.Services
{
    public interface IAccountService
    {
        public Task<AccountDTO> GetAccount(int accountNumber);
        public Task<AccountDTO> Deposit(TransactionDTO transaction);
        public Task<AccountDTO> Withdraw(TransactionDTO transaction);
    }
}
