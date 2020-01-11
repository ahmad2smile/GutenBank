namespace GutenBank.Domain
{
    public class Message
    {
        private readonly TransactionStatus _transactionStatus;

        public Message(TransactionStatus transactionStatus)
        {
            _transactionStatus = transactionStatus;
        }

        public Message()
        {
        }

        public override string ToString()
        {
            return _transactionStatus switch
            {
                TransactionStatus.Success => "Transaction was processed successfully",
                TransactionStatus.InsufficientBalanceError => "You have insufficient funds for transaction",
                TransactionStatus.UpdateNotAllowed=> "Update to account is not allowed right now, Please try again late",
                _ => "Something went wrong please try again later"
            };
        }
    }
}
