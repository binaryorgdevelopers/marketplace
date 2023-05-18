using MediatR;
using Microsoft.Extensions.Logging;
using Ordering.Application.IntegrationEvents;
using Ordering.Application.IntegrationEvents.Events;
using Ordering.Domain.AggregatesModel.OrderAggregate;

namespace Ordering.Application.Commands;

public class CreateOrderCommandHandler : IRequestHandler<CreateOrderCommand, bool>
{
    private readonly IOrderRepository _orderRepository;
    private readonly IOrderingIntegrationEventService _orderingIntegrationEventService;
    private readonly ILogger<CreateOrderCommandHandler> _logger;

    public CreateOrderCommandHandler(IOrderRepository orderRepository,
        IOrderingIntegrationEventService orderingIntegrationEventService, ILogger<CreateOrderCommandHandler> logger)
    {
        _orderRepository = orderRepository ?? throw new ArgumentException(nameof(orderRepository));
        _orderingIntegrationEventService = orderingIntegrationEventService ??
                                           throw new ArgumentNullException(nameof(orderingIntegrationEventService));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    public async Task<bool> Handle(CreateOrderCommand request, CancellationToken cancellationToken)
    {
        //Add integration event to clean Basket
        var orderStartedIntegrationEvent = new OrderStartedIntegrationEvent(request.UserId);

        await _orderingIntegrationEventService.AddAndSaveEventAsync(orderStartedIntegrationEvent);

        //Add/Update the Buyer Aggregate


        var address = new Address(request.Street, request.City, request.State, request.Country, request.ZipCode);

        var order = new Order(request.UserId, request.UserName, address, request.CardTypeId, request.CardNumber,
            request.CardSecurityNumber, request.CardHolderName, request.CardExpiration);

        foreach (var item in request.OrderItems)
        {
            order.AddOrderItem(item.ProductId, item.ProductName, item.UnitPrice, item.Discount, item.PictureUrl,
                item.Units);
        }

        _logger.LogInformation("-----Creating Order - Order :{@Order}", order);
        _orderRepository.Add(order); 

        return await _orderRepository.UnitOfWork.SaveEntitiesAsync(cancellationToken);
    }
}