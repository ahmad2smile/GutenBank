using GutenBank.Models;
using System;

namespace GutenBank.Exceptions
{
    public class NotFoundException: BaseException
    {
        public NotFoundException(AccountDTO account): base(account)
        {
        }

        public override string Message { get; } = "Account not found";
    }
}