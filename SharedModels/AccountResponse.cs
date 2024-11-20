namespace SharedModels;

public struct AccountResponse
{
    public string AccountId { get; set; }
    public string Amount { get; set; }
    public string Currency { get; set; }
    public string BankName { get; set; }
}