using System;
using CSharp.Choices;
using System.Collections.Generic;
namespace Domain.Models
{
    [AsChoice]
    public static partial class PlacingOrderEvent
    {
        public interface IPlacingOrderEvent { }
        public record PlacingOrderEventSuccedeedEvent : IPlacingOrderEvent
        {
            public decimal NumberOfOrder { get; }
            public DateTime PlacedDate { get; }
            public IReadOnlyCollection<CalculateCustomerOrder> CalculatedOrder { get; }
            internal PlacingOrderEventSuccedeedEvent(IReadOnlyCollection<CalculateCustomerOrder> calculatedOrder, decimal numberOfOrder,DateTime placedDate)
            {
                CalculatedOrder = calculatedOrder;
                NumberOfOrder = numberOfOrder;
                PlacedDate = placedDate;
            }
        }
        public record PlacingOrderEventFailedEvent : IPlacingOrderEvent
        {
            public string Reason { get; }
            internal PlacingOrderEventFailedEvent(string reason)
            {
                Reason = reason;
            }
        }
    }
}
