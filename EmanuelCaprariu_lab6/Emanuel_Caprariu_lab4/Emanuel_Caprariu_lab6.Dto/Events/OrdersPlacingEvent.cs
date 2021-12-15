using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Emanuel_Caprariu_lab6.Dto.Models;

namespace Emanuel_Caprariu_lab6.Dto.Events
{
    public class OrdersPlacingEvent
    {
        public List<CartOrderDto> Orders { get; init; }
    }
}
