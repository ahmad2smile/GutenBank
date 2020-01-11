using GutenBank.Models;
using System;

namespace GutenBank.Exceptions
{
    public class InsufficientBalanceException: BaseException
    {
        public InsufficientBalanceException(AccountDTO account): base(account)
        {
        }
    }
}
