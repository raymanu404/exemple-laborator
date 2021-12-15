using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Emanuel_Caprariu_lab6.Dto.Events;
using Emanuel_Caprariu_lab6.Events.Models;
using Emanuel_Caprariu_lab6.Events;

namespace Emanuel_Caprariu_lab6.Accomodation.EventProcessor
{
    internal class OrdersPlacedEventHandler : AbstractEventHandler<OrdersPlacingEvent>
    {
        public override string[] EventTypes => new string[] { typeof(OrdersPlacingEvent).Name };

        protected override Task<EventProcessingResult> OnHandleAsync(OrdersPlacingEvent eventData)
        {
            Console.WriteLine(eventData.ToString());
            return Task.FromResult(EventProcessingResult.Completed);
        }
    }
}
