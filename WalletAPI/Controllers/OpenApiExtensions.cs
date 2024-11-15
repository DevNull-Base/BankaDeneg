using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SharedModels;
using Swashbuckle.AspNetCore.Annotations;
using WalletAPI.Extensions;

namespace WalletAPI.Controllers;

public class OpenApiExtensions: ControllerBase
{
    /// <summary>
    /// Создание заявки на получение банковской карты.
    /// </summary>
    /// <param name="request">Запрос на создание заявки.</param>
    /// <returns>Созданная заявка.</returns>
    [HttpPost("v1/card/request")]
    [ProducesResponseType(typeof(CardApplicationResponse), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(ErrorResponse),StatusCodes.Status400BadRequest)]
    [SwaggerOperationFilter(typeof(AddRequiredHeadersFilter))]
    [Authorize]
    public IActionResult CreateApplication([FromBody] CardRequest request)
    {
        
        var application = new CardApplicationResponse
        {
            Id = Guid.NewGuid().ToString(),
            CreatedAt = DateTime.UtcNow.ToString("o"),
            Status = ApplicationStatus.New,
            Currency = request.Currency,
            PartyIdentification = request.PartyIdentification,
            ServiceProvider = request.ServiceProvider,
            CardDesign = request.CardDesign,
            DeliveryAddress = request.DeliveryAddress
        };

        return CreatedAtAction(nameof(GetApplication), new { applicationId = application.Id }, application);
    }

    /// <summary>
    /// Получение информации о заявке на получение банковской карты.
    /// </summary>
    /// <param name="applicationId">Идентификатор заявки.</param>
    /// <returns>Информация о заявке.</returns>
    [HttpGet("v1/card/request/{applicationId}")]
    [ProducesResponseType(typeof(CardApplicationResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ErrorResponse),StatusCodes.Status404NotFound)]
    [SwaggerOperationFilter(typeof(AddRequiredHeadersFilter))]
    [Authorize]
    public IActionResult GetApplication(string applicationId)
    {
        var application = new CardApplicationResponse
        {
            Id = applicationId,
            CreatedAt = DateTime.UtcNow.ToString("o"),
            Status = ApplicationStatus.New,
            Currency = "USD",
            PartyIdentification = new PartyIdentification
            {
                AccountSchemeName = AccountSchemeName.TXID,
                Identification = "1234567890"
            },
            ServiceProvider = new SharedModels.ServiceProvider
            {
                SchemeName = AccountSchemeName.BICFI,
                Identification = "044525225"
            },
            CardDesign = new CardDesign
            {
                DesignNumber = "1"
            },
            DeliveryAddress = new DeliveryAddress
            {
                Street = "ул. Пушкина, д. Колотушкина",
                City = "Ярославль",
                PostalCode = "150000",
                Country = "Россия"
            }
        };

        return Ok(application);
    }
    
    /// <summary>
    /// Получение уведомлений о статусе заявки банковской карты.
    /// </summary>
    /// <param name="applicationId">Идентификатор заявки.</param>
    /// <returns>Список уведомлений.</returns>
    [HttpGet("v1/card/{applicationId}/notifications")]
    [ProducesResponseType(typeof(IEnumerable<CardNotification>),StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ErrorResponse),StatusCodes.Status404NotFound)]
    [SwaggerOperationFilter(typeof(AddRequiredHeadersFilter))]
    [Authorize]
    public IActionResult GetNotifications(string applicationId)
    {
        var notifications = new List<CardNotification>
        {
            new CardNotification
            {
                Id = "12345",
                NotificationType = "ApplicationStatusChanged",
                CreatedAt = DateTime.UtcNow.ToString("o"),
                Message = "Статус заявки изменен на 'Approved'."
            },
            new CardNotification
            {
                Id = "67890",
                NotificationType = "CardReadyForIssuance",
                CreatedAt = DateTime.UtcNow.ToString("o"),
                Message = "Карта готова к выдаче."
            }
        };

        return Ok(notifications);
    }
    
    /// <summary>
    /// Получение всех подписок пользователя.
    /// </summary>
    /// <param name="partyIdentification">Идентификация пользователя.</param>
    /// <returns>Список подписок пользователя.</returns>
    [HttpGet("v1/subscription")]
    [ProducesResponseType(typeof(IEnumerable<Subscription>),StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [SwaggerOperationFilter(typeof(AddRequiredHeadersFilter))]
    [Authorize]
    public IActionResult GetUserSubscriptions([FromQuery] PartyIdentification partyIdentification)
    {
        var subscriptions = new List<Subscription>
        {
            new Subscription
            {
                Id = "12345",
                Organization = "Читай город",
                ProductName = "Читай 365",
                Cost = "299.99",
                Currency = "RUB",
                Url = "https://example.com/subscription/12345",
                NextPaymentDate = DateTime.UtcNow.AddMonths(1)
            },
            new Subscription
            {
                Id = "67890",
                Organization = "Яндекс",
                ProductName = "Яндекс Плюс",
                Cost = "179.99",
                Currency = "RUB",
                Url = "https://example.com/subscription/67890",
                NextPaymentDate = DateTime.UtcNow.AddYears(1)
            }
        };

        return Ok(subscriptions);
    }

    /// <summary>
    /// Получение детальной информации о подписке.
    /// </summary>
    /// <param name="subscriptionId">Идентификатор подписки.</param>
    /// <returns>Детальная информация о подписке.</returns>
    [HttpGet("v1/subscription/{subscriptionId}")]
    [ProducesResponseType(typeof(Subscription),StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ErrorResponse),StatusCodes.Status404NotFound)]
    [SwaggerOperationFilter(typeof(AddRequiredHeadersFilter))]
    [Authorize]
    public IActionResult GetSubscriptionDetails(string subscriptionId)
    {

        var subscription = new Subscription
        {
            Id = subscriptionId,
            Organization = "ЯрОблТранс",
            ProductName = "Проездной рабочего дня - 2 вида транспорта",
            Cost = "900.00",
            Currency = "RUB",
            Url = "https://example.com/subscription/12345",
            NextPaymentDate = DateTime.UtcNow.AddMonths(1)
        };

        return Ok(subscription);
    }

    /// <summary>
    /// Отказ от подписки.
    /// </summary>
    /// <param name="subscriptionId">Идентификатор подписки.</param>
    /// <returns>Результат отказа от подписки.</returns>
    [HttpDelete("v1/subscription/{subscriptionId}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(typeof(ErrorResponse),StatusCodes.Status404NotFound)]
    [SwaggerOperationFilter(typeof(AddRequiredHeadersFilter))]
    [Authorize]
    public IActionResult CancelSubscription(string subscriptionId)
    {
        return NoContent();
    }
}