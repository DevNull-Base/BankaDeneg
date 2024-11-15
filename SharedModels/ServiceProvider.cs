namespace SharedModels;

/// <summary>
/// Идентификация банка.
/// </summary>
public struct ServiceProvider
{
    /// <summary>
    /// Схема идентификации банка.
    /// </summary>
    public AccountSchemeName SchemeName { get; set; }

    /// <summary>
    /// Идентификатор банка.
    /// </summary>
    public string Identification { get; set; }
}