using GutenBank.Models;

namespace GutenBank.Exceptions
{
    public class UpdateNotAllowedException: BaseException
    {
        public UpdateNotAllowedException(AccountDTO account): base(account)
        {
        }
    }
}
