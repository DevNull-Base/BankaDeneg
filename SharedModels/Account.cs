using System.Runtime.Serialization;

namespace SharedModels;

public struct Account
{
    public string Id { get; set; }
    public AccountStatus Status { get; set; }
    public string Currency { get; set; }
    public AccountType Type { get; set; }
    public BankProductType SubType { get; set; }
    
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

/// <summary>
/// Тип банковского продукта.
/// </summary>
public enum BankProductType
{
    /// <summary>
    /// Кредитная карта.
    /// </summary>
    CreditCard,

    /// <summary>
    /// Текущий счет.
    /// </summary>
    CurrentAccount,

    /// <summary>
    /// Кредит.
    /// </summary>
    Loan,

    /// <summary>
    /// Ипотека.
    /// </summary>
    Mortgage,

    /// <summary>
    /// Предоплаченная карта.
    /// </summary>
    PrePaidCard,

    /// <summary>
    /// Сберегательный счет.
    /// </summary>
    Savings
}

/// <summary>
/// Схема идентификации банка и клиента.
/// </summary>
public enum AccountSchemeName
{
    /// <summary>
    /// Персональный идентификационный номер (PAN).
    /// </summary>
    [EnumMember(Value = "RU.CBR.PAN")]
    PAN,

    /// <summary>
    /// Номер мобильного телефона.
    /// </summary>
    [EnumMember(Value = "RU.CBR.CellphoneNumber")]
    CellphoneNumber,

    /// <summary>
    /// Транзакционный идентификатор (TXID).
    /// </summary>
    [EnumMember(Value = "RU.CBR.TXID")]
    TXID,

    /// <summary>
    /// Основной банковский счет (BBAN).
    /// </summary>
    [EnumMember(Value = "RU.CBR.BBAN")]
    BBAN,

    /// <summary>
    /// Номер банковского счета.
    /// </summary>
    [EnumMember(Value = "RU.CBR.AccountNumber")]
    AccountNumber,

    /// <summary>
    /// Банковский идентификационный код (BICFI).
    /// </summary>
    [EnumMember(Value = "RU.CBR.BICFI")]
    BICFI,

    /// <summary>
    /// Банковский идентификационный код (BIK).
    /// </summary>
    [EnumMember(Value = "RU.CBR.BIK")]
    BIK,
}