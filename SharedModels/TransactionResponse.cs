namespace SharedModels;

public struct TransactionResponse
{
    public string Amount { get; set; }
    public string Currency { get; set; }
    public string BankName { get; set; }
    public string Type { get; set; }
    public string Title { get; set; }
}