namespace GutenBank.Domain
{
    public enum TransactionStatus
    {
        Success,
        InsufficientBalanceError,
        UpdateNotAllowed,
        UnknowError
    }
}