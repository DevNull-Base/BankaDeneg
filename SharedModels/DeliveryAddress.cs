namespace SharedModels;

/// <summary>
/// Адрес доставки банковской карты.
/// </summary>
public struct DeliveryAddress
{
    /// <summary>
    /// Улица.
    /// </summary>
    public string Street { get; set; }

    /// <summary>
    /// Город.
    /// </summary>
    public string City { get; set; }

    /// <summary>
    /// Страна.
    /// </summary>
    public string Country { get; set; }

    /// <summary>
    /// Почтовый индекс.
    /// </summary>
    public string PostalCode { get; set; }
}