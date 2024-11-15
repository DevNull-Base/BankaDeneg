namespace SharedModels;

/// <summary>
/// Запрос на создание заявки на получение банковской карты.
/// </summary>
public struct CardRequest
{
    /// <summary>
    /// Идентификатор заявки.
    /// </summary>
    public string Id { get; set; }

    /// <summary>
    /// Валюта.
    /// </summary>
    public string Currency { get; set; }

    /// <summary>
    /// Идентификация пользователя.
    /// </summary>
    public PartyIdentification PartyIdentification { get; set; }

    /// <summary>
    /// Идентификация банка.
    /// </summary>
    public ServiceProvider ServiceProvider { get; set; }

    /// <summary>
    /// Дизайн карты.
    /// </summary>
    public CardDesign CardDesign { get; set; }

    /// <summary>
    /// Адрес доставки.
    /// </summary>
    public DeliveryAddress DeliveryAddress { get; set; }
}