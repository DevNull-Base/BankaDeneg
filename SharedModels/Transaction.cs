namespace SharedModels;

public class Transaction
{
    public string AccountId { get; set; }
    public string Id { get; set; }
    public string Reference { get; set; }
    public TransactionType Type { get; set; }
    public TransactionStatus Status { get; set; }
    public string Amount { get; set; }
    public string Currency { get; set; }
}

public enum TransactionType
{
    Credit,
    Debit
}

public enum TransactionStatus
{
    Booked,
    Pending
}