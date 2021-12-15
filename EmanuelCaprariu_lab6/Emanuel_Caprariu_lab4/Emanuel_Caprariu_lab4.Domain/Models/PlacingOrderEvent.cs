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
            public IEnumerable<PlacedCartOrder> Orders { get; }
            internal PlacingOrderSuccedeedEvent(IEnumerable<PlacedCartOrder> orders,string csv, decimal numberOfOrder,DateTime placedDate)
            {
                Orders = orders;
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
