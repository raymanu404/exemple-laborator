using System;
using CSharp.Choices;
using System.Collections.Generic;

namespace Domain.Models
{
    [AsChoice]
    public static partial class PlacingOrderEvent
    {
        public interface IPlacingOrderEvent { }
        public record PlacingOrderSuccedeedEvent : IPlacingOrderEvent
        {
            public decimal NumberOfOrder { get; }
            public DateTime PlacedDate { get; }

            public string Csv { get; }
            public IReadOnlyCollection<CalculateCustomerOrder> CalculatedOrder { get; }
            internal PlacingOrderSuccedeedEvent(IReadOnlyCollection<CalculateCustomerOrder> calculatedOrder,string csv, decimal numberOfOrder,DateTime placedDate)
            {
                CalculatedOrder = calculatedOrder;
                Csv = csv;
                NumberOfOrder = numberOfOrder;
                PlacedDate = placedDate;
            }
        }
        public record PlacingOrderFailedEvent : IPlacingOrderEvent
        {
            public string Reason { get; }
            internal PlacingOrderFailedEvent(string reason)
            {
                Reason = reason;
            }
        }
        

    }
}
