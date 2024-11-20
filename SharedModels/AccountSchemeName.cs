using System.Runtime.Serialization;

namespace SharedModels;

/// <summary>
/// Схема идентификации банка.
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