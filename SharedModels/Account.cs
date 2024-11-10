using System.Runtime.Serialization;

namespace SharedModels;

public struct Account
{
    public string Id { get; set; }
    public AccountStatus Status { get; set; }
    public string Currency { get; set; }
    public AccountType Type { get; set; }
    public AccountSubType SubType { get; set; }
    
    public AccountSchemeName SchemeName { get; set; }
    public string? Identification { get; set; }
    public string? Name { get; set; }
    
    public AccountSchemeName ServiceProviderSchemeName { get; set; }
    public string ServiceProviderIdentification { get; set; }
}

public enum AccountStatus
{
    Enabled,
    Disabled,
    Deleted
}

public enum AccountType
{
    Business,
    Personal,
}

public enum AccountSubType
{
    CreditCard,
    CurrentAccount,
    Loan,
    Mortgage,
    PrePaidCard,
    Savings
}

public enum AccountSchemeName
{
    [EnumMember(Value = "RU.CBR.PAN")]
    PAN,

    [EnumMember(Value = "RU.CBR.CellphoneNumber")]
    CellphoneNumber,

    [EnumMember(Value = "RU.CBR.BBAN")]
    BBAN,
    
    [EnumMember(Value = "RU.CBR.AccountNumber")]
    AccountNumber,
    
    [EnumMember(Value = "RU.CBR.BICFI")]
    BICFI,
    
    [EnumMember(Value = "RU.CBR.BIK")]
    BIK,
}