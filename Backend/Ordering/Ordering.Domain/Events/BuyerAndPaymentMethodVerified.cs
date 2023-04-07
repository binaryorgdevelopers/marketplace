using MediatR;
using Ordering.Domain.AggregatesModel.BuyerAggregate;

namespace Ordering.Domain.Events;

public class BuyerAndPaymentMethodVerified : INotification
{
    public Buyer Buyer { get; private set; }
    public PaymentMethod Payment { get; private set; }
    public int OrderId { get; private set; }

    public BuyerAndPaymentMethodVerified(Buyer buyer, PaymentMethod payment, int orderId)
    {
        Buyer = buyer;
        Payment = payment;
        OrderId = orderId;
    }
}