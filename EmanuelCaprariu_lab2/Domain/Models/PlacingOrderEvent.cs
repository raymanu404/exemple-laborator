using System;
using CSharp.Choices;

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
            internal PlacingOrderEventSuccedeedEvent(decimal numberOfOrder,DateTime placedDate)
            {
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
