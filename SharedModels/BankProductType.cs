namespace SharedModels;

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