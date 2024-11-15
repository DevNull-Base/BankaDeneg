namespace SharedModels;

/// <summary>
/// Подписка пользователя.
/// </summary>
public struct Subscription
{
    /// <summary>
    /// Идентификатор подписки.
    /// </summary>
    public string Id { get; set; }

    /// <summary>
    /// Организация (юрлицо), на которую подписана подписка.
    /// </summary>
    public string Organization { get; set; }

    /// <summary>
    /// Название товара (подписки).
    /// </summary>
    public string ProductName { get; set; }

    /// <summary>
    /// Стоимость подписки в месяц.
    /// </summary>
    public string Cost { get; set; }

    /// <summary>
    /// Валюта подписки.
    /// </summary>
    public string Currency { get; set; }

    /// <summary>
    /// Ссылка (URL) на подписку.
    /// </summary>
    public string Url { get; set; }
    
    /// <summary>
    /// Дата следующего платежа.
    /// </summary>
    public DateTime NextPaymentDate { get; set; }
}