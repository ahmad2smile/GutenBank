using GutenBank.Models;
using System;

namespace GutenBank.Exceptions
{
    public class BaseException: Exception
    {
        public BaseException(AccountDTO account)
        {
            Account = account;
        }

        public AccountDTO Account { get; }
    }
}
