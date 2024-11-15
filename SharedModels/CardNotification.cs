namespace SharedModels;

/// <summary>
/// Уведомление о статусе банковской карты.
/// </summary>
public struct CardNotification
{
    /// <summary>
    /// Идентификатор уведомления.
    /// </summary>
    public string Id { get; set; }

    /// <summary>
    /// Тип уведомления.
    /// </summary>
    public string NotificationType { get; set; }

    /// <summary>
    /// Дата создания уведомления.
    /// </summary>
    public string CreatedAt { get; set; }

    /// <summary>
    /// Текст уведомления.
    /// </summary>
    public string Message { get; set; }
}